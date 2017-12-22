Shader "Custom/ScrollBackground" {
	
	Properties 
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_ScrollSpeed("Scroll Speed", Float) = 0.1
	}


	SubShader 
	{
		Tags 
		{ 
			"RenderType"="Opaque" 
		}
		
		Pass 
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _ScrollSpeed;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0; 
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.pos = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);

				return output;
			}

			fixed4 frag(vertexOutput input) : SV_Target
			{
				input.uv.x += _ScrollSpeed * _Time.y;
				input.uv.x = frac(input.uv.x);

				return tex2D(_MainTex, input.uv);
			}

			ENDCG
		}

	}
	FallBack "Diffuse"
}
