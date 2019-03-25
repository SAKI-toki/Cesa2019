//色反転
Shader "Custom/ColorInvert"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Threshold("Threshold",Range(0,1)) = 0
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
			float _Threshold;

			//色を決める関数
			fixed4 frag(v2f_img i) : COLOR
			{
				half4 col = tex2D(_MainTex,i.uv);
				col.rgb = abs(_Threshold - col.rgb);
				return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
