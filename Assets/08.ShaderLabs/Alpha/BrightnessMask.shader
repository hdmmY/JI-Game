Shader "Custom/BrightnessMask" {
	Properties 
	{
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Main Tex", 2D) = "white" {}
		_MaskTex ("Mask Tex", 2D) = "white" {}

		_BrightScale ("Brightness Scale", Range(0, 2)) = 1
		_BackgroundFactor ("Background Factor", Range(0, 1)) = 0.5
	}

	SubShader 
	{		
		Pass 
		{ 		
			ZTest Always 
			Cull Off 
			ZWrite Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			fixed4 _Color;
			fixed _BrightScale;			
			fixed _BackgroundFactor;
			sampler2D _MainTex;
			sampler2D _MaskTex;
			float4 _MainTex_ST;
			
			struct a2v 
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			struct v2f 
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD2;
			};
			
			v2f vert(a2v v) 
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);				
				o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				
				return o;
			}
			
			fixed luminance(fixed3 color)
			{
				return 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
			}

			fixed4 frag(v2f i) : SV_Target 
			{
				fixed3 texColor = tex2D(_MainTex, i.uv).rgb;
				fixed3 maskColor = tex2D(_MaskTex, i.uv).rgb;

				fixed maskBrightness = luminance(maskColor) * _BrightScale;

				fixed3 color;
				
				//color = (texColor + maskColor) * maskBrightness;
				
				//color = min(texColor * maskBrightness, maskColor);

				//color = max(texColor * maskColor, maskColor);

				//color = texColor * maskBrightness + maskColor * (1 - maskBrightness);

				color = lerp(maskColor, texColor * maskBrightness, _BackgroundFactor);


				return fixed4(color, 1);
			}
			

			ENDCG
		}
	} 

	FallBack "Specular"
}

