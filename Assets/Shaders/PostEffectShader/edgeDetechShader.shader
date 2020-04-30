Shader "Custom/Book/Chapter12/EdgeDetechShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_EdgeOnly ("EdgeOnly", Float) = 1
		_EdgeColor("EdgeColor", Color) = (0,0,0,1)
		_BackgroundColor("BackgroundColor", Color) = (1,1,1,1)
		_Thickness("Thickness", Float) = 1
		_MaskTex("MaskTex", 2D) = "white"{}
		_Speed("Speed", Float) = 1
		_Judge("Judge", Float) = 0.1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		//屏幕后处理的标配
		ZWrite Off 
		ZTest Always
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma  vertex vert
			#pragma  fragment frag
			#include "UnityCG.cginc"

			struct v2f{
				fixed4 pos:SV_POSITION;
				fixed2 uv:TEXCOORD0;
			};
			sampler2D _MainTex;
			float4 _MainTex_ST;
			v2f vert(appdata_img v){
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			fixed4 frag(v2f i):SV_TARGET
			{
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				half2 maskUV:TEXCOORD0;
				half2 uv[9]:TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			half4 _MainTex_TexelSize;
			fixed4 _EdgeColor;
			fixed _EdgeOnly;
			fixed4 _BackgroundColor;
			fixed _Thickness;
			sampler2D _MaskTex;
			half4 _MaskTex_ST;
			fixed _Speed;
			fixed _Judge;
			half Sobel(v2f i)
			{
				const half Gx[9] = {
					-1, -2, -1,
					0, 0, 0,
					1, 2, 1,
				};
				const half Gy[9] = {
					-1, 0, 1,
					-2, 0, 2,
					-1, 0, 1,
				};
				half texColor;
				half edgX = 0;
				half edgY = 0;
				for(int it = 0; it < 9; it++){
					texColor = Luminance(tex2D(_MainTex, i.uv[it]).rgb);
					edgX += texColor * Gx[it];
					edgY += texColor * Gy[it];
				}
				half edge = 1 - abs(edgX) - abs(edgY);
				return edge;
			}



			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				half2 uv = v.texcoord;
				o.uv[0] = uv + _Thickness * _MainTex_TexelSize.xy * half2(-1,-1);
				o.uv[1] = uv + _Thickness * _MainTex_TexelSize.xy * half2(0,-1);
				o.uv[2] = uv + _Thickness * _MainTex_TexelSize.xy * half2(1,-1);
				o.uv[3] = uv + _Thickness * _MainTex_TexelSize.xy * half2(-1,0);
				o.uv[4] = uv + _Thickness * _MainTex_TexelSize.xy * half2(0,0);
				o.uv[5] = uv + _Thickness * _MainTex_TexelSize.xy * half2(1,0);
				o.uv[6] = uv + _Thickness * _MainTex_TexelSize.xy * half2(-1,1);
				o.uv[7] = uv + _Thickness * _MainTex_TexelSize.xy * half2(0,1);
				o.uv[8] = uv + _Thickness * _MainTex_TexelSize.xy * half2(1,1);
				o.maskUV = uv + fixed2(0,frac(_Speed * _Time.y));
				o.maskUV = TRANSFORM_TEX(o.maskUV, _MaskTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half edge = Sobel(i);
				// fixed4 texCol = tex2D(_MaskTex, i.uv[4])
				
				// fixed4 withEdgeColor = lerp(_EdgeColor, texCol, edge);
				// fixed4 onlyEdgeColor = lerp(_EdgeColor, _BackgroundColor, edge);
				// fixed4 showColor = lerp(withEdgeColor, onlyEdgeColor, _EdgeOnly);
				fixed4 showColor = (1-edge) * _EdgeColor;

				fixed4 maskC = tex2D(_MaskTex, i.maskUV);
				showColor.a = showColor.a * (1-maskC.r);
				return showColor;
			}


			ENDCG
		}


	}
	FallBack "Diffuse"
}
