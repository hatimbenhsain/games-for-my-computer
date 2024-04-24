Shader "Unlit/DiscoFloor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            // from https://shadertoyunofficial.wordpress.com/2019/01/02/programming-tricks-in-shadertoy-glsl/
#define hash21(p) frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453)

// from https://www.shadertoy.com/view/Xt2BDc
#define hash31(p) frac(sin(dot(p, float3(17, 1527, 113))) * 43758.5453123)

#define blend(dest, source) dest = lerp(dest, float4(source.rgb, 1.0), source.a);
#define add(dest, source) dest += source * source.a;
#define mod(x, y) (x - y*floor(x/y)) 

const float glow_level = 0.7;
const float glow_attenuation = 6.0;
const float zoom = 2.5;
const float tile_radius = 0.45;
const float frame_level = 0.05;
const float lit_tile_threshold = 0.75;
const float unlit_tile_level = 0.15;

// from https://www.shadertoy.com/view/MsS3Wc
float3 hsv2rgb(in float3 c)
{
    float3 rgb = clamp(abs(mod(c.x * 6.0 + float3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
    return c.z * lerp(float3(1.0, 1.0, 1.0), rgb, c.y);
}

// from https://iquilezles.org/articles/distfunctions2d
float sdBox(in float2 p, in float2 b)
{
    float2 d = abs(p) - b;
    return length(max(d, float2(0.0, 0.0))) + min(max(d.x, d.y), 0.0);
}

void get_tile_colour(in float2 tile_id, out float3 tile_hsv)
{
    // hue
    tile_hsv.x = hash21(tile_id);
    // saturation
    tile_hsv.y = 1.0;
    // value
    float modTime = mod(_Time.y, 100.0);
    float time = floor(modTime * 2.0);
    float level = step(lit_tile_threshold, hash31(float3(tile_id, time)));
    float lastLevel = step(lit_tile_threshold, hash31(float3(tile_id, time - 1.0)));
    level = max(level, lastLevel * (1.0 - frac(modTime * 2.0) * 1.5));
    tile_hsv.z = level;
}

#define ADD_GLOW(tile) \
get_tile_colour(tile_id + tile, tile_hsv); \
add(color, float4(hsv2rgb(tile_hsv), pow(1.0 - sdBox(tile - tile_coord, float2(tile_radius, tile_radius)), glow_attenuation) * glow_level * tile_hsv.z));

fixed4 frag(v2f i) : SV_Target
{
    float2 R = 1/_ScreenParams.xy; // Unity's built-in variable for screen dimensions
    float2 U = ((2.0 * i.uv) - R) / min(R.x, R.y) * zoom; // Adjust zoom is used properly
    float2 FU = frac(U); // 'fract' in GLSL is 'frac' in HLSL

    // unique ID for the tile
    float2 tile_id = floor(U);
    // local tile coords [-0.5, 0.5]
    float2 tile_coord = FU - 0.5;
    // distance from edge of light
    float tile_dist = sdBox(tile_coord, float2(tile_radius, tile_radius));
    float4 color = float4(float3(frame_level, frame_level, frame_level), 1.0);

    // get tile's colour
    float3 tile_hsv;
    get_tile_colour(tile_id, tile_hsv);

    // calculate a vignette to apply to the saturation (from https://www.shadertoy.com/view/lsKSWR)
    float2 vignette_coord = FU * (1.0 - FU.yx);
    float vignette = sqrt(vignette_coord.x * vignette_coord.y * 5.0);

    // render the tile
    float3 light_colour = hsv2rgb(float3(tile_hsv.x, 1.0 - (tile_hsv.z * vignette), max(tile_hsv.z, unlit_tile_level)));
    blend(color, float4(light_colour, step(tile_dist, 0.0)));
    // render the tile's own glow on the frame
    add(color, float4(hsv2rgb(tile_hsv), pow(1.0 - max(tile_dist, 0.0), glow_attenuation) * glow_level * tile_hsv.z));

    // get vector to the three nearest neighbours
    float2 neighbours = float2((tile_coord.x < 0.0) ? -1.0 : 1.0, (tile_coord.y < 0.0) ? -1.0 : 1.0);
    // render the neighbours' glows
    ADD_GLOW(neighbours)
    ADD_GLOW(float2(neighbours.x, 0.0))
    ADD_GLOW(float2(0.0, neighbours.y))

    // This return statement must be at the end after all modifications to the color.
    return color;
}

            ENDCG
        }
    }
}
