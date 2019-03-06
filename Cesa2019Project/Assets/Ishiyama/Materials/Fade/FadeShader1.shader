Shader "Custom/FadeShader1"
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
				fixed4 c = tex2D(_MainTex, i.uv);
				float2 uv = i.uv;
				float aspect = _ScreenParams.x / _ScreenParams.y;
				uv.x *= aspect;
				float dist = distance(uv, float2(0.5f * aspect, 0.5f));
				//真ん中からFadeDistanceより長かったら_Colorにする
				if (dist > _Distance)
				{
					return _Color;
				}
				//スムーズに_Colorにする
				if (dist > _Distance * (1.0f - _Smooth))
				{
					float percent = (dist - _Distance * (1.0f - _Smooth)) / (_Distance * _Smooth);
					return fixed4(
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
