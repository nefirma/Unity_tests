Shader "Custom/WavePixelShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _WaveScale ("Wave Scale", Float) = 1.0
//        _WaveSpeed ("WaveSpeed", Range(0, 5.0)) = 1 //
        _Frequency ("Frequency", Range(0, 1.3)) = 1 //
        _Amplitude ("Amplitude", Range(0, 5.0)) = 1 //
    }

    SubShader {
        Tags {"RenderType"="Opaque"}
        LOD 100

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
            float _WaveSpeed;
            float _WaveScale;
            float _Frequency;//
            float _Amplitude;// 

            v2f vert (appdata v) {
                v2f o;
                // o.vertex = UnityObjectToClipPos(v.vertex);
                v.vertex.z +=  cos((v.vertex.x + _Time.y * _WaveSpeed) * _Frequency) * _Amplitude * (v.vertex.x - 5); //
                o.vertex = UnityObjectToClipPos(v.vertex); //
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target {
                float4 col = tex2D(_MainTex, i.uv);
                float wave = sin(_Time.y * _WaveSpeed + i.uv.x * _WaveScale);
                float4 waveCol = float4(wave, wave, wave, 1.0);
                return col * waveCol;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
