Shader "Hidden/MiniMapShader"
{
    Properties
    {
        mainTex ("MapTexture", 2D) = "white" {}
        fogOfWar ("FogOfWar", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D mainTex;
            sampler2D fogOfWar;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(mainTex, i.uv) + tex2D(fogOfWar, i.uv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                if(col.r >= 1.0f && col.g == 0.0f && col.b == 0.0f)
                {
                    //col.r == 0.0f;
                    col.a = 0.0f;
                }
                else
                {
                    col.a = 1.0f;
                }
                return col;
            }
            ENDCG
        }
    }
}
