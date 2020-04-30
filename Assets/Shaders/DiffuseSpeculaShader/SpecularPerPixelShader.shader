Shader "Custom/Book/Chapter6/SpecularPerPixel"
{
	Properties
	{
		_DiffuseColor("Diffuse", Color) = (1,1,1,1)
		_SpecularColor("Specular", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Strengh("Strengh", Float) = 1
		_Gloss("Gloss",Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}//只有定义了正确的光照模式 才能拿到正确的光照信息
	
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
				float4 vertex : SV_POSITION;
				float4 worldPos:TEXCOORD1;
				float3 worldNormal:TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _DiffuseColor;
			fixed _Strengh;
			fixed _Gloss;
			fixed4 _SpecularColor;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;//环境光的颜色
				fixed3 worldNormal = i.worldNormal;
				// fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);//光源方向
				fixed3 worldLight = UnityWorldSpaceLightDir(i.worldPos);
				fixed3 diffuse = _Strengh * _LightColor0.rgb * _DiffuseColor.rgb * saturate(dot(worldNormal, worldLight));
				fixed3 reflectDir = normalize(reflect(-worldLight, worldNormal));
				fixed3 viewDir = UnityWorldSpaceViewDir(i.worldPos);
				fixed3 specular = _LightColor0.rgb * _SpecularColor.rgb * pow(saturate(dot(viewDir, reflectDir)), _Gloss);
				fixed4 col = tex2D(_MainTex, i.uv);
				return fixed4(diffuse + ambient + specular.rgb, 1) * col;
				// col = col + fixed3(diffuse.x,diffuse.y, diffuse.z, 1);
				// return col;
			}
			ENDCG
		}
	}
}
