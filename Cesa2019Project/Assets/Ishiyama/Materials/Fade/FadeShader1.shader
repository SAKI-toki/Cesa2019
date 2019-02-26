Shader "Custom/FadeShader1"
{
	Properties
	{
		_MainTex("MainTex", 2D) = ""{}
		_Smooth("Smooth",Range(0,1)) = 0.5
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
			float _Smooth;
			//真ん中からの距離
			uniform float FadeDistance = 100.0f;

			//色を決める関数
			fixed4 frag(v2f_img i) : COLOR
			{
				fixed4 c = tex2D(_MainTex, i.uv);
				float2 uv = i.uv;
				float aspect = _ScreenParams.x / _ScreenParams.y;
				uv.x *= aspect;
				float dist = distance(uv, float2(0.5f * aspect, 0.5f));
				//真ん中からFadeDistanceより長かったら黒にする
				if (dist > FadeDistance)
				{
					return fixed4(0, 0, 0, 1);
				}
				//スムーズに黒にする
				if (dist > FadeDistance * (1.0f - _Smooth))
				{
					//黒の割合を出す
					float temp = 1.0f - (dist - FadeDistance * (1.0f - _Smooth)) / (FadeDistance*_Smooth);
					return c * fixed4(temp, temp, temp, 1);
				}
				return c;
			}
			ENDCG
		}
	}
    FallBack "Diffuse"
}
