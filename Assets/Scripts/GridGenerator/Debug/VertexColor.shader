// Author: Xin Zhang <cowcoa@gmail.com>

Shader "Cow/VertexColor" {
  Properties {
     _MainTex ("Base (RGBA)", 2D) = "white" {}
	 _AlphaFactor ("Alpha Factor", Range(0.0, 1.0)) = 1.0
  }

  SubShader {
	Tags { 
            "Queue"="Transparent+3000" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
        }

	Cull Back
	Lighting Off
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha

    Pass {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #include "UnityCG.cginc"
     
	  sampler2D _MainTex;
	  float _AlphaFactor;

      struct v2f {
          float4 pos : SV_POSITION;
          fixed4 color : COLOR;
		  float4 uv : TEXCOORD0;
      };
      
      v2f vert (appdata_full v)
      {
          v2f o;
          o.pos = UnityObjectToClipPos (v.vertex);
          o.color = v.color;
		  o.uv = v.texcoord;
          return o;
      }

      fixed4 frag (v2f i) : SV_Target
	  {
	      fixed4 texcolor = tex2D(_MainTex, i.uv);
	      fixed4 color;
		  color.rgb = texcolor.rgb * texcolor.a + i.color.rgb * (1.0f - texcolor.a);
		  color.a = _AlphaFactor;
		  return color;
      }
      ENDCG
    }
  } 
}
