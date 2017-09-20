// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/IntersectionHightlight" {
	Properties
	{
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_HighLightColor("Highlight Color", Color) = (0.5, 0.5, 0.5, 0)
		_MainTex("Intersection Texture", 2D) = "white" {}
		_InvFade("Soft Factor", Range(0.01,3.0)) = 1.0
	}

		

	SubShader
	{
		Tags
		{ 
			"RenderType" = "Transparent"
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
		}
		
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		Cull Off 
		Lighting Off 
		ZWrite Off 

		Pass{

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			fixed4 _HighLightColor;

			struct vertexInput 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct vertexOutput {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 projPos : TEXCOORD1;
			};

			float4 _MainTex_ST;

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
		
				o.vertex = UnityObjectToClipPos(v.vertex); //得到片元的投影坐标
				 
				o.projPos = ComputeScreenPos(o.vertex);   // 得到片元投影坐标映射到(0, 1)上后的坐标
				COMPUTE_EYEDEPTH(o.projPos.z);            // 计算这个点相对Camera的距离
		
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex); // 更加精准的匹配 texture

				return o;
	        }

			sampler2D _CameraDepthTexture;   // Depth Texture -- 深度信息Texture
			float _InvFade;      // 一个控制参数

			fixed4 frag(vertexOutput i) : COLOR
			{
				float partZ = i.projPos.z;
				float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));

				float fade = saturate(_InvFade * (abs(sceneZ - partZ))); // 片元坐标的深度和Depth Buffer中的深度越接近，fade(消散程度)越小

				_HighLightColor.a = 1 - fade; // fade(消散程度)越小，描绘轮廓的线越清楚

				fixed4 highlightCol = 2.0 *  _HighLightColor;                       // 轮廓线颜色
				fixed4 tintCol = 2.0 *  _TintColor * tex2D(_MainTex, i.texcoord);   // 正常的颜色
				
				fixed4 col = lerp(highlightCol, tintCol, fade);  // fade(消散程度)越小，颜色越偏向轮廓线的颜色
				
				if(abs(sceneZ - partZ) <= _InvFade)
					col = highlightCol;
				else
					col = tintCol;

				return col;
			}
		
			ENDCG
		}
	}
}


