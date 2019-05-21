Shader "Unlit/TitleLogoShader"
{
    Properties
	{
	   _MainTex("Texture (RGB)", 2D) = "white" {}
	   _Alpha("Alpha", Range(0, 1)) = 1
	}
		SubShader
	   {
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

			Cull off
			CGPROGRAM
			#pragma surface surf Lambert alpha

			sampler2D _MainTex;
			fixed _Alpha;

			struct Input
			{
				float2 uv_MainTex;
				float3 viewDir;
				float3 worldNormal;
			};

			void surf(Input IN, inout SurfaceOutput o)
			{
				half4 c = tex2D(_MainTex, IN.uv_MainTex);
                if(c.r==0.0f&&c.g==0.0f&&c.b==0.0f)
                {
                    discard;
                }
                o.Albedo=c.rgb;
                o.Alpha=c.a*_Alpha;
			}
			ENDCG
	   }
		   FallBack "Diffuse"
}
