Shader "Custom/Circular Circles" {
    Properties {
        _CircleCount ("Circle Count", Float ) = 8
        _CircleRadius ("Circle Radius", Range(0, 1)) = 0.18
        _CircleSharpness ("Circle Sharpness", Range(0, 0.999)) = 0.95
        _GroupRadius("Group Radius", Range(0, 1)) = 0.6
        [HDR] _CircleColor("Circle Color", Color) = (1,1,1,1) // HDR color property
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            // Two pi
            static float TAU = 6.28318530718;

            // Properties
            uniform float _CircleCount;
            uniform float _CircleRadius;
            uniform float _CircleSharpness;
            uniform float _GroupRadius;
            uniform float4 _CircleColor; // HDR color uniform

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 coords : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.coords = v.texcoord0 * 2 - 1; // Fragment Coordinates
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag( VertexOutput i ) : COLOR {

                // Find closest circle center for each fragment
                float angRad = atan2( i.coords.g, i.coords.r );
                float snapVal = _CircleCount / TAU;
                float angRadSnapped = round( angRad * snapVal) / snapVal;
                float2 dirSnapped = float2( cos(angRadSnapped), sin(angRadSnapped) );
                float2 nearestCircleCenter = dirSnapped * _GroupRadius;

                // Distance to nearest circle center
                float dist = distance( i.coords, nearestCircleCenter );

                // Remap it into looking solid instead of having a circular soft gradient
                float remappedDist = (dist - _CircleRadius) / (_CircleRadius * (_CircleSharpness - 1.0));
                float finalCircleMask = saturate( remappedDist ); // Ensure 0 to 1 range

                // Apply the HDR color to the mask
                return float4( finalCircleMask.xxx * _CircleColor.rgb, _CircleColor.a );
            }
            ENDCG
        }
    }
}
