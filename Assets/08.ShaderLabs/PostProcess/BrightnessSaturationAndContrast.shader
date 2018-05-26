Shader "Custom/BrightnessSaturationAndContrast" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Brightness("Brightness", Float) = 2
		_Saturation("Saturation", Float) = 1
		_Contrast("Contrast", Float) = 1
	}
	SubShader {
		
		ZTest Always
		Cull Off
		Zwrite Off

		Pass {

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float _Brightness;
			float _Saturation;
			float _Contrast;

			struct vertexInput{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0; 
			};

			struct vertexOutput{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0; 
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = input.texcoord;

				return output;
			}


			fixed4 frag(vertexOutput input) : SV_Target
			{
				fixed4 renderTex = tex2D(_MainTex, input.uv);

				// apply brightness
				fixed3 finalColor = renderTex.rgb * _Brightness;

				// apply saturation
				fixed luminance = 0.2125 * renderTex.r + 0.7154 * renderTex.g + 0.0721 * renderTex.b;
				fixed3 luminanceColor = fixed3(luminance, luminance, luminance);
				finalColor = lerp(luminanceColor, finalColor, _Saturation);

				// apply contrast
				fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
				finalColor = lerp(avgColor, finalColor, _Contrast);

				return fixed4(finalColor.rgb, renderTex.a);
			}

			ENDCG
		}

	}
	FallBack Off
}
