Shader "Custom/GrayscaleWithGaussianBlurIntensityAndBrightness"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Alpha ("Alpha", Range(0, 1)) = 1.0
        _BlurSize ("Blur Size", Range(0, 10)) = 1.0
        _Intensity ("Intensity", Range(0, 1)) = 1.0  // 控制颜色深浅
        _Brightness ("Brightness", Range(0, 2)) = 1.0 // 控制明暗程度
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Alpha;
            float _BlurSize;
            float _Intensity;
            float _Brightness;

            float kernel[9];
            float2 offset[9];

            void InitializeKernel()
            {
                kernel[0] = 0.0625; offset[0] = float2(-1, -1);
                kernel[1] = 0.125;  offset[1] = float2( 0, -1);
                kernel[2] = 0.0625; offset[2] = float2( 1, -1);
                kernel[3] = 0.125;  offset[3] = float2(-1,  0);
                kernel[4] = 0.25;   offset[4] = float2( 0,  0);
                kernel[5] = 0.125;  offset[5] = float2( 1,  0);
                kernel[6] = 0.0625; offset[6] = float2(-1,  1);
                kernel[7] = 0.125;  offset[7] = float2( 0,  1);
                kernel[8] = 0.0625; offset[8] = float2( 1,  1);
            }

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                InitializeKernel();

                float2 texelSize = _BlurSize * float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y);
                fixed3 col = fixed3(0, 0, 0);

                for(int j = 0; j < 9; j++)
                {
                    float2 samplePos = i.texcoord + offset[j] * texelSize;
                    fixed4 sampleCol = tex2D(_MainTex, samplePos);
                    col += sampleCol.rgb * kernel[j];
                }

                float gray = dot(col, float3(0.299, 0.587, 0.114));
                float3 originalColor = tex2D(_MainTex, i.texcoord).rgb;
                col = lerp(fixed3(gray, gray, gray), originalColor, _Intensity);

                // 调整亮度
                col *= _Brightness;

                float alpha = tex2D(_MainTex, i.texcoord).a * _Alpha;

                return fixed4(col, alpha);
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
