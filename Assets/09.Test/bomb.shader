Shader "Custom/Test/dev_bomb_circle" 
{
	Properties 
    {
        _Radius ("Radius", Float) = 1.0
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Width ("Width", Float) = 1.0
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

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
		
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f 
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Radius;
            float4 _Color;
            float _Width;

            v2f vert(appdata_base i)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.texcoord;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = length(i.uv - float2(0.5, 0.5));

                float width = 0.05f * _Width;
            
                float alpha = smoothstep(0.4 - width, 0.4, dist) - 
                    smoothstep(0.4, 0.4 + width, dist);

                return fixed4(_Color.rgb, alpha);
            }

            ENDCG
        }
	}
}
