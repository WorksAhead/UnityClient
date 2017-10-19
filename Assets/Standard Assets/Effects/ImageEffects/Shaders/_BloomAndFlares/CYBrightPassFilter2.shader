// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CY/BrightPassFilter"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "" {}
		_LumTex("Color", 2D) = "" {}
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f 
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;	
	sampler2D _LumTex;
	
	half4 _BloomParams;	// x = Threshhold	y = brightOffset	z = HDRBrightLevel w = HDRBloomMul
		
	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv =  v.texcoord.xy;
		return o;
	} 

	half4 fragBrightPass(v2f i) : SV_Target
	{
		half4 vSample = tex2D(_MainTex, i.uv); // fetch half res scene target and apply range scale
		half fAdaptedLum = tex2D(_LumTex, i.uv).x;

		half Level = _BloomParams.z * _BloomParams.w;

		// Determine what the pixel's value will be after tone-mapping occurs
		vSample.rgb *= Level / (fAdaptedLum + 1e-6);
		vSample.rgb -= _BloomParams.x;
		vSample.rgb = max(vSample.rgb, (half3)0.0);
		vSample.rgb /= (_BloomParams.y + vSample.rgb); // map result to 0 to 1 range

		return vSample * 8.0; // scale range to keep some precision in darks (gets rescaled down to original value during final tone map scene)
	}

	half4 SampleLumOffsets0;
	half4 SampleLumOffsets1;

	half4 HDRSampleLumInitialPS(v2f i) : SV_Target
	{
		half4 color = 0;
		half fRecipSampleCount = 0.25f;
		half2 vLumInfo = half2(0.0f, 64.0f);

		half3 cTex = tex2D(_MainTex, i.uv + SampleLumOffsets0.xy).rgb;
		half fLum = Luminance(cTex.rgb);

		vLumInfo.x += fLum;
		vLumInfo.y = min(vLumInfo.y, fLum);

		cTex = tex2D(_MainTex, i.uv + SampleLumOffsets0.zw).rgb;
		fLum = Luminance(cTex.rgb);

		vLumInfo.x += fLum;
		vLumInfo.y = min(vLumInfo.y, fLum);

		cTex = tex2D(_MainTex, i.uv + SampleLumOffsets1.xy).rgb;
		fLum = Luminance(cTex.rgb);

		vLumInfo.x += fLum;
		vLumInfo.y = min(vLumInfo.y, fLum);

		cTex = tex2D(_MainTex, i.uv + SampleLumOffsets1.zw).rgb;
		fLum = Luminance(cTex.rgb);

		vLumInfo.x += fLum;
		vLumInfo.y = min(vLumInfo.y, fLum);

		// clamp to acceptable range
		color.xy = min(vLumInfo.xy * half2(fRecipSampleCount, 1), 64);

		return color;
	}

	float4 SampleOffsets[16];

	half4 HDRSampleLumIterativePS(v2f i) : SV_Target
	{
		int nIter = 4;
		half nRecipIter = 1.0f / (half) nIter;
		half4 vResampleSum = 0.0f;

		for (int iter = 0; iter<nIter; iter++)
		{
			// Compute the sum of luminance throughout the sample points
			half2 vTex = tex2D(_MainTex, i.uv + SampleOffsets[iter].xy);
			vResampleSum.xy += vTex.xy;
		}

		// Divide the sum to complete the average
		//vResampleSum.x *= nRecipIter;
		vResampleSum.xy *= nRecipIter;

		return vResampleSum;
	}

	ENDCG 
	
	Subshader 
	{
		// 0
		Pass
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment fragBrightPass

			ENDCG
		}
		// 1
		Pass
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment HDRSampleLumInitialPS

			ENDCG
		}
		// 2
		Pass
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment HDRSampleLumIterativePS

			ENDCG
		}
	}
	Fallback off
}
