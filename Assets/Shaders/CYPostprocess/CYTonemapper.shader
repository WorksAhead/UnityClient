// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CY/FilmicTonemapper" {
	Properties {
		_MainTex ("", 2D) = "black" {}
		_LumTex ("", 2D) = "grey" {}
		_BloomTex("", 2D) = "black" {}
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	 
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	sampler2D _LumTex;
	sampler2D _BloomTex;
	

	float _BloomIntensity;
	float _FilmicShoulderScale;
	float _FilmicMidtoneScale;
	float _FilmicToeScale;
	float _FilmicWhitePoint;
	
	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 

	half4 DecodeRGBK(in half4 Color, const half fMultiplier, bool bUsePPP = false)
	{
		if (bUsePPP)
		{
			//Color.rgb *= Color.rgb * (Color.a * Color.a) * fMultiplier;

			Color.rgb *= (Color.a * Color.a) * fMultiplier;
		}
		else
			Color.rgb *= Color.a * fMultiplier;

		return Color;
	}
	#define SCENE_HDR_MULTIPLIER 32.f

	float4 fragFilmicToneMapping(v2f i) : SV_Target
	{
		float4 cColor = tex2D(_MainTex, i.uv );
		half fAdaptedLum = tex2D(_LumTex, i.uv).x;
		float4 cBloom = DecodeRGBK(tex2D(_BloomTex, i.uv), SCENE_HDR_MULTIPLIER, true);

		// Krawczyk scene key estimation adjusted to better fit our range - low (0.05) to high key (0.8) interpolation based on avg scene luminance
		const half fSceneKey = 1.03f - 2.0f / (2.0f + log2(fAdaptedLum + 1.0f));

		// Exposure compensation -/+1.5 f-stops
		half exposure = clamp(fSceneKey / fAdaptedLum, 0.36, 2.8);

		float4 cBloomColor = float4(1, 1, 1, 1);
		cBloom *= cBloomColor * _BloomIntensity * 3.0 / 8.0;

		cColor = cColor * exposure + cBloom * 0.5f;

		// Filmic response curve as proposed by J. Hable
		float4 HDRFilmCurve = float4(_FilmicShoulderScale, _FilmicMidtoneScale, _FilmicToeScale, _FilmicWhitePoint);
		half4 c = half4(max(cColor.rgb, 0), HDRFilmCurve.w);
		half ShoStren = 0.22 * HDRFilmCurve.x, LinStren = 0.3 * HDRFilmCurve.y, LinAngle = 0.1, ToeStren = 0.2, ToeNum = 0.01 * HDRFilmCurve.z, ToeDenom = 0.3;
		half4 compressedCol = ((c * (ShoStren * c + LinAngle*LinStren) + ToeStren*ToeNum) / (c * (ShoStren * c + LinStren) + ToeStren*ToeDenom)) - (ToeNum/ToeDenom);
		cColor.xyz = saturate(compressedCol / compressedCol.w);

		return cColor;
	}
	
	
	ENDCG 
	
Subshader {
   // Filmic tonemapping from CE3
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragFilmicToneMapping
      ENDCG
  }   
}

Fallback off
	
} // shader
