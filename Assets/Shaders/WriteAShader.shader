// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Book/Chapter5/SimpleShader"
{
    Properties
    {
        _MainTexture("MainTexture", 2D) = "white"{}
        _Speed("sss", Float) = 1
        
    }
    
    SubShader{
        Cull Off
        Tags{"Queue" = "Transparent" "RenderType" = "Opaque"}
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct a2v//application to vertex 从应用到顶点着色器
            {
                float4 vertex : POSITION;//模型顶点坐标
                float2 textcoord : TEXCOORD0;//模型第一套纹理坐标
                float3 normal : NORMAL;//法线坐标
            };
            
            struct v2f//vertex to fragment 从顶点着色器到片段着色器
            {
                float4 pos : SV_POSITION;//包含了在裁剪空间下的坐标
                fixed3 color : COLOR;//储存了颜色信息
            };
            
            sampler2D _MainTexture;
            half _Speed;
            v2f vert(a2v i)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(i.vertex);
                o.pos.y *= _SinTime.w;
                o.color = i.normal * 0.5 + fixed3(0.5,0.5,0.5);
                return o;
            }
            
            fixed4 frag(v2f i):SV_TARGET
            {
                return fixed4(i.color, 1);
            }
            ENDCG
        }   
    }
    
    FallBack "Diffuse"
}