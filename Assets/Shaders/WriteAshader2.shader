Shader "Custom/Test"
{
	Properties{
		_MainTexture("MainTexture", 2D) = "white"{}
		_SS("SS", Range(0,1)) = 0
	}
	SubShader{
		Cull Off
		Tags{"Queue" = "Transparent" "RenderType"="Opaque"}
		Pass{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				struct a2v
				{
					float4 vertex:POSITION;
					float4 texcoord:TEXCOORD0; 
				};
				struct v2f
				{
					float4 pos:SV_POSITION;
					float2 uv:TEXCOORD0; 
				};

				sampler2D _MainTexture;
				float4 _MainTexture_ST;
				half _SS;
				v2f vert(a2v v){
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTexture);
					return o;
				}
				float4 frag(v2f v):SV_TARGET
				{
					float3 color = tex2D(_MainTexture, v.uv).rgb;
					return float4(color.rgb, 1);
				}

			ENDCG
		}
	}
	Fallback "Diffuse"
}