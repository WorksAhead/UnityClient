// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33008,y:32644,varname:node_4795,prsc:2|emission-8203-OUT,clip-5023-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:31809,y:32751,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1f52f3fcb021fa047b68bb158f21aad9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:2053,x:31809,y:32597,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:6102,x:32256,y:33418,ptovrint:False,ptlb:noise,ptin:_noise,varname:node_6102,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2e955053055687342b28b1ea906d0e39,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:5023,x:32551,y:33217,varname:node_5023,prsc:2|A-15-U,B-6102-R;n:type:ShaderForge.SFN_TexCoord,id:15,x:31762,y:33375,varname:node_15,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Color,id:2993,x:31809,y:32950,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_2993,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:6070,x:32344,y:32702,varname:node_6070,prsc:2|A-6074-RGB,B-2993-A,C-2053-RGB;n:type:ShaderForge.SFN_ValueProperty,id:8963,x:32503,y:33067,ptovrint:False,ptlb:Value,ptin:_Value,varname:node_8963,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3920,x:32344,y:32881,varname:node_3920,prsc:2|A-2993-RGB,B-6074-A,C-2053-A;n:type:ShaderForge.SFN_Multiply,id:4767,x:32555,y:32770,varname:node_4767,prsc:2|A-6070-OUT,B-3920-OUT;n:type:ShaderForge.SFN_Multiply,id:8203,x:32742,y:32857,varname:node_8203,prsc:2|A-4767-OUT,B-8963-OUT;proporder:2993-8963-6074-6102;pass:END;sub:END;*/

Shader "Shader Forge/alpha-blend-rj-add" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _Value ("Value", Float ) = 1
        _MainTex ("MainTex", 2D) = "white" {}
        _noise ("noise", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float4 _Color;
            uniform float _Value;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                clip((i.uv1.r+_noise_var.r) - 0.5);
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_6070 = (_MainTex_var.rgb*_Color.a*i.vertexColor.rgb);
                float3 node_3920 = (_Color.rgb*_MainTex_var.a*i.vertexColor.a);
                float3 emissive = ((node_6070*node_3920)*_Value);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
