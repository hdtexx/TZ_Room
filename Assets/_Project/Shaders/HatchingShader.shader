Shader "Custom/HatchingShaderWithAngle"
{
    Properties
    {
        _HatchColor ("Hatch Color", Color) = (0,0,0,1)
        _HatchThickness ("Hatch Thickness", Range(0.1, 10)) = 5
        _HatchAngle ("Hatch Angle", Range(0, 360)) = 45
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
        
        CBUFFER_START(UnityPerMaterial)
            float4 _HatchColor;
            float _HatchThickness;
            float _HatchAngle;
        CBUFFER_END
        
        struct Attributes
        {
            float4 positionOS : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionHCS : SV_POSITION;
            float2 uv : TEXCOORD0;
            float3 positionWS : TEXCOORD1;
        };

        Varyings vert(Attributes IN)
        {
            Varyings OUT;
            OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
            OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
            OUT.uv = IN.uv;
            return OUT;
        }
        ENDHLSL

        Pass
        {
            Name "Hatching"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(Varyings IN) : SV_Target
            {
                float rad = radians(_HatchAngle);
                float2 rotatedUV;
                rotatedUV.x = IN.uv.x * cos(rad) - IN.uv.y * sin(rad);
                rotatedUV.y = IN.uv.x * sin(rad) + IN.uv.y * cos(rad);
                float scaledThickness = _HatchThickness * 0.1;
                float hatchingPattern = fmod(rotatedUV.y, scaledThickness * 2.0) < scaledThickness ? 1.0 : 0.0;
                
                float4 finalColor = float4(_HatchColor.rgb, _HatchColor.a * hatchingPattern);
                
                return finalColor;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}