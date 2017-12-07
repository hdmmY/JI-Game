Shader "Custom/EdgeOutline" {
	Properties 
	{
		_MainTex ("Main Tex", 2D) = "white" {}
		_EdgeColor ("EdgeColor", Color) = (0, 0, 0, 0)
		_EdgeWidth ("EdgeWidth", Float) = 1
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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;

			float _EdgeWidth;
			fixed4 _EdgeColor;


			struct a2v 
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			struct v2f
			 {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				half2  uv[9] : TEXCOORD1;

			};
			
			v2f vert(a2v v) 
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);		
				o.texcoord = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				
				o.uv[0] = v.texcoord + _MainTex_TexelSize.xy * half2(-1, -1);
				o.uv[1] = v.texcoord + _MainTex_TexelSize.xy * half2(0, -1);
				o.uv[2] = v.texcoord + _MainTex_TexelSize.xy * half2(1, -1);
				o.uv[3] = v.texcoord + _MainTex_TexelSize.xy * half2(-1, 0);
				o.uv[4] = v.texcoord + _MainTex_TexelSize.xy * half2(0, 0);
				o.uv[5] = v.texcoord + _MainTex_TexelSize.xy * half2(1, 0);
				o.uv[6] = v.texcoord + _MainTex_TexelSize.xy * half2(-1, 1);
				o.uv[7] = v.texcoord + _MainTex_TexelSize.xy * half2(0, 1);
				o.uv[8] = v.texcoord + _MainTex_TexelSize.xy * half2(1, 1);

				return o;
			}
			
			half Sobel(v2f i) {
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

			fixed4 frag(v2f i) : SV_Target 
			{
				fixed edge = Sobel(i);
				clip((0.1 - edge));

				return _EdgeColor;
			}
			
			ENDCG
		}
	} 
	FallBack "Specular"
}
