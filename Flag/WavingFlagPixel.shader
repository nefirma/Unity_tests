Shader "Custom/WavingFlagPixel" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Speed ("Speed", Range(0, 10)) = 1
        _Amplitude ("Amplitude", Range(0, 1)) = 0.1
        _WaveLength ("Wave Length", Range(0, 1)) = 0.1
    }


    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 200

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Speed;
            float _Amplitude;
            float _WaveLength;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target {
                float4 col = tex2D(_MainTex, i.uv);
                float2 uv = i.uv;
                float offset = _Speed * _Time.y + uv.x * _WaveLength;
                float wave = _Amplitude * sin(offset * 3.141592);
                uv.y += wave;
                return tex2D(_MainTex, uv) * col;
            }
            ENDCG
        }
    }
FallBack "Diffuse"
}