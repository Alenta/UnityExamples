Shader "Unlit/Hologram"
{
    Properties
    {
        _MainTex ("Albedo Texture", 2D) = "white" {}
        // Declares a "public" variable in the shader inspector
        _TintColor("Tint Color", Color) = (1,1,1,1)
        _Transparency("Transparenct", Range(0.0,0.5)) = 0.25
        _CutoutThresh("Cutout Treshold", Range(0.0,1.0)) = 0.2
        _Distance("Distance", Float) = 1
        _Amplitude("Amplitude", Float) = 1
        _Speed("Speed", Float) = 1
        _Amount("Amount", Range(0.0,1.0)) = 1
        _PixelScale("Pixel Scale", Float) = 1
    }
    SubShader
    {
        // Rendering Order: Background - Geometry - AlphaTest - Transparent - Overlay
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        // ZWrite Off tells shader to not write pixels to "Depth buffer"
        // On for solid objects, off for transparent objects
        ZWrite Off
        // Blend factors: We're telling the shader to blend with source alpha
        // Then blend with OneMinusSrcAlpha
        // SrcAlpha means that we multiply the current value with the value from SrcAlpha
        // OneMinusSrcAlpha means the same, but with 1 - the source value.
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // No need for fog
            // Helper functions from unity ecosystem
            #include "UnityCG.cginc"

            struct appdata
            {
                // Packed array with 4 floats for the mesh Vertex
                // Binds vertex to position
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                // Binds UV data to Texture Coordinate 0
                float2 uv : TEXCOORD0;
                // SV_POSITION means screen space position
                float4 vertex : SV_POSITION;
            };

            // Lets us use _MainTex as a 2D Sample 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TintColor;
            float _Transparency;
            float _CutoutThresh;
            float _Distance;
            float _Amplitude;
            float _Speed;
            float _Amount;
            float _PixelScale;

            v2f vert (appdata v)
            {
                // New Struct "o", short for output
                v2f o;
                // We take the x vertex of v, 
                v.vertex.x += (sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount) * _PixelScale;

                // UnityObjectToClipPos Converts from Local Space to Clip Space
                // Space Matrix Transforms: Local -> World -> View -> Clip -> Screen
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            

            // Takes in a v2f struct i, SV_Target is a Render Target
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // col for color, 4 floats (3 colors and alpha)
                // Here, it uses _MainTex and the data from the materials UV
                fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
                col.a = _Transparency;
                // Pixels that are less than a certain amount of red does not get rendered
                clip(col.r - _CutoutThresh);
                return col;
            }
            ENDCG
        }
    }
}

// Data types in HLSL:
            
// Float (32bit High precision, works everywhere but more expensive), 
// Half(16bit, usable for most uses when you need lower cost), 
// Fixed (-1 to 1, Low precision, cheap)
// float4 -> half4 -> fixed4
// float4x4 (C# Matrix4x4)
// bool 0 1
// bool4
// int, gets converted to float anyway
// int2

// Common methods:
// UnityObjectToClipPos(x.vertex) -> Converts from Local Space to Clip Space
// UnityObjectToWorldNormal(x.normal) -> Converts from Local space to World Space, but also has some extra steps to handle normals
// Manual version of the above line: o.normal = mul( (float3x3)unity_ObjectToWorld, v.normals);
// Another version that does the same: o.normal = mul(x.normals, (float3x3)unity_WorldToObject); 
// UNITY_MATRIX_M can also be used instead of unity_ObjectToWorld, I think Matrix_M is standard HLSL, but ObjectToWorld is specific to unity. Also relates to platforms.
// Unity recommends ObjectToWorld.
// Method says WorldToObject, but this will output ObjectToWorld. 
// This is because of the multiply order, which will transpose the matrix, giving us the Object space translated to World space.

// Typical ways to optimize a shader:
// How many vertices do you have, vs how many pixels / fragments do you have?
// Usually you have more pixels than verts. 
// Then, you would want to do as much as possible of the work and calculations in the vertex shader.
// As little as possible in the fragment shader.
// If the situation is swapped (Rare) but if you have more pixels than verts, the reverse will be true.
// Then frag shader is "cheaper" than vert shader. 

// Some concepts/tricks: A shader treats colors as vectors, and opposite. 
// You can "splat" one part of a multiple float (float4 f.ex) like so:
// In the vert shader:
// o.uv = v.uv;
// In the frag shader: 
// return float4(o.uv.xxx,1); <- Note the three 'x'es.
// This will take the "vector" (But actually float) values from the UV.x, and pass it across the R, G and B channels
// This will then result in a horizontal gradient map with monochrome colors