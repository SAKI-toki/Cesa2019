//グレースケール
Shader "Custom/GrayScale"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
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

			//色を決める関数
			fixed4 frag(v2f_img i) : COLOR
			{
				half4 col = tex2D(_MainTex,i.uv);
				half gray =
				col.r * 0.2126 + 
				col.g * 0.7252 + 
				col.b * 0.0722;
				col.r = col.g = col.b = gray;
				return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
