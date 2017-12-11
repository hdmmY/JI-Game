Shader "Custom Test/AlphaBind" {
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)

		_MainTex ("Main Tex", 2D) = "white" {}
		_HoleTex ("Hole Tex", 2D) = "white" {}

		_HoleWidth ("Hole Width", Range(0, 0.5)) = 0.1
		_HoleRadius ("Hole Radius", Range(0, 1)) = 0
		//_HoleDisappearRadius ("Hole DisappearRadius", Range(0, 0.5)) = 0.1
	}
	
	SubShader 
	{
		Tags
		{
			"RenderType" = "Transparent"
			"IgnoreProjector" = "True"
		}

		Pass 
		{
			ZWrite Off
			Cull Off
			ZTest Always
			Fog { Mode Off } 

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _MainTex_ST;

			sampler2D _HoleTex;

			fixed4 _Color;

			fixed _HoleWidth;
			fixed _HoleRadius;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0; 
			};

			struct vertexOutput
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0; 
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);

				return output;
			}

			fixed4 frag(vertexOutput input) : SV_Target
			{
				// Determine whether current pixel in hole
				fixed radius = distance(input.uv, float2(0.5, 0.5));
				fixed bindFactor = saturate(((abs(radius - _HoleRadius)) / _HoleWidth));

				// 
				fixed amplitude = tex2D(_MainTex, input.uv).r;
				if(radius < _HoleRadius)
				{
					fixed holeDisappearRadius = _HoleRadius * 0.5;
					amplitude *= 1 - saturate((_HoleRadius - radius) / (_HoleRadius - holeDisappearRadius));
				}

				//
				fixed3 baseColor = _Color * amplitude;
				fixed3 holeColor = tex2D(_HoleTex, input.uv).rgb;

				fixed3 finalCOlor = lerp(holeColor, baseColor, bindFactor);

				return fixed4(finalCOlor, amplitude);
			}



			ENDCG
		}
	}
	FallBack "Diffuse"
}
