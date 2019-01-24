Shader "Unlit/CosmoShader"
 {
    Properties
    {
        _Color1 ("Color 1", Color) = (0.0,0.0,0.0,1.0)
        _Color2 ("Color 2", Color) = (1.0,1.0,1.0,1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            fixed4 _Color1; 
            fixed4 _Color2;
            int _UseTint;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                       
                fixed4 col;

                //interpolate between _Color1 and _Color2 for each color component (r,g,b,a)
                fixed R_out = _Color1.r * (1.0 - i.color.r) + _Color2.r * (i.color.r);
                fixed G_out = _Color1.g * (1.0 - i.color.g) + _Color2.g * (i.color.g);
                fixed B_out = _Color1.b * (1.0 - i.color.b) + _Color2.b * (i.color.b);
                fixed A_out = _Color1.a * (1.0 - i.color.a) + _Color2.a * (i.color.a);
                
                col.r = R_out;
                col.g = G_out;
                col.b = B_out;
                col.a = A_out;
                
                return col;
            }
            ENDCG
        }
    }
}
