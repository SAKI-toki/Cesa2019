//色に重みをつける
Shader "Custom/ColorWeight"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Weight("Color",Color) = (0,0,0,1)
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
				half4 _Weight;

				//色を決める関数
				fixed4 frag(v2f_img i) : COLOR
				{
					half4 col = tex2D(_MainTex,i.uv);
					col.rgb = _Weight.rgb * col.rgb;
					return col;
				}
				ENDCG
			}
		}
			FallBack "Diffuse"
}
