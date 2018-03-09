Shader "Custom/Sprite/newFlicker"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

        [PerRendererData] _FlickColor("Flick Color", Color) = (0.0, 0.0, 0.0, 0.0)
		[PerRendererData] _BindFactor("Bind Factor", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="False"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex dev_SpriteVert
            #pragma fragment dev_SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
        
            float4 _FlickColor;
			float _BindFactor;

            v2f dev_SpriteVert(appdata_t IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

            #ifdef UNITY_INSTANCING_ENABLED
                IN.vertex.xy *= _Flip.xy;
            #endif

                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 dev_SpriteFrag(v2f IN) : SV_Target
            {
                fixed4 rawColor = tex2D (_MainTex, IN.texcoord);

                #if ETC1_EXTERNAL_ALPHA
                    fixed4 alpha = tex2D (_AlphaTex, IN.texcoord);
                    rawColor.a = lerp (rawColor.a, alpha.r, _EnableExternalAlpha);
                #endif

                rawColor.rgb = lerp(_Color, _FlickColor, _BindFactor) * rawColor.a;
                
                return rawColor;
            }

        ENDCG
        }
    }
}
