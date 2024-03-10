Shader"Unlit/CellShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MyColor ("Color", Color) = (1,1,1,1)
        _CellMoveSpeed ("CellMoveSpeed", Range(0,3)) = 1
        _Zoom ("Zoom", Range(0,50)) = 15
        _Beat ("Beat", Range(0,10)) = 4
        _NoiseSpeed ("NoiseSpeed", Range(0,2)) = 0.2
        _BlendFactor ("BlendFactor", Range(0,1)) = 0.5
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            // Declare the properties as uniforms
            uniform float4 _MyColor; // Color property
            uniform float _CellMoveSpeed; // Speed at which cells move
            uniform float _Zoom; // Zoom level
            uniform float _Beat; // Beat influence
            uniform float _NoiseSpeed; // Speed of the noise effect
            uniform float _BlendFactor; // Factor for blending effects

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // https://www.shadertoy.com/view/4ttXzj
            // https://iquilezles.org/articles/smin
            float smin(float a, float b, float k)
            {
                float h = clamp(0.5 + 0.5 * (b - a) / k, 0.0, 1.0);
                return lerp(b, a, h) - k * h * (1.0 - h);
            }
 
            float cells(float2 uv)
            { // Trimmed down.
                uv = lerp(sin(uv + float2(1.57, 0)), sin(uv.yx * 1.4 + float2(1.57, 0)), .75);
                return uv.x * uv.y * .3 + .7;
            }
 
            static float BEAT = _Beat;
            float fbm(float2 uv)
            {
    
                float f = 200.0;
                float2 r = (float2(.9, .45));
                float2 tmp;
                float T = 100.0 + _Time.y * _CellMoveSpeed;
                T += sin(_Time.y * BEAT) * .1;
                // layers of cells with some scaling and rotation applied.
                for (int i = 1; i < 8; ++i)
                {
                    float fi = float(i);
                    uv.y -= T * .5;
                    uv.x -= T * .4;
                    tmp = uv;
        
                    uv.x = tmp.x * r.x - tmp.y * r.y;
                    uv.y = tmp.x * r.y + tmp.y * r.x;
                    float m = cells(uv);
                    f = smin(f, m, .07);
                }
                return 1. - f;
            }

            float3 g(float2 uv)
            {
                float2 off = float2(0.0, .03);
                float t = fbm(uv);
                float x = t - fbm(uv + off.yx);
                float y = t - fbm(uv + off);
                float s = .0025;
                float3 xv = float3(s, x, 0);
                float3 yv = float3(0, y, s);
                return normalize(cross(xv, -yv)).xzy;
            }

            float3 ld = normalize(float3(1.0, 2.0, 3.));

            float2 hash(float2 p) 
            {
                p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)));
                return -1.0 + 2.0 * frac(sin(p) * 43758.5453123);
            }

            float noise(float2 p)
            {
                const float K1 = 0.366025404; // (sqrt(3)-1)/2;
                const float K2 = 0.211324865; // (3-sqrt(3))/6;

                float2 i = floor(p + (p.x + p.y) * K1);
                float2 a = p - i + (i.x + i.y) * K2;
                float m = step(a.y, a.x);
                float2 o = float2(m, 1.0 - m);
                float2 b = a - o + K2;
                float2 c = a - 1.0 + 2.0 * K2;
                float3 h = max(0.5 - float3(dot(a, a), dot(b, b), dot(c, c)), 0.0); 
                float3 n = h * h * h * h * float3(dot(a, hash(i + 0.0)), dot(b, hash(i + o)), dot(c, hash(i + 1.0)));
    return dot(n, float3(70.0, 70.0, 70.0));
} 
 
            fixed4 frag(v2f i) : SV_Target
            {

                float2 UV = i.uv;
                float2 uv = UV;
                uv -= float2(0.5, 0.5);

                float2 ouv = uv;
                float B = sin(_Time.y * BEAT);
                uv = lerp(uv, uv * sin(B), .035);
                float2 _uv = uv * _Zoom;
                // zoom in
                float f = fbm(_uv);

                // Generate an additional noise layer
                float noiseLayer = noise(uv * 1.0 + _Time.y * _NoiseSpeed);

                // Blend the noise layer with the existing base color
                float combinedEffect = lerp(f, noiseLayer, _BlendFactor); 

                // Base color, now influenced by both the fbm and the added noise layer
                float3 baseColor = float3(_MyColor.r, _MyColor.g + combinedEffect * .05, _MyColor.b + B * .05) * combinedEffect + 0.8 * combinedEffect;

                float3 v = normalize(float3(uv, 1.));
                float3 grad = g(_uv);

                // Specular, rim lighting, and edge darkening calculations remain the same
                float3 H = normalize(ld + v);
                float S = max(0., dot(grad, H));
                S = pow(S, 4.0) * .2;
                baseColor += S * float3(.4, .7, .7);

                float R = 1.0 - clamp(dot(grad, v), .0, 1.);
                baseColor = lerp(baseColor, float3(.8, .8, 1.), smoothstep(-.2, 2.9, R));

                baseColor = lerp(baseColor, float3(0., 0., 0.), smoothstep(.45, .55, (max(abs(ouv.y), abs(ouv.x)))));

                return float4(smoothstep(.0, 1., baseColor), 1.);
            }

            
            ENDCG
        }
    }
}
