Shader "Hidden/NewImageEffectShader"
{
    Properties
    {
        mainTex ("Main Texture", 2D) = "white" {}
        secondaryTex ("Secondary Texture", 2D) = "white" {}
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
            sampler2D secondaryTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(mainTex, i.uv) + tex2D(secondaryTex, i.uv);
                //if red==1 && blue==1, alpha should be 0
                //if red==1 && blue==0, alpha should be 0.5
                //if red==0 && blue==0, alpha should be 1
                col.a = 2.0f - col.r*1.5f-col.b*0.5f;
                /*if(col.a >=1.0f)
                {
                    col.a = 0.7;
                }*/
                return fixed4(0,0,0,col.a);
            }
            ENDCG
        }
    }
}
