Shader "Custom/Sprite/Dissolve" {
	
	Properties {
		_DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
		//_LineWidth("Dissolve Line Width", Range(0, 1)) = 0.1

		
		[PerRendererData] 
		_MainTex("Sprite Texture", 2D) = "white" {}
		
		[MaterialToggle] 
		PixelSnap ("Pixel snap", Float) = 0

		//_DissolveFirstColor("Dissolve First Color", Color) = (0, 0, 0, 0)
		//_DissolveSecondColor("Dissolve Second Color", Color) = (0, 0, 0, 0)
		_DissolveMap("Dissolve Map", 2D) = "white" {}
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
			//fixed _LineWidth;
			sampler2D _MainTex;

			sampler2D _DissolveMap;
			float4 _DissolveMap_ST;

			//fixed4 _DissolveFirstColor;
			//fixed4 _DissolveSecondColor;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0; 
			};

			struct vertexOutput
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 uvDissolveMap : TEXCOORD1;
			};

			vertexOutput vert(vertexInput i)
			{
				vertexOutput o;

				o.vertex = UnityObjectToClipPos(i.vertex);
				o.color = i.color;
				o.texcoord = i.uv;
				o.uvDissolveMap = TRANSFORM_TEX(i.uv, _DissolveMap);

				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap (o.vertex);
				#endif

				return o;
			}


			fixed4 frag(vertexOutput i) : SV_Target
			{
				fixed3 dissolve = tex2D(_DissolveMap, i.uvDissolveMap).rgb;

				clip(dissolve.r - _DissolveAmount);

				return tex2D(_MainTex, i.texcoord);
			}


			ENDCG
		}			
	}
}
