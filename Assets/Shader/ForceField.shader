Shader "Custom/ForceField" {
	Properties 
	{
		_Tint ("Tint Color", Color) = (0, 0, 0, 0)
		_MainTex ("Main Tex", 2D) = "white" {}

		_Fresnel ("Fresnel Intensity", Range(0, 200)) = 3.0
		_FresnelWidth ("Fresnel Width", Range(0, 2)) = 3.0

		_Distort ("Distort", Range(0, 100)) = 1.0

		_ScrollSpeedU ("U Scroll Speed", Float) = 2
		_ScrollSpeedV ("V Scroll Speed", Float) = 2
	}


	SubShader 
	{
		Tags
		{
			"Queue" = "Overlay"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}	

		GrabPass
		{
			"_GrabTexture"
		}

		Pass 
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct a2v
			{
				fixed4 vertex : POSITION;
				fixed4 normal : NORMAL;
				fixed3 uv : TEXCOORD0; 
			};

			struct v2f
			{
				fixed2 uv : TEXCOORD0;
				fixed4 vertex : SV_POSITION;
				fixed3 rimColor : TEXCOORD1;
				fixed4 screenPos : TEXCOORD2; 
			};

			fixed4 _Tint;

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _CameraDepthTexture;
			
			sampler2D _GrabTexture;
			fixed4 _GrabTexture_TexelSize;

			fixed _Fresnel;
			fixed _FresnelWidth;

			fixed _Distort;

			fixed _ScrollSpeedU;
			fixed _ScrollSpeedV;


			v2f vert(a2v v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;

				// Scroll uv
				o.uv.x += _Time * _ScrollSpeedU;
				o.uv.y += _Time * _ScrollSpeedV;

				// Fresnel
				fixed3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				fixed dotProduct = 1 - saturate(dot(v.normal, viewDir));
				o.rimColor = smoothstep(1 - _FresnelWidth, 1, dotProduct) * 0.5;
				o.screenPos = ComputeScreenPos(v.vertex);

				return o;
			}


			fixed4 frag(v2f i) : SV_Target
			{
				fixed3 texColor = tex2D(_MainTex, i.uv);
				texColor = texColor * _Tint.rgb;

				// Distrotion
				i.screenPos.xy += (texColor.rg * 2 - 1) * _Distort * _GrabTexture_TexelSize;  // Soft Additive
				fixed3 distortColor = tex2Dproj(_GrabTexture, i.screenPos).rgb * _Tint.rgb;

				return fixed4(lerp(distortColor, texColor, texColor.r), 0.9);
				
			}


			ENDCG
		}
	}
}
