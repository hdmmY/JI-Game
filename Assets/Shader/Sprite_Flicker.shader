Shader "Custom/Sprite/Flicker" {
	
	Properties {		
		[PerRendererData] 
		_MainTex("Sprite Texture", 2D) = "white" {}
		
		[MaterialToggle] 
		PixelSnap ("Pixel snap", Float) = 0

		[PerRendererData]
		_FlickColor("Flick Color", Color) = (0.0, 0.0, 0.0, 0.0)

		[PerRendererData]
		_BindFactor("Bind Factor", Range(0, 1)) = 0.5
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

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			float4 _EdgeColor;
			float _EdgeWidth;

			float4 _FlickColor;
			float _BindFactor;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0; 
			};

			struct vertexOutput
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};

			vertexOutput vert(vertexInput i)
			{
				vertexOutput o;

				o.vertex = UnityObjectToClipPos(i.vertex);
				o.texcoord = i.uv;

				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap (o.vertex);
				#endif

				return o;
			}

			fixed4 frag(vertexOutput i) : SV_Target
			{
				fixed4 texColor = tex2D(_MainTex, i.texcoord);	
				
				_FlickColor.a = texColor.a;

				return lerp(texColor, _FlickColor, _BindFactor);
			}

			ENDCG
		}			
	}
}
