Shader "Custom/Sprite/SpriteFadeBetwTwoTexture" {
	
	Properties
    {		
		_UpperTex ("Upper Texture", 2D) = "white" {}
		
		_UnderTex ("Under Texture", 2D) = "white" {}

        // When fade is 0, display under texture; 
        // When fade is 1, display upper texture.
        _Fade ("Fade", Range(0, 1)) = 1
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
        Blend SrcAlpha OneMinusSrcAlpha

		Pass{
			
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

            sampler2D _UpperTex;
            float4 _UpperTex_ST;

            sampler2D _UnderTex;
            float4 _UnderTex_ST;

            float _Fade;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 vertex : SV_POSITION;
				float2 upperUV : TEXCOORD0;
                float2 underUV : TEXCOORD1;
			};

			vertexOutput vert(vertexInput i)
			{
				vertexOutput o;

				o.vertex = UnityObjectToClipPos(i.vertex);
				o.upperUV = UnityStereoScreenSpaceUVAdjustInternal(i.uv, _UpperTex_ST);
				o.underUV = UnityStereoScreenSpaceUVAdjustInternal(i.uv, _UnderTex_ST);                

				return o;
			}

			half4 frag(vertexOutput i) : SV_Target
			{
				float4 upperColor = tex2D(_UpperTex, i.upperUV);
                float4 underColor = tex2D(_UnderTex, i.underUV);

				return lerp(underColor, upperColor, _Fade);
			}

			ENDCG
		}			
	}
}
