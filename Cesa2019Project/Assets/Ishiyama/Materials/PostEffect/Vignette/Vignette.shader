//ビネット
Shader "Custom/Vignette"
{
	Properties
	{
		_MainTex("MainTex", 2D) = ""{}
		_Color("Color",Color) = (0,0,0,1)
		_Smooth("Smooth",Range(0,1)) = 0.5
		_Distance("Distance",Range(0,2)) = 1
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
			//どれだけスムーズにするか
			float _Smooth;
			fixed4 _Color;
			//真ん中からの距離
			float _Distance;

			//色を決める関数
			fixed4 frag(v2f_img i) : COLOR
			{
				half4 c = tex2D(_MainTex, i.uv);
				half2 uv = i.uv;
				half aspect = _ScreenParams.x / _ScreenParams.y;
				uv.x *= aspect;
				half dist = distance(uv, half2(0.5f * aspect, 0.5f));
				//真ん中からFadeDistanceより長かったら_Colorにする
				if (dist > _Distance)
				{
					return _Color;
				}
				//スムーズに_Colorにする
				if (dist > _Distance * (1.0f - _Smooth))
				{
					half percent = (dist - _Distance * (1.0f - _Smooth)) / (_Distance * _Smooth);
					return half4(
						c.r + (_Color.r - c.r) * percent,
						c.g + (_Color.g - c.g) * percent,
						c.b + (_Color.b - c.b) * percent,
						1);
				}
				return c;
			}
			ENDCG
		}
	}
    FallBack "Diffuse"
}
