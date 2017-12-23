Shader "Custom/EdgeDetection" {

	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}

		[PerRendererData]
		_CentreColor("Centre Color", Color) = (0.0, 0.0, 0.0, 0.0)
	}

	SubShader {

		Tags{
			"PreviewType"="Plane"
		}
		
		Pass {
			ZTest Always
			Cull Off
			ZWrite Off
		

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _MainTex_TexelSize;  // texel size of _MainTex

			fixed4 _CentreColor;  // the main color of the sprite

			struct vertexOutput
			{
				float4 vertex: SV_POSITION;
				//half2 uv[5]: TEXCOORD0; 
				half2 uv[9] : TEXCOORD0;
			};


			vertexOutput vert(appdata_img i)
			{
				vertexOutput o;

				o.vertex = UnityObjectToClipPos(i.vertex);

				half2 uv = i.texcoord;
				// o.uv[0] = uv + _MainTex_TexelSize.xy * half2(-0.5, -0.5);
				// o.uv[1] = uv + _MainTex_TexelSize.xy * half2(0.5, -0.5);
				// o.uv[2] = uv + _MainTex_TexelSize.xy * half2(-0.5, 0.5);
				// o.uv[3] = uv + _MainTex_TexelSize.xy * half2(0.5, 0.5);
				// o.uv[4] = uv;

				o.uv[0] = uv + _MainTex_TexelSize.xy * half2(-1, -1);
				o.uv[1] = uv + _MainTex_TexelSize.xy * half2(0, -1);
				o.uv[2] = uv + _MainTex_TexelSize.xy * half2(1, -1);
				o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 0);
				o.uv[4] = uv + _MainTex_TexelSize.xy * half2(0, 0);
				o.uv[5] = uv + _MainTex_TexelSize.xy * half2(1, 0);
				o.uv[6] = uv + _MainTex_TexelSize.xy * half2(-1, 1);
				o.uv[7] = uv + _MainTex_TexelSize.xy * half2(0, 1);
				o.uv[8] = uv + _MainTex_TexelSize.xy * half2(1, 1);

				return o;
			}


			half Sobel(vertexOutput i) {
				const half Gx[9] = {-1,  0,  1,
									-2,  0,  2,
									-1,  0,  1};
				const half Gy[9] = {-1, -2, -1,
									0,  0,  0,
									1,  2,  1};		
				
				half texColor;
				half edgeX = 0;
				half edgeY = 0;
				for (int it = 0; it < 9; it++) {
					texColor = tex2D(_MainTex, i.uv[it]).a;
					edgeX += texColor * Gx[it];
					edgeY += texColor * Gy[it];
				}
				
				half edge = 1 - abs(edgeX) - abs(edgeY);

				return saturate(edge);
			}


			half Roberts(vertexOutput i)
			{
				const half Gy[4] = {-1, 0,
									0,  1};

				const half Gx[4] = {0, -1,
								    1,  0};

				half texColor;
				half edgeX = 0;
				half edgeY = 0;
				
				for(int idx = 0; idx < 4; idx++)
				{
					texColor = tex2D(_MainTex, i.uv[idx]).a;
					edgeX += Gx[idx] * texColor;
					edgeY += Gy[idx] * texColor;
				}

				return saturate(1 - abs(edgeX) - abs(edgeY));
			}


			fixed4 frag(vertexOutput i) : SV_Target
			{
				half edge = Sobel(i);

				fixed4 texColor = tex2D(_MainTex, i.uv[4]);

				return lerp(_CentreColor, texColor, edge);
			}

			ENDCG
		}	
	}

	FallBack "Diffuse"
}
