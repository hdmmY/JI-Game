Shader "Custom/Sprite/Dissolve_EdgeOutline" {
	
	Properties {		
		[PerRendererData] 
		_MainTex("Sprite Texture", 2D) = "white" {}
		
		[MaterialToggle] 
		PixelSnap ("Pixel snap", Float) = 0

		[PerRendererData]
		_DissolveMap("Dissolve Map", 2D) = "white" {}

		[PerRendererData]
		_DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0

		[PerRendererData]
		_EdgeColor("Edge Color", Color) = (0.0, 0.0, 0.0, 0.0)

		[PerRendererData]
		_EdgeWidth("Edge Width", Float) = 1
	}

	SubShader {
		
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}		

		Cull Off
		Lighting Off
		ZTest Always
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha


		Pass{
			
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"

			fixed _DissolveAmount;

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			sampler2D _DissolveMap;
			float4 _DissolveMap_ST;

			float4 _EdgeColor;
			float _EdgeWidth;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0; 
			};

			struct vertexOutput
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 uvDissolveMap : TEXCOORD1;
				half2  uv[9] : TEXCOORD2;
			};

			vertexOutput vert(vertexInput i)
			{
				vertexOutput o;

				o.vertex = UnityObjectToClipPos(i.vertex);
				o.texcoord = i.uv;
				o.uvDissolveMap = TRANSFORM_TEX(i.uv, _DissolveMap);

				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap (o.vertex);
				#endif

				o.uv[0] = i.uv + _MainTex_TexelSize.xy * half2(-1, -1);
				o.uv[1] = i.uv + _MainTex_TexelSize.xy * half2(0, -1);
				o.uv[2] = i.uv + _MainTex_TexelSize.xy * half2(1, -1);
				o.uv[3] = i.uv + _MainTex_TexelSize.xy * half2(-1, 0);
				o.uv[4] = i.uv + _MainTex_TexelSize.xy * half2(0, 0);
				o.uv[5] = i.uv + _MainTex_TexelSize.xy * half2(1, 0);
				o.uv[6] = i.uv + _MainTex_TexelSize.xy * half2(-1, 1);
				o.uv[7] = i.uv + _MainTex_TexelSize.xy * half2(0, 1);
				o.uv[8] = i.uv + _MainTex_TexelSize.xy * half2(1, 1);

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
				
				half edge = _EdgeWidth - abs(edgeX) - abs(edgeY);

				return saturate(edge);
			}

			fixed4 frag(vertexOutput i) : SV_Target
			{
				fixed3 dissolve = tex2D(_DissolveMap, i.uvDissolveMap).rgb;

				fixed4 texColor = tex2D(_MainTex, i.texcoord);

				clip(dissolve.g - _DissolveAmount);

				fixed edge = Sobel(i);
				return lerp(_EdgeColor, texColor, edge);
			}


			ENDCG
		}			
	}
}
