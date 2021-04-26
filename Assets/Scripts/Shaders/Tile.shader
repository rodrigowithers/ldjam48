Shader "Unlit/Tile"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //_Radius ("Radius", Float) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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
                float3 position : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float3 _PlayerPosition;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.position = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float dist = distance(i.position, _PlayerPosition) / _Radius;
                return lerp(col, 0, dist);
            }
            ENDCG
        }
    }
}
