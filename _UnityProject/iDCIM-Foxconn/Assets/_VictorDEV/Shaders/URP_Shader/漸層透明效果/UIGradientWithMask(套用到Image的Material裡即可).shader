Shader "VictorDev/UIGradientWithMask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _TopColor ("Top Color", Color) = (1,0,0,1)
        _BottomColor ("Bottom Color", Color) = (0,0,1,1)
        _GradientDirection ("Gradient Direction", Range(0, 1)) = 0 // 0: Vertical, 1: Horizontal
        _BottomRatio ("Bottom Ratio", Range(0, 1)) = 0.5
        _RotationSpeed ("Rotation Speed", Float) = 1.0 // 旋轉速率
        [HideInInspector]_MainTex ("Base (RGB)", 2D) = "white" {} // 這裡依然保留 MainTex
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100

        Stencil
        {
            Ref 1
            Comp Equal
            Pass Keep
        }

        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR0;
            };

            fixed4 _Color;
            fixed4 _TopColor;
            fixed4 _BottomColor;
            float _GradientDirection;
            float _BottomRatio;
            float _RotationSpeed; // 旋轉速率
            sampler2D _MainTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = _Color;

                // 計算旋轉
                float angle = _Time.y * _RotationSpeed; // _Time.y 是時間變數
                float cosA = cos(angle);
                float sinA = sin(angle);

                // 簡單地旋轉 UV，這裡是針對水平和垂直方向旋轉進行計算
                float2 center = float2(0.5, 0.5); // 旋轉中心，默認為UV座標的中心
                o.uv -= center; // 偏移至中心
                o.uv = float2(
                    cosA * o.uv.x - sinA * o.uv.y, // 旋轉公式
                    sinA * o.uv.x + cosA * o.uv.y
                );
                o.uv += center; // 還原偏移

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 假設不使用_MainTex
                float gradient = lerp(i.uv.y, i.uv.x, _GradientDirection);
                gradient = (gradient - (1.0 - _BottomRatio)) / _BottomRatio;
                gradient = saturate(gradient);

                fixed4 color = lerp(_BottomColor, _TopColor, gradient);
                return color * i.color;
            }
            ENDHLSL
        }
    }
}
