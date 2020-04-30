Shader "Custom/Book/Chapter6/DiffusePerPixelMergeImages"
{
	Properties
	{
		_DiffuseColor("Diffuse", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_MainRate("MainRatae", Range(0,1)) = 1
		_SecondTex("SecondTexture", 2D) = "white"{}
		_SecondRate("SecondRate", Range(0,1)) = 0
		_Strengh("Strengh", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}//只有定义了正确的光照模式 才能拿到正确的光照信息
	
		Cull Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal: NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
				float3 normal:NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _SecondTex;
			float4 _SecondTex_ST;
			fixed _MainRate;
			fixed _SecondRate;
			fixed4 _DiffuseColor;
			fixed _Strengh;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _SecondTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;//环境光的颜色
				fixed3 worldNormal = UnityObjectToWorldNormal(i.normal);
				fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);//光源方向
				fixed3 diffuse = _Strengh * _LightColor0.rgb * _DiffuseColor.rgb * saturate(dot(worldNormal, worldLight));

				fixed4 col1 = tex2D(_MainTex, i.uv) * _MainRate;
				fixed4 col2 = tex2D(_SecondTex, i.uv2) * _SecondRate;
				return fixed4(diffuse + col1.xyz * col2.xyz + ambient, 1);
				// col = col + fixed3(diffuse.x,diffuse.y, diffuse.z, 1);
				// return col;
			}
			ENDCG
		}
	}
}
