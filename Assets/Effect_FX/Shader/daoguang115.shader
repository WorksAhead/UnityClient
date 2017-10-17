// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:34738,y:32643,varname:node_1,prsc:2|emission-1625-OUT,alpha-2422-OUT;n:type:ShaderForge.SFN_Tex2d,id:4,x:33691,y:32717,ptovrint:False,ptlb:diffuse,ptin:_diffuse,varname:node_3956,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9d1d3a474a8ef8645a6bc2e1cde71993,ntxv:0,isnm:False|UVIN-9803-OUT;n:type:ShaderForge.SFN_Tex2d,id:5,x:33663,y:33039,ptovrint:False,ptlb:alpha,ptin:_alpha,varname:node_7474,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-54-UVOUT;n:type:ShaderForge.SFN_Multiply,id:14,x:34129,y:32951,varname:node_14,prsc:2|A-108-OUT,B-5-R;n:type:ShaderForge.SFN_TexCoord,id:54,x:33125,y:32799,varname:node_54,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_VertexColor,id:101,x:33335,y:32536,varname:node_101,prsc:2;n:type:ShaderForge.SFN_Multiply,id:107,x:33927,y:32764,varname:node_107,prsc:2|A-4-RGB,B-101-RGB;n:type:ShaderForge.SFN_Multiply,id:108,x:33939,y:32890,varname:node_108,prsc:2|A-112-OUT,B-4-A;n:type:ShaderForge.SFN_ValueProperty,id:112,x:33663,y:32940,ptovrint:False,ptlb:QD,ptin:_QD,varname:node_3368,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Vector1,id:259,x:32834,y:33063,varname:node_259,prsc:2,v1:2;n:type:ShaderForge.SFN_OneMinus,id:6127,x:32974,y:33171,varname:node_6127,prsc:2|IN-3791-V;n:type:ShaderForge.SFN_Add,id:6900,x:33383,y:33238,varname:node_6900,prsc:2|A-2643-OUT,B-3791-V;n:type:ShaderForge.SFN_Multiply,id:2643,x:33202,y:33134,varname:node_2643,prsc:2|A-3791-U,B-6127-OUT;n:type:ShaderForge.SFN_OneMinus,id:8346,x:33021,y:33421,varname:node_8346,prsc:2|IN-3791-Z;n:type:ShaderForge.SFN_Multiply,id:8559,x:33307,y:33389,varname:node_8559,prsc:2|A-8346-OUT,B-3791-W;n:type:ShaderForge.SFN_Add,id:4562,x:33512,y:33439,varname:node_4562,prsc:2|A-8559-OUT,B-3791-Z;n:type:ShaderForge.SFN_Multiply,id:3542,x:33698,y:33426,varname:node_3542,prsc:2|A-259-OUT,B-4562-OUT;n:type:ShaderForge.SFN_Vector1,id:2658,x:32834,y:32972,varname:node_2658,prsc:2,v1:-1;n:type:ShaderForge.SFN_Add,id:1225,x:33875,y:33393,varname:node_1225,prsc:2|A-2658-OUT,B-3542-OUT;n:type:ShaderForge.SFN_Vector2,id:8760,x:33904,y:33583,varname:node_8760,prsc:2,v1:0,v2:-1;n:type:ShaderForge.SFN_Multiply,id:8607,x:34148,y:33405,varname:node_8607,prsc:2|A-1225-OUT,B-8760-OUT;n:type:ShaderForge.SFN_Add,id:146,x:34352,y:33361,varname:node_146,prsc:2|A-54-UVOUT,B-8607-OUT;n:type:ShaderForge.SFN_Multiply,id:7377,x:33596,y:33224,varname:node_7377,prsc:2|A-259-OUT,B-6900-OUT;n:type:ShaderForge.SFN_Add,id:3712,x:33782,y:33189,varname:node_3712,prsc:2|A-2658-OUT,B-7377-OUT;n:type:ShaderForge.SFN_Multiply,id:5835,x:34129,y:33191,varname:node_5835,prsc:2|A-3712-OUT,B-6279-OUT;n:type:ShaderForge.SFN_Add,id:9803,x:34568,y:33165,varname:node_9803,prsc:2|A-5835-OUT,B-146-OUT;n:type:ShaderForge.SFN_Color,id:9310,x:33894,y:32531,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_9310,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.0001081317,c3:0.0001081317,c4:1;n:type:ShaderForge.SFN_Multiply,id:1625,x:34169,y:32643,varname:node_1625,prsc:2|A-9310-RGB,B-107-OUT;n:type:ShaderForge.SFN_Multiply,id:1049,x:34338,y:32916,varname:node_1049,prsc:2|A-9310-A,B-14-OUT;n:type:ShaderForge.SFN_Multiply,id:2422,x:34536,y:32869,varname:node_2422,prsc:2|A-101-A,B-1049-OUT;n:type:ShaderForge.SFN_Vector2,id:6279,x:33901,y:33299,varname:node_6279,prsc:2,v1:-1,v2:0;n:type:ShaderForge.SFN_TexCoord,id:3791,x:32473,y:33183,varname:node_3791,prsc:2,uv:2,uaff:True;n:type:ShaderForge.SFN_Vector1,id:6104,x:32473,y:33475,varname:node_6104,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Vector1,id:1795,x:32455,y:33639,varname:node_1795,prsc:2,v1:0.5;proporder:4-5-112-9310;pass:END;sub:END;*/

Shader "Shader Forge/daoguang" {
    Properties {
        _diffuse ("diffuse", 2D) = "white" {}
        _alpha ("alpha", 2D) = "white" {}
        _QD ("QD", Float ) = 1
        _Color ("Color", Color) = (0,0.0001081317,0.0001081317,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform sampler2D _alpha; uniform float4 _alpha_ST;
            uniform float _QD;
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv2 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float node_2658 = (-1.0);
                float node_259 = 2.0;
                float2 node_9803 = (((node_2658+(node_259*((i.uv2.r*(1.0 - i.uv2.g))+i.uv2.g)))*float2(-1,0))+(i.uv0+((node_2658+(node_259*(((1.0 - i.uv2.b)*i.uv2.a)+i.uv2.b)))*float2(0,-1))));
                float4 _diffuse_var = tex2D(_diffuse,TRANSFORM_TEX(node_9803, _diffuse));
                float3 emissive = (_Color.rgb*(_diffuse_var.rgb*i.vertexColor.rgb));
                float3 finalColor = emissive;
                float4 _alpha_var = tex2D(_alpha,TRANSFORM_TEX(i.uv0, _alpha));
                return fixed4(finalColor,(i.vertexColor.a*(_Color.a*((_QD*_diffuse_var.a)*_alpha_var.r))));
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
