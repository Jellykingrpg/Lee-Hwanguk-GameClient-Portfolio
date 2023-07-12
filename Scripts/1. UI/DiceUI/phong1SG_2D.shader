Shader "Custom/SpriteShader" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Sprite Texture", 2D) = "white" {}
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform float4 _Color;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Vertex Shader �Լ�
            v2f vert(appdata v) {
                v2f o;
                // Sprite�� UV ��ǥ�� ����
                o.uv = v.uv;
                // Sprite�� ���� ��ǥ�� ����
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            // Fragment Shader �Լ�
            // Sprite�� �ȼ� ���� ���� ���
            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color*0.8; //�ʹ� ��� ->0.8�� �� ����
            // Sprite�� Alpha���� �������� �������� ���
            col.a = tex2D(_MainTex, i.uv).a;
            return col;
        }
        ENDCG
    }
    }
        FallBack "Diffuse"
}
