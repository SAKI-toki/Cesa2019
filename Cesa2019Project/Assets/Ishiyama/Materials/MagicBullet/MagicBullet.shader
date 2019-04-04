Shader "Custom/MagicBullet"
{
	Properties
	{
	   _RimValue("RimValue", Range(0, 20)) = 0.5
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
		LOD 200

		//Pass
		//{
		//  ZWrite ON
		//  ColorMask 0
		//}

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
		};

		fixed4 _Color;
		fixed _RimValue;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			//法線
			float3 normal = normalize(IN.worldNormal);
			//カメラの向き
			float3 dir = normalize(IN.viewDir);
			//内積の絶対値
			float val = 1 - (abs(dot(dir, normal)));
			//リム
			float rim = val * val *  _RimValue;
			o.Alpha = clamp(c.a * rim, 0, 1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}