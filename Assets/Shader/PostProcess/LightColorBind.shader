Shader "Custom/LightColorBind" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightTex ("Light Tex", 2D) = "white" {}

		_Brightness("Brightness", Float) = 2	
	}
	SubShader {
		
		ZTest Always
		Cull Off
		Zwrite Off

		Pass {

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _LightTex;

			fixed _Brightness;

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


			fixed3 rgb2hsb(in fixed3 c)
			{
				fixed4 K = fixed4(0, -1.0/3.0, 2.0/3.0, -1);
				
				fixed4 p = lerp(fixed4(c.bg, K.wz),
								fixed4(c.gb, K.xy),
								step(c.b, c.g));
				fixed4 q = lerp(fixed4(p.xyw, c.r),
								fixed4(c.r, p.yzx),
								step(p.x, c.r));

				fixed d = q.x - min(q.w, q.y);
				fixed e = 1.0e-10;
				
				return fixed3(abs(q.z + (q.w - q.y) / (6 * d + e)),
								d / (q.x + e),
								q.x); 
			}


			fixed4 frag(vertexOutput input) : SV_Target
			{
				fixed4 screenColor = tex2D(_MainTex, input.uv);
				fixed4 lightColor = tex2D(_LightTex, input.uv);
		
				fixed luminane = Luminance(lightColor.rgb);
				fixed3 lightHSB = rgb2hsb(lightColor.rgb);

				fixed factor = lightHSB.z;

				// Apply brightness
				fixed3 finalColor = screenColor.rgb * (factor * _Brightness + 1);

				// Bind SrcAlpha OnMinusSrcAlpha
				finalColor = lightColor.rgb * factor + finalColor * (1 - factor);

				return fixed4(finalColor, 1);
			}


			ENDCG
		}

	}
	FallBack Off
}
