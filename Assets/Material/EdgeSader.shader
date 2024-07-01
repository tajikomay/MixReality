Shader "Custom/EdgeShader"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _EdgeColor("Edge Color", Color) = (0,0,0,1)
        _Threshold("Threshold", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
        };

        half _Threshold;
        float4 _Color;
        float4 _EdgeColor;

        void surf(Input IN, inout SurfaceOutput o)
        {
            float edge = dot(IN.worldNormal, normalize(IN.worldPos));
            if (edge > _Threshold || edge < -_Threshold)
            {
                o.Albedo = _Color.rgb;
            }
            else
            {
                o.Albedo = _EdgeColor.rgb;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
