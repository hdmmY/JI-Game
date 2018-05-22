Shader "Custom/SimpleAlphaBlend" {
	Properties 
	{
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_GrayscaleTex ("Grayscale Tex", 2D) = "white" {}
	}

	SubShader 
	{
		Tags 
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent"
		}
		
		Pass 
		{
			ZWrite Off
			Blend SrcAlpha One
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
						
			sampler2D _GrayscaleTex;
			float4 _GrayscaleTex_ST;

			fixed4 _Color;
			
			fixed _AlphaScale;
			
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
				o.uv = TRANSFORM_TEX(v.texcoord, _GrayscaleTex);
				
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target 
			{				
				fixed value = tex2D(_GrayscaleTex, i.uv).a;
								
				return fixed4(value * _Color.rgb, value);
			}
			
			ENDCG
		}
	} 
	FallBack "Transparent/VertexLit"
}
