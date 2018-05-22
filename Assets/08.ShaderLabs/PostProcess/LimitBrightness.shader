Shader "Custom/LimitBrightness" {
	Properties 
	{
		_MainTex ("Main Tex", 2D) = "white" {}
		
		_Threshold ("Threshold", Range(0, 1)) = 1.0
	}

	SubShader 
	{	
		Pass 
		{ 
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			
			fixed _Threshold;

			struct a2v 
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			struct v2f
			 {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			v2f vert(a2v v) 
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);		
				o.texcoord = v.texcoord;

				return o;
			}
			
			fixed3 rgb2hsb(in fixed3 c)
			{
				fixed4 K = fixed4(0, -1.0/3.0, 2.0/3.0, -1);
				
				fixed4 p = lerp(fixed4(c.bg, K.wz),
								fixed4(c.gb, K.xy),
								step(c.b, c.g));
				fixed4 q = lerp(fixed4(p.xyw, c.r),
								fixed4(c.r, p.yzx),
								step(p.x, c.r));

				fixed d = q.x - min(q.w, q.y);
				fixed e = 1.0e-10;
				
				return fixed3(abs(q.z + (q.w - q.y) / (6 * d + e)),
								d / (q.x + e),
								q.x); 
			}

			fixed3 hsb2rgb(in fixed3 c)
			{
				fixed3 rgb = clamp(abs(fmod(c.x * 6.0 + fixed3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 
				                0.0,
				                1.0 );

			    rgb = rgb * rgb * fixed3(3.0 - 2.0 * rgb);
			 	
			    return c.z * lerp(fixed3(1, 1, 1), rgb, c.y);
			}

			fixed4 frag(v2f i) : SV_Target 
			{
				fixed3 origin = tex2D(_MainTex, i.texcoord).rgb;
				fixed3 originHSB = rgb2hsb(origin.rgb);

				fixed3 newHSB = originHSB;
				newHSB.z = originHSB.z - step(_Threshold, originHSB.z) * (originHSB.z - _Threshold);

				return fixed4(hsb2rgb(newHSB), 1);				
			}
			
			ENDCG
		}
	} 
	FallBack "Specular"
}
