using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof (Camera))]
    [AddComponentMenu("Image Effects/Color Adjustments/CY_Tonemapping")]
    public class CYTonemapping : PostEffectsBase
    {
        private Material tonemapMaterial;
        private Shader tonemapper;
        public float FilmicShoulderScale = 1.6f;
        public float FilmicMidtoneScale = 0.84f;
        public float FilmicToeScale = 1.0f;
        public float FilmicWhitePoint = 2.0f;

        public override bool CheckResources()
        {
            CheckSupport(false, true);

            if (!tonemapper)
            {
                tonemapper = Shader.Find("CY/FilmicTonemapper");
            }

            tonemapMaterial = CheckShaderAndCreateMaterial(tonemapper, tonemapMaterial);

            if (!isSupported)
                ReportAutoDisable();
            return isSupported;
        }

        private void OnDisable()
        {
            if (tonemapMaterial)
            {
                DestroyImmediate(tonemapMaterial);
                tonemapMaterial = null;
            }
        }


        private bool CreateInternalRenderTexture()
        {
            return true;
        }


        // attribute indicates that the image filter chain will continue in LDR
        [ImageEffectTransformsToLDR]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (CheckResources() == false)
            {
                Graphics.Blit(source, destination);
                return;
            }

                CYBloom bloom = gameObject.GetComponent<CYBloom>();

                tonemapMaterial.SetTexture("_BloomTex", bloom.enabled ? bloom.rtBloomFinal : null);
                tonemapMaterial.SetTexture("_LumTex", bloom.enabled ? bloom.rtAvgLuminance : null);
                tonemapMaterial.SetFloat("_BloomIntensity", bloom.bloomIntensity);
                tonemapMaterial.SetFloat("_FilmicShoulderScale", FilmicShoulderScale);
                tonemapMaterial.SetFloat("_FilmicMidtoneScale", FilmicMidtoneScale);
                tonemapMaterial.SetFloat("_FilmicToeScale", FilmicToeScale);
                tonemapMaterial.SetFloat("_FilmicWhitePoint", FilmicWhitePoint);
                Graphics.Blit(source, destination, tonemapMaterial, 0);
        }
    }
}
