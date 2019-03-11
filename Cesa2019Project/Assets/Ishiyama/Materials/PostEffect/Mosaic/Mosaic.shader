//モザイク
Shader "Custom/Mosaic"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_PixelNum("PixelNum",Range(0,100)) = 50
	}
	SubShader
	{
		Cull Off
		ZWrite Off
		ZTest Always
		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert_img
			#pragma fragment frag

			sampler2D _MainTex;
			float _PixelNum;

			//色を決める関数
			fixed4 frag(v2f_img i) : COLOR
			{
				half ratioX = 1 / _PixelNum;
				half ratioY = 1 / _PixelNum * _ScreenParams.x / _ScreenParams.y;
				half2 uv = half2(
					(int)(i.uv.x / ratioX) * ratioX, 
					(int)(i.uv.y / ratioY) * ratioY);

				return tex2D(_MainTex, uv);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
