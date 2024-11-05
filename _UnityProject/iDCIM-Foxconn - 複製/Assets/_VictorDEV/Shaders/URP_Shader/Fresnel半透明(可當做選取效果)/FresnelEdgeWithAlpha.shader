Shader "Custom/FresnelEdgeWithAlphaThreshold"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _FresnelColor ("Fresnel Color", Color) = (0.5, 0.5, 1, 1)
        _FresnelPower ("Fresnel Power", Range(1, 10)) = 3
        _Alpha ("Alpha", Range(0, 1)) = 1.0
        _FresnelThreshold ("Fresnel Threshold", Range(0, 1)) = 0.2 // 新增Fresnel閾值
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD0;
            };

            float4 _MainColor;
            float4 _FresnelColor;
            float _FresnelPower;
            float _Alpha;
            float _FresnelThreshold;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDir = normalize(_WorldSpaceCameraPos - TransformObjectToWorld(IN.positionOS).xyz);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // Normalize the normal and view direction
                float fresnel = pow(1.0 - saturate(dot(IN.worldNormal, IN.viewDir)), _FresnelPower);
                
                // 使用閾值保護Fresnel效果，防止過於透明
                fresnel = max(fresnel, _FresnelThreshold);
                
                // Interpolate between the main color and Fresnel color
                float4 fresnelEffect = lerp(_MainColor, _FresnelColor, fresnel);
                
                // Set alpha based on fresnel and _Alpha property
                fresnelEffect.a = _Alpha * fresnel;
                
                return fresnelEffect;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
