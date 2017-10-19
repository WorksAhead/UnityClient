using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    [AddComponentMenu ("Image Effects/Bloom and Glow/CY_Bloom")]
    public class CYBloom : PostEffectsBase
    {
        public float bloomIntensity = 0.5f;
        public float bloomThreshold = 6.0f;
        public float brightOffset = 5.0f;
        public float HDRBrightLevel = 1.25f;
        public float HDRBloomMul = 0.7f;

        public RenderTexture rtBloomFinal;
        public RenderTexture rtAvgLuminance;

        public Shader screenBlendShader;
        private Material screenBlend;

        public Shader brightPassFilterShader;
        private Material brightPassFilterMaterial;


        public override bool CheckResources ()
        {
            CheckSupport (false);

            if (!screenBlendShader)
            {
                screenBlendShader = Shader.Find("CY/BlendForBloom");
            }
            if (!brightPassFilterShader)
            {
                brightPassFilterShader = Shader.Find("CY/BrightPassFilter");
            }

            screenBlend = CheckShaderAndCreateMaterial (screenBlendShader, screenBlend);
            brightPassFilterMaterial = CheckShaderAndCreateMaterial(brightPassFilterShader, brightPassFilterMaterial);

            if (!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }

        private static float GaussianDistribution1D(float x, float rho)
        {
            float g = 1.0f / (rho * Mathf.Sqrt(2.0f * Mathf.PI));
            g *= Mathf.Exp(-(x * x) / (2.0f * rho * rho));
            return g;
        }

        private void GetGaussBlurParams(int nTexWidth, int nTexHeight, Vector4[] pHParams, Vector4[] pVParams, Vector4[] pWeightsPS)
        {
            // setup texture offsets, for texture sampling
            float s1 = 1.0f / nTexWidth;
            float t1 = 1.0f / nTexHeight;

            // Horizontal/Vertical pass params
            const int nSamples = 16;
            const float fDistribution = 3.0f;
            int nHalfSamples = (nSamples >> 1);

            float[] pWeights = new float[32]; 
            float fWeightSum = 0;

            int s;
            for (s = 0; s < nSamples; ++s)
            {
                pWeights[s] = GaussianDistribution1D(s - nHalfSamples, fDistribution);
                fWeightSum += pWeights[s];
            }

            // normalize weights
            for (s = 0; s < nSamples; ++s)
            {
                pWeights[s] /= fWeightSum;
            }

            Vector4 vWhite = new Vector4(1.0f, 1.0f, 1.0f, 1.0f );
            // set bilinear offsets
            for (s = 0; s < nHalfSamples; ++s)
            {
                float off_a = pWeights[s * 2];
                float off_b = ((s * 2 + 1) <= nSamples - 1) ? pWeights[s * 2 + 1] : 0;
                float a_plus_b = (off_a + off_b);
                if (a_plus_b == 0)
                    a_plus_b = 1.0f;
                float offset = off_b / a_plus_b;

                pWeights[s] = off_a + off_b;
                pWeightsPS[s] = vWhite * pWeights[s];

                float fCurrOffset = (float)s * 2 + offset - nHalfSamples;
                pHParams[s] = new Vector4(s1 * fCurrOffset, 0, 0, 0);
                pVParams[s] = new Vector4(0, t1 * fCurrOffset, 0, 0);
            }
        }

        private void GetSampleOffsets_DownScale4x4Bilinear(int nWidth, int nHeight, Vector4[] avSampleOffsets)
        {
            float tU = 1.0f / (float)nWidth;
            float tV = 1.0f / (float)nHeight;

            // Sample from the 16 surrounding points.  Since bilinear filtering is being used, specific the coordinate
            // exactly halfway between the current texel center (k-1.5) and the neighboring texel center (k-0.5)

            int index = 0;
            for (int y = 0; y < 4; y += 2)
            {
                for (int x = 0; x < 4; x += 2, index++)
                {
                    avSampleOffsets[index].x = (x - 1.0f) * tU;
                    avSampleOffsets[index].y = (y - 1.0f) * tV;
                    avSampleOffsets[index].z = 0;
                    avSampleOffsets[index].w = 1;
                }
            }
        }

        private void MeasureLuminance(RenderTexture rtScaled)
        {
            // create luminance rts
            const int NUM_HDR_TONEMAP_TEXTURES = 4;
            int dwCurTexture = NUM_HDR_TONEMAP_TEXTURES - 1;
            RenderTexture[] rtLuminances = new RenderTexture[NUM_HDR_TONEMAP_TEXTURES];

            for (int i = 0; i < NUM_HDR_TONEMAP_TEXTURES; i++)
            {
                int iSampleLen = 1 << (2 * i);
                rtLuminances[i] = RenderTexture.GetTemporary(iSampleLen, iSampleLen, 0, RenderTextureFormat.RGHalf);
            }

            float tU, tV;
            tU = 1.0f / (3.0f * rtLuminances[dwCurTexture].width);
            tV = 1.0f / (3.0f * rtLuminances[dwCurTexture].height);

            Vector4[] avSampleOffsets = new Vector4[16];
            int index = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    avSampleOffsets[index].x = x * tU;
                    avSampleOffsets[index].y = y * tV;
                    avSampleOffsets[index].z = 0;
                    avSampleOffsets[index].w = 1;

                    index++;
                }
            }

            float s1 = 1.0f / rtScaled.width;
            float t1 = 1.0f / rtScaled.height;

            // Use rotated grid
            Vector4 vSampleLumOffsets0 = new Vector4(s1 * 0.95f, t1 * 0.25f, -s1 * 0.25f, t1 * 0.96f);
            Vector4 vSampleLumOffsets1 = new Vector4(-s1 * 0.96f, -t1 * 0.25f, s1 * 0.25f, -t1 * 0.96f);

            brightPassFilterMaterial.SetVector("SampleLumOffsets0", vSampleLumOffsets0);
            brightPassFilterMaterial.SetVector("SampleLumOffsets1", vSampleLumOffsets1);

            // initial pass
            Graphics.Blit(rtScaled, rtLuminances[dwCurTexture], brightPassFilterMaterial, 1);

            // iterative luminance passes
            dwCurTexture--;

            while (dwCurTexture >= 0)
            {
                GetSampleOffsets_DownScale4x4Bilinear(rtLuminances[dwCurTexture + 1].width, rtLuminances[dwCurTexture + 1].height, avSampleOffsets);
                brightPassFilterMaterial.SetVectorArray("SampleOffsets", avSampleOffsets);
                Graphics.Blit(rtLuminances[dwCurTexture + 1], rtLuminances[dwCurTexture], brightPassFilterMaterial, 2);

                dwCurTexture--;
            }

            RenderTexture.ReleaseTemporary(rtAvgLuminance);
            rtAvgLuminance = RenderTexture.GetTemporary(1, 1, 0, RenderTextureFormat.RGHalf);
            Graphics.Blit(rtLuminances[0], rtAvgLuminance);

            // cleanup
            for (int i = 0; i < NUM_HDR_TONEMAP_TEXTURES; i++)
            {
                 RenderTexture.ReleaseTemporary(rtLuminances[i]);
            }
        }

        public void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            var rtFormat = RenderTextureFormat.ARGBHalf;
            var rtW2 = source.width / 2;
            var rtH2 = source.height / 2;
            var rtW4 = source.width / 4;
            var rtH4 = source.height / 4;

            // downsample
            RenderTexture quarterRezColor = RenderTexture.GetTemporary(rtW4, rtH4, 0, rtFormat);
            RenderTexture halfRezColorDown = RenderTexture.GetTemporary(rtW2, rtH2, 0, rtFormat);

            Graphics.Blit(source, halfRezColorDown);
            Graphics.Blit(halfRezColorDown, quarterRezColor);

            RenderTexture.ReleaseTemporary(halfRezColorDown);

            // measure luminance
            MeasureLuminance(quarterRezColor);

            // bright pass
            RenderTexture secondQuarterRezColor = RenderTexture.GetTemporary(rtW4, rtH4, 0, rtFormat);
            BrightFilter(bloomThreshold, quarterRezColor, secondQuarterRezColor);

            // blurring from CE3
            RenderTexture blur1_h = RenderTexture.GetTemporary(rtW4, rtH4, 0, rtFormat);
            RenderTexture blur1_v = RenderTexture.GetTemporary(rtW4, rtH4, 0, rtFormat);
            RenderTexture blur2_h = RenderTexture.GetTemporary(rtW4>>1, rtH4>>1, 0, rtFormat);
            RenderTexture blur2_v = RenderTexture.GetTemporary(rtW4>>1, rtH4>>1, 0, rtFormat);
            RenderTexture blur3_h = RenderTexture.GetTemporary(rtW4>>2, rtH4>>2, 0, rtFormat);
            RenderTexture blur3_v = RenderTexture.GetTemporary(rtW4>>2, rtH4>>2, 0, rtFormat);

            Vector4[] psWeights = new Vector4[32];
            Vector4[] pHParams = new Vector4[32];
            Vector4[] pVParams = new Vector4[32];

            // blur 1
            GetGaussBlurParams(blur1_h.width, blur1_h.height, pHParams, pVParams, psWeights);

            screenBlend.SetVectorArray("psWeights", psWeights);
            screenBlend.SetVectorArray("PI_psOffsets", pHParams);
            Graphics.Blit(secondQuarterRezColor, blur1_h, screenBlend, 1);
            screenBlend.SetVectorArray("PI_psOffsets", pVParams);
            Graphics.Blit(blur1_h, blur1_v, screenBlend, 1);

            // blur 2
            GetGaussBlurParams(blur2_h.width, blur2_h.height, pHParams, pVParams, psWeights);

            Graphics.Blit(blur1_v, blur2_v);
            screenBlend.SetVectorArray("PI_psOffsets", pHParams);
            Graphics.Blit(blur2_v, blur2_h, screenBlend, 1);
            screenBlend.SetVectorArray("PI_psOffsets", pVParams);
            Graphics.Blit(blur2_h, blur2_v, screenBlend, 1);

            // blur 3
            GetGaussBlurParams(blur3_h.width, blur3_h.height, pHParams, pVParams, psWeights);

            Graphics.Blit(blur2_v, blur3_v);
            screenBlend.SetVectorArray("PI_psOffsets", pHParams);
            Graphics.Blit(blur3_v, blur3_h, screenBlend, 1);
            screenBlend.SetVectorArray("PI_psOffsets", pVParams);
            Graphics.Blit(blur3_h, blur3_v, screenBlend, 1);

             RenderTexture.ReleaseTemporary(rtBloomFinal);
             rtBloomFinal = RenderTexture.GetTemporary(rtW4, rtH4, 0, rtFormat);

            screenBlend.SetFloat("_Intensity", bloomIntensity);
            screenBlend.SetTexture("_Bloom1", blur1_v);
            screenBlend.SetTexture("_Bloom2", blur2_v);
            screenBlend.SetTexture("_Bloom3", blur3_v);
            Graphics.Blit(source, rtBloomFinal, screenBlend, 0);

            RenderTexture.ReleaseTemporary(blur1_h);
            RenderTexture.ReleaseTemporary(blur1_v);
            RenderTexture.ReleaseTemporary(blur2_h);
            RenderTexture.ReleaseTemporary(blur2_v);
            RenderTexture.ReleaseTemporary(blur3_h);
            RenderTexture.ReleaseTemporary(blur3_v);
            RenderTexture.ReleaseTemporary (quarterRezColor);
            RenderTexture.ReleaseTemporary (secondQuarterRezColor);

            Graphics.Blit(source, destination);
        }

        private void BrightFilter (float thresh, RenderTexture from, RenderTexture to)
        {
            brightPassFilterMaterial.SetTexture("_LumTex", rtAvgLuminance);
            brightPassFilterMaterial.SetVector ("_BloomParams", new Vector4 (thresh, brightOffset, HDRBrightLevel, HDRBloomMul));
            Graphics.Blit (from, to, brightPassFilterMaterial, 0);
        }

    }
}
