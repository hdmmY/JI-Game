Shader "Custom/Test/dev_SpriteDefault"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [PerRendererData] _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
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
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnitySprites.cginc"
        
            v2f dev_SpriteVert(appdata_t IN)
            {
                v2f OUT;

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
                fixed4 color = tex2D (_MainTex, IN.texcoord);

                color.rgb = _Color * color.a;
                
                return color;
            }

        ENDCG
        }
    }
}
