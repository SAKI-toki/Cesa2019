Shader "Custom/RimShader"
{
	Properties
	{
	   _MainTex("Texture (RGB)", 2D) = "white" {}
	   _RimValue("RimValue", Range(0, 5)) = 0.5
	   _Color("Color", Color) = (1, 1, 1, 1)
	}
		SubShader
	   {
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

			CGPROGRAM
			#pragma surface surf Lambert alpha

			sampler2D _MainTex;
			fixed _RimValue;

			struct Input
			{
				float2 uv_MainTex;
				float3 viewDir;
				float3 worldNormal;
			};

			fixed4 _Color;
			uniform float AddRim = 0;

			void surf(Input IN, inout SurfaceOutput o)
			{
				//テクスチャの色を取得(時間でy座標を変化て幻想的に!)
				half4 c = tex2D(_MainTex, IN.uv_MainTex + _Time.y * 0.1);
				//ベースの色と掛け合わせる
				o.Albedo = c.rgb * _Color;

				//法線
				float3 normal = normalize(IN.worldNormal);
				//カメラの向き
				float3 dir = normalize(IN.viewDir);
				//内積の絶対値
				float val = 1 - (abs(dot(dir, normal)));
				//リム
				float rim = val * val *  (_RimValue + AddRim);
				o.Alpha = c.a * rim;
			}
			ENDCG
	   }
		   FallBack "Diffuse"
}
