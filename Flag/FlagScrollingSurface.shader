Shader "Custom/FlagScrollingSurface" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Scroll Speed", Range(0, 10)) = 1
        _Offsets ("Offsets", Vector) = (0,0,0,0)
        _WaveSpeed ("WaveSpeed", Range(0, 5.0)) = 1 //
        _Frequency ("Frequency", Range(0, 1.3)) = 1 //
        _Amplitude ("Amplitude", Range(0, 5.0)) = 1 //
    }

    SubShader {
        Tags {"RenderType"="Opaque"}
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert // allows the be compiled by GPU as a vertex shader
            #pragma fragment frag // the same as above, but for a fragment shader
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
            float4 _MainTex_ST;
            float4 _Offsets;
            float _Speed;
            float _WaveSpeed;
            float _Frequency;//
            float _Amplitude;// 

            v2f vert (appdata v) {
                v2f o;
                //o.vertex = UnityObjectToClipPos(v.vertex);
                v.vertex.z +=  cos((v.vertex.x + _Time.y * _WaveSpeed) * _Frequency) * _Amplitude * (v.vertex.x - 5); //
                o.vertex = UnityObjectToClipPos(v.vertex); //
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                const float2 scroll = float2(_Time.y * _Speed, 0);
                const float2 uv = i.uv + scroll;
                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
