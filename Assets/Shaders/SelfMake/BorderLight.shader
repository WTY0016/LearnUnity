Shader "Custom/SelfMake/BorderLight"
{
	Properties
	{
		_DiffuseColor("Diffuse", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Border("Border", Float) = 1
		_BorderColor("BorderColor", Color) = (1,1,1,1)
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
				float3 worldPos : TEXCOORD1;
				float3 worldNormal:TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _DiffuseColor;
			fixed _Strengh;
			fixed4 _EmissiveColor;
			fixed _Border;
			fixed4 _BorderColor;
			v2f vert (appdata v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 tex = tex2D(_MainTex, i.uv);
				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				fixed3 diffuse = tex.rgb * _LightColor0.rgb * saturate(dot(lightDir, i.worldNormal));
				fixed3 viewDir = UnityWorldSpaceViewDir(i.worldPos);
				fixed jiajiao = dot(normalize(viewDir), i.worldNormal);
				if(jiajiao < _Border){
					return _BorderColor;
				}
				return fixed4(diffuse, 1);
			}
			ENDCG
		}
	}
}
