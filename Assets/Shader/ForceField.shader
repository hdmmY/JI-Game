Shader "Custom/ForceField"
{
	Properties
	{
		_MainTex ("Main Tex", 2D) = "white" {}
		_DistortTex ("Distort Tex", 2D) = "white" {}

		_MainColor("MainColor", Color) = (1,1,1,1)
				
		_Distort("Distort", Range(0, 100)) = 1.0
		_DistortRadio ("Distort Radio", Range(0, 1)) = 0.5

		_ScrollSpeedU("Scroll U Speed",float) = 2
		_ScrollSpeedV("Scroll V Speed",float) = 0
	}
	SubShader
	{ 
		Tags
		{ 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
		}

		GrabPass{ "_GrabTexture" }

		Pass
		{
			Lighting Off 
			ZWrite On
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				fixed4 vertex : POSITION;
				fixed4 normal: NORMAL;
				fixed3 uv : TEXCOORD0;
			};

			struct v2f
			{
				fixed4 vertex : SV_POSITION;
				fixed2 uv : TEXCOORD0;
				fixed3 worldNormal: TEXCOORD1;
				fixed3 worldViewDir: TEXCOORD2;
				fixed2 srceenPos : TEXCOORD3;
			};

			sampler2D _MainTex; 
			fixed4 _MainTex_ST;

			sampler2D _GrabTexture;
			fixed4 _GrabTexture_ST;
			fixed4 _GrabTexture_TexelSize;
			
			fixed4 _MainColor;

			sampler2D _DistortTex;
			fixed _Distort;
			fixed _DistortRadio;

			fixed _ScrollSpeedU;
			fixed _ScrollSpeedV;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//scroll uv
				o.uv.x += _Time * _ScrollSpeedU;
				o.uv.y += _Time * _ScrollSpeedV;
				o.srceenPos = ComputeScreenPos(o.vertex);

				//fresnel 
				fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject); 
				o.worldViewDir = UnityWorldSpaceViewDir(worldPos);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				i.worldNormal = normalize(i.worldNormal);
				i.worldViewDir = normalize(i.worldViewDir);

				fixed4 main = tex2D(_MainTex, i.uv);
				main.rgb *= _MainColor.rgb * main.a;

				//distortion
				fixed2 dsitortionUV = tex2D(_DistortTex, i.uv).rg;
				i.srceenPos += (dsitortionUV - 0.5) * _Distort * _GrabTexture_TexelSize.xy;
				fixed3 distortColor = tex2D(_GrabTexture, i.srceenPos);

				// fresnel 
				fixed3 fresnelColor = pow((1 - dot(i.worldViewDir, i.worldNormal)), 5);
				fresnelColor *= _MainColor;

				//lerp distort color & fresnel color
				main.rgb = lerp(distortColor, main, _DistortRadio);
				main.rgb += fresnelColor;

				return fixed4(main.rgb, _MainColor.a);
			}

			ENDCG
		}
	}
}
