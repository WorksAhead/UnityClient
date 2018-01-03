// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33916,y:32513,varname:node_3138,prsc:2|emission-9725-OUT,alpha-1608-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32971,y:32459,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:8423,x:32935,y:32726,varname:node_8423,prsc:2,ntxv:0,isnm:False|UVIN-2504-OUT,TEX-4181-TEX;n:type:ShaderForge.SFN_Tex2d,id:4596,x:32299,y:32882,varname:node_4596,prsc:2,ntxv:0,isnm:False|UVIN-5377-UVOUT,TEX-3733-TEX;n:type:ShaderForge.SFN_Tex2d,id:3792,x:32299,y:33081,varname:node_3792,prsc:2,ntxv:0,isnm:False|UVIN-1827-UVOUT,TEX-3733-TEX;n:type:ShaderForge.SFN_Panner,id:5377,x:31964,y:32892,varname:node_5377,prsc:2,spu:0.5,spv:0|UVIN-5640-UVOUT,DIST-4106-OUT;n:type:ShaderForge.SFN_Panner,id:1827,x:31982,y:33114,varname:node_1827,prsc:2,spu:0,spv:0.5|UVIN-5640-UVOUT,DIST-6597-OUT;n:type:ShaderForge.SFN_TexCoord,id:5640,x:31639,y:32658,varname:node_5640,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_Tex2dAsset,id:3733,x:31932,y:32350,ptovrint:False,ptlb:wenli_tex,ptin:_wenli_tex,varname:node_3733,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4106,x:31712,y:32892,varname:node_4106,prsc:2|A-6683-T,B-2252-OUT;n:type:ShaderForge.SFN_Time,id:6683,x:31429,y:32862,varname:node_6683,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:2252,x:31418,y:33103,ptovrint:False,ptlb:u_flowspeed,ptin:_u_flowspeed,varname:node_2252,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:7446,x:31440,y:33247,ptovrint:False,ptlb:v_flowspeed,ptin:_v_flowspeed,varname:node_7446,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:6597,x:31712,y:33139,varname:node_6597,prsc:2|A-6683-T,B-7446-OUT;n:type:ShaderForge.SFN_Append,id:9656,x:32462,y:32995,varname:node_9656,prsc:2|A-4596-R,B-3792-G;n:type:ShaderForge.SFN_Multiply,id:4250,x:32714,y:33052,varname:node_4250,prsc:2|A-9656-OUT,B-6614-OUT,C-5640-Z;n:type:ShaderForge.SFN_ValueProperty,id:6614,x:32418,y:33241,ptovrint:False,ptlb:flow_strength,ptin:_flow_strength,varname:node_6614,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.25;n:type:ShaderForge.SFN_Add,id:2504,x:32686,y:32716,varname:node_2504,prsc:2|A-5640-UVOUT,B-4250-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:4181,x:32575,y:32403,ptovrint:False,ptlb:main_tex,ptin:_main_tex,varname:node_4181,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4432,x:33249,y:32620,varname:node_4432,prsc:2|A-7241-RGB,B-8423-RGB,C-3513-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3513,x:32971,y:32656,ptovrint:False,ptlb:emiss_strength,ptin:_emiss_strength,varname:node_3513,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:1608,x:33588,y:32804,varname:node_1608,prsc:2|A-8423-A,B-5373-A,C-1773-OUT,D-5239-OUT,E-7241-A;n:type:ShaderForge.SFN_ValueProperty,id:1773,x:33080,y:32954,ptovrint:False,ptlb:alpha_strength,ptin:_alpha_strength,varname:node_1773,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_VertexColor,id:5373,x:33136,y:32726,varname:node_5373,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9725,x:33657,y:32625,varname:node_9725,prsc:2|A-4432-OUT,B-5373-RGB;n:type:ShaderForge.SFN_Add,id:3802,x:33061,y:33245,varname:node_3802,prsc:2|A-2246-B,B-5640-W;n:type:ShaderForge.SFN_Tex2d,id:2246,x:32809,y:33198,varname:node_2246,prsc:2,ntxv:0,isnm:False|TEX-3733-TEX;n:type:ShaderForge.SFN_Clamp01,id:5239,x:33332,y:33239,varname:node_5239,prsc:2|IN-3802-OUT;proporder:7241-4181-3733-2252-7446-6614-3513-1773;pass:END;sub:END;*/

Shader "Aaron/ceshi_001" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _main_tex ("main_tex", 2D) = "white" {}
        _wenli_tex ("wenli_tex", 2D) = "white" {}
        _u_flowspeed ("u_flowspeed", Float ) = 1
        _v_flowspeed ("v_flowspeed", Float ) = 1
        _flow_strength ("flow_strength", Float ) = 0.25
        _emiss_strength ("emiss_strength", Float ) = 0
        _alpha_strength ("alpha_strength", Float ) = 1
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _wenli_tex; uniform float4 _wenli_tex_ST;
            uniform float _u_flowspeed;
            uniform float _v_flowspeed;
            uniform float _flow_strength;
            uniform sampler2D _main_tex; uniform float4 _main_tex_ST;
            uniform float _emiss_strength;
            uniform float _alpha_strength;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_6683 = _Time + _TimeEditor;
                float2 node_5377 = (i.uv0+(node_6683.g*_u_flowspeed)*float2(0.5,0));
                float4 node_4596 = tex2D(_wenli_tex,TRANSFORM_TEX(node_5377, _wenli_tex));
                float2 node_1827 = (i.uv0+(node_6683.g*_v_flowspeed)*float2(0,0.5));
                float4 node_3792 = tex2D(_wenli_tex,TRANSFORM_TEX(node_1827, _wenli_tex));
                float2 node_2504 = (i.uv0+(float2(node_4596.r,node_3792.g)*_flow_strength*i.uv0.b));
                float4 node_8423 = tex2D(_main_tex,TRANSFORM_TEX(node_2504, _main_tex));
                float3 emissive = ((_Color.rgb*node_8423.rgb*_emiss_strength)*i.vertexColor.rgb);
                float3 finalColor = emissive;
                float4 node_2246 = tex2D(_wenli_tex,TRANSFORM_TEX(i.uv0, _wenli_tex));
                return fixed4(finalColor,(node_8423.a*i.vertexColor.a*_alpha_strength*saturate((node_2246.b+i.uv0.a))*_Color.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
