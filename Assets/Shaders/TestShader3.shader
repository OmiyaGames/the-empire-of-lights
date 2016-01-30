Shader "Omiya Games/Test Shader 2"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5, 0.5, 0.5, 1)
		_MainTex("Light (RGB)", 2D) = "white" {}
		_ShadeTex("Shade (RGB)", 2D) = "black" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		//_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" "PerformanceChecks" = "False" }
		LOD 200

CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#pragma surface surf NoLighting

		fixed4 LightingNoLighting(SurfaceOutputStandard s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		sampler2D _ShadeTex;
		sampler2D _BumpMap;
		float4 _Color;

		struct Input
		{
			float2 uv_MainTex : TEXCOORD0;
			float2 uv_ShadeTex : TEXCOORD0;
			float2 uv_BumpMap;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//texcol.rgb = dot(texcol.rgb, float3(0.3, 0.59, 0.11));

			// Calculate the normal
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			// Calculate the albedo
			half4 highlight = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			half4 shade = tex2D(_ShadeTex, IN.uv_ShadeTex) * _Color;
			//o.Albedo = highlight.rgb;
			//o.Alpha = highlight.a;
			o.Albedo = shade.rgb;
			o.Alpha = shade.a;
		}
ENDCG
	}

		Fallback "Diffuse"
}
