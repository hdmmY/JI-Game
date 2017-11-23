Shader "Custom/ScrollBackground3" {
	
	Properties 
	{
		_TextureUp("The Upper Texture", 2D) = "white" {}
		_TextureMiddle("The Middle Texture", 2D) = "white" {}
		_TextureDown("The Down Texture", 2D) = "white" {}
		_ScrollSpeed("Scroll Speed", Float) = 0.1
		_Mod3AddV("Mod3AddV", Float) = 0
	}


	SubShader 
	{
		Tags 
		{ 
			"RenderType"="Opaque" 
			"Queue" = "Geometry"
		}
		
		Pass 
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _TextureUp;
			sampler2D _TextureMiddle;
			sampler2D _TextureDown;
			float _ScrollSpeed;
			float _Mod3AddV;

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
				output.uv = input.texcoord;

				return output;
			}

			fixed4 frag(vertexOutput input) : SV_Target
			{
				float addV = _ScrollSpeed * _Time.y;
				
				float mod3AddV = addV - floor(addV / 3) * 3;
				float originV = input.uv.y;
				_Mod3AddV = mod3AddV;

				if(mod3AddV < originV)
				{
					return tex2D(_TextureUp, float2(input.uv.x, originV - mod3AddV));
				}
				else if(mod3AddV < originV + 1)
				{
					return tex2D(_TextureMiddle, float2(input.uv.x, originV + 1 - mod3AddV));
				}
				else if(mod3AddV < originV + 2)
				{
					return tex2D(_TextureDown, float2(input.uv.x, originV + 2 - mod3AddV));
				}
				else
				{
					return tex2D(_TextureUp, float2(input.uv.x, originV + 3 - mod3AddV));
				}



				//return fixed4(currentUV.y, currentUV.y, currentUV.y, 1);
				return fixed4(1, 1, 1, 1);
			}

			ENDCG
		}

	}
	FallBack "Diffuse"
}
