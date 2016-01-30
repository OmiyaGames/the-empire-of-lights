Shader "Omiya Games/Test Shader 4"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		[NoScaleOffset] _MainTex("Light (RGB)", 2D) = "white" {}
		[NoScaleOffset] _ShadeTex("Shade (RGB)", 2D) = "black" {}
	}

		SubShader
	{
		Pass
		{
			// indicate that our pass is the "base" pass in forward
			// rendering pipeline. It gets ambient and main directional
			// light data set up; light direction in _WorldSpaceLightPos0
			// and color in _LightColor0
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc" // for UnityObjectToWorldNormal
#include "UnityLightingCommon.cginc" // for _LightColor0

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 diff : COLOR0; // diffuse lighting color
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				// get vertex normal in world space
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				// dot product between normal and light direction for
				// standard diffuse (Lambert) lighting
				o.diff = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				return o;
			}

			float getGrayscale(fixed4 diff)
			{
				return (diff.r * 0.3) + (diff.g * 0.59) + (diff.b * 0.11);
			}
			sampler2D _MainTex;
			sampler2D _ShadeTex;

			fixed4 frag(v2f i) : SV_Target
			{
				// sample texture
				fixed4 highlight = tex2D(_MainTex, i.uv);
				fixed4 shade = tex2D(_ShadeTex, i.uv);

				float grayscale = getGrayscale(i.diff);
				return (highlight * grayscale) + (shade * (1 - grayscale));
			}
		ENDCG
		}
	}
Fallback "Diffuse"
}
