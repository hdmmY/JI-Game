Shader "Custom/ShockWave" 
{
  Properties 
    { 
      _MainTex ("Main Tex", 2D) = "white" {} 

      _Radius ("Radius", Range(0, 1)) = 0.1
      _Width ("Width", Range(0, 1)) = 0.1
      _Aspect ("Aspect", Float) = 1   //_Aspect的作用是对圆进行修正，保证在不同的分辨率下都为正圆
   } 

   SubShader 
   { 
        ZTest Always
        Cull Off
        Zwrite Off

        Pass 
        { 
            CGPROGRAM 
            #pragma vertex vert 
            #pragma fragment frag 

            #include "UnityCG.cginc" 

            sampler2D _MainTex; 
           
            float4 _Centre;
            float _Radius;
            float _Width;
            float _Aspect;

            struct appdata 
            { 
                float4 vertex : POSITION; 
                fixed2 uv : TEXCOORD0; 
            }; 

            struct v2f 
            { 
                float4 vertex : SV_POSITION; 
                fixed2 uv : TEXCOORD0; 
            }; 

            v2f vert (appdata v) 
            { 
                v2f o; 
                
                o.vertex = UnityObjectToClipPos(v.vertex);       
                o.uv = v.uv; 

                return o; 
            } 


            fixed4 frag (v2f i) : SV_Target 
            { 
                float2 dir = _Centre.xy - i.uv;
                
                //当前点离环形中边（以内外圆半径平均值为半径的圆）的距离
                float edgeWidth = length(float2(dir.x * _Aspect, dir.y)) - _Radius;   

                //计算uv坐标偏移 
                float sinX = _Width + edgeWidth;
                float weight = 2 * _Width * sin(3.14159265358979323846264338327 / (2 * _Width) * sinX);  //正弦函数：2d*sin(2π/4d x)
                float2 offsetUV = dir * weight;             //偏移量 = 偏移方向 * 偏移权重

                //最后uv取值，判断是否在环形区域内，环形区域外直接取原来uv，区域内计算偏移值
                float2 resultUV = lerp(i.uv, i.uv + offsetUV, step(abs(edgeWidth) > _Width, 0.5));

                return tex2D(_MainTex, resultUV); 
           } 
           ENDCG 
       } 
   }  
}
