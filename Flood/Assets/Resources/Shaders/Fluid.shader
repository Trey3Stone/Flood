Shader "Custom/Fluid"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard vertex:vert fullforwardshadows addshadow

        // Use shader model 5.0 target, to get nicer looking lighting
        #pragma target 5.0

		#include "UnityCG.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

		

		uint _Res;

		#ifdef SHADER_API_D3D11
		StructuredBuffer<float> _HeightData;
		#endif

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        //UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
       // UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v) {
			#ifdef SHADER_API_D3D11

			uint i = round(v.texcoord.x);
			uint2 iVec = int2(i % _Res, i / _Res);

			float height = _HeightData[i];

			float3 offset = float3(0, height, 0);
			v.vertex.xyz += offset;


			float nCount = 0;
			float3 nTotal = float3(0, 0, 0);

			int hasNeighbor;

			// North
			hasNeighbor = iVec.y < (_Res - 1);
			nCount += hasNeighbor;
			nTotal += normalize(float3(0, 1, hasNeighbor * (height - _HeightData[i + _Res])));

			// East
			hasNeighbor = iVec.x < (_Res - 1);
			nCount += hasNeighbor;
			nTotal += normalize(float3(hasNeighbor * (height - _HeightData[i + 1]), 1, 0));

			// South
			hasNeighbor = iVec.y > 0;
			nCount += hasNeighbor;
			nTotal += normalize(float3(0, 1, hasNeighbor * (_HeightData[i - _Res] - height)));

			// West
			hasNeighbor = iVec.x > 0;
			nCount += hasNeighbor;
			nTotal += normalize(float3(hasNeighbor * (_HeightData[i - 1] - height), 1, 0));

			v.normal = normalize(nTotal / nCount);

			//v.normal = normalize(v.normal + offset);
			//v.color = half4(v.normal.x, v.normal.y, v.normal.z, 1);
			#endif
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
