Shader "Custom/Book/Chapter12/Brightness"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Brightness("Brightness", Float) = 1
		_Stauration("Stauration", Float) = 1
		_Contrast("Contrast", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		//屏幕后处理的标配
		ZWrite Off 
		ZTest Always
		Cull Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Brightness;
			float _Stauration;
			float _Contrast;

			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 renderTex = tex2D(_MainTex, i.uv);
				fixed3 finalColor = renderTex.rgb * _Brightness;
				fixed luminance = Luminance(renderTex);
				finalColor = lerp(fixed3(luminance, luminance, luminance), finalColor, _Stauration);
				fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
				finalColor = lerp(avgColor, finalColor, _Contrast);
				return fixed4(finalColor, renderTex.a);
			}
			ENDCG
		}
	}
}
