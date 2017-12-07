Shader "Custom/SingleTexture" {
	Properties 
	{
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Main Tex", 2D) = "white" {}
		_AlphaScale ("Alpha Scale", Float) = 1.0
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
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _AlphaScale;
			
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
				fixed3 color = _Color * textureColor.rgb;	

				return fixed4(color, textureColor.a * _AlphaScale);
			}
			
			ENDCG
		}
	} 
	FallBack "Specular"
}
