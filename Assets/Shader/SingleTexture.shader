Shader "Custom/SingleTexture" {
	Properties 
	{
		_MainTex("Main Tex", 2D) = "white" {}

		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_AlphaScale ("Alpha Scale", Range(0, 1)) = 1
	}

	SubShader 
	{		
		Tags 
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent"
			"PreviewType"="Plane"
		}

		Pass 
		{ 
			ZTest Always
			Cull Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color;
			
			fixed _AlphaScale;

			sampler2D _MainTex;
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
			
			fixed4 frag(v2f i) : SV_Target 
			{
				fixed4 textureColor = tex2D(_MainTex, i.uv);
				
				textureColor.rgb *= _Color;
				textureColor.a *= _AlphaScale;

				return textureColor;
			}
			
			ENDCG
		}
	} 
	FallBack "Specular"
}
