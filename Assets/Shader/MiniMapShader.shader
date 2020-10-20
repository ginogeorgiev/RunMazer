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
                //the FogOfWar mask shows us where we already were. red marks the path 
                fixed4 fogCol = tex2D(fogOfWar,i.uv);
                fixed4 mainCol = tex2D(mainTex, i.uv);
                
                //trail
                if(fogCol.r != 1.0f)
                {
                    
                    mainCol.rgb = float3(0.0f,0.0f,0.0f);
                    mainCol.a = 1.0f;
                }
               
                return mainCol;
            }
            ENDCG
        }
    }
}
