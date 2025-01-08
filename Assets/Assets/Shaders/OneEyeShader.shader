Shader "Custom/OneEyeShader"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _EyeVisible ("Visible Eye (0: Left, 1: Right)", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Properties
            fixed4 _MainColor;
            float _EyeVisible;

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                if(_EyeVisible <=-1) discard;


                // Check the active eye
                if (unity_StereoEyeIndex != _EyeVisible)
                {
                    discard; // Skip rendering for the other eye
                }
                return _MainColor;
            }
            ENDCG
        }
    }
}
