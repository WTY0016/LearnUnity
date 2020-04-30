Shader "Custom/Book/Chapter7/SimpleTexture"
{
	Properties
	{
		_MainTex("MainTexture", 2D) = "white"{}
		_ColorTint("Color", Color) = (1,1,1,1)
		_Specular("Specular", Color) = (1,1,1,1)
		_Gloss("Gloss", Range(8,256)) = 20
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
				float3 normal: NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldPos:TEXCOORD1;
				float3 worldNormal:TEXCOORD2;
			};

			fixed4 _ColorTint;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Gloss;
			fixed4 _Specular;
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
				fixed3 worldNormal = i.worldNormal;
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				fixed3 albedo = tex2D(_MainTex, i.uv).rgb * _ColorTint.rgb;
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				fixed3 diffuse = _LightColor0.xyz * albedo * saturate(dot(worldNormal, worldLightDir));
				fixed3 viewDir = UnityWorldSpaceViewDir(i.worldPos);
				fixed3 reflectDir = normalize(reflect(-worldLightDir, worldNormal));
				fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(reflectDir, worldNormal)), _Gloss);
				return fixed4(ambient + diffuse + specular, 1);
				
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
