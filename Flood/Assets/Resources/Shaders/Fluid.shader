Shader "Custom/Fluid"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		//_FluidMap("Height Map", 2D) = "black" {}
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
		Texture2D _FluidMap;
		Texture2D _TerrainMap;
		Texture2D _Mask;
		#endif

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        //UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
       // UNITY_INSTANCING_BUFFER_END(Props)
		
		static const float THETA = 0.866025403784438646763723170752936183471402626905190314027f; // sqrt(3)/2
		static const float3 NEIGHBOR_NORMAL_LATERALS[7] = {
			float3(   0, 1,      0),
			float3(  -1, 0,      0),
			float3(-0.5, 0,  THETA),
			float3( 0.5, 0,  THETA),
			float3(   1, 0,      0),
			float3( 0.5, 0, -THETA),
			float3(-0.5, 0, -THETA)
		};

		// int3(x, y, useOddOffset)
		static const int3 NEIGHBOR_OFFSETS[7] = {
			int3(0,  0,  0),
			int3(0,  1,  0),
			int3(-1,  1,  1),
			int3(-1,  0,  1),
			int3(0, -1,  0),
			int3(1,  0,  1),
			int3(1,  1,  1)
		};

		
		void vert(inout appdata_full v) {
			#ifdef SHADER_API_D3D11

			//uint i = round(v.texcoord.x);
			int2 iVec = v.texcoord.xy; // UV is flipped for some reason

			float height = _FluidMap[iVec.yx].r;

			float3 offset = float3(0, height, 0);
			v.vertex.xyz += offset;

			v.normal = float3(0, 1, 0);
			

			float nCount = 0;
			float3 nTotal = float3(0, 0, 0);

			int hasNeighbor;
			int2 nVec;

			int2 oddOffset = int2(0, -(iVec.y & 1));

			

			[unroll(6)] for (int n = 1; n <= 6; n++) {
				nVec = iVec.yx + NEIGHBOR_OFFSETS[n].xy + NEIGHBOR_OFFSETS[n].z * oddOffset;
				hasNeighbor = _Mask[nVec]; // Needs terrain check
				nCount += hasNeighbor;
				nTotal += hasNeighbor * normalize((_FluidMap[nVec].r - height) * NEIGHBOR_NORMAL_LATERALS[n] + float3(0, 1, 0));
			}

			/*
			// N1			
			nVec = iVec.yx + int2(0, 1);
			hasNeighbor = _Mask[nVec]; // Needs terrain check
			nCount += hasNeighbor;
			nTotal += hasNeighbor * normalize((_FluidMap[nVec].r - height) * NEIGHBOR_NORMAL_LATERALS[1] + float3(0, 1, 0));

			// N2
			nVec = iVec.yx + int2(-1, 1 - (iVec.y & 1));
			hasNeighbor = _Mask[nVec]; // Needs terrain check
			nCount += hasNeighbor;
			nTotal += hasNeighbor * normalize((_FluidMap[nVec].r - height) * NEIGHBOR_NORMAL_LATERALS[2] + float3(0, 1, 0));
			
			// N3
			nVec = iVec.yx + int2(-1, -(iVec.y & 1));
			hasNeighbor = _Mask[nVec]; // Needs terrain check
			nCount += hasNeighbor;
			nTotal += hasNeighbor * normalize((_FluidMap[nVec].r - height) * NEIGHBOR_NORMAL_LATERALS[3] + float3(0, 1, 0));

			// N4
			nVec = iVec.yx + int2(0, -1);
			hasNeighbor = _Mask[nVec]; // Needs terrain check
			nCount += hasNeighbor;
			nTotal += hasNeighbor * normalize((_FluidMap[nVec].r - height) * NEIGHBOR_NORMAL_LATERALS[4] + float3(0, 1, 0));

			// N5
			nVec = iVec.yx + int2(1, -(iVec.y & 1));
			hasNeighbor = _Mask[nVec]; // Needs terrain check
			nCount += hasNeighbor;
			nTotal += hasNeighbor * normalize((_FluidMap[nVec].r - height) * NEIGHBOR_NORMAL_LATERALS[5] + float3(0, 1, 0));
			//v.vertex.y = (_FluidMap[nVec] - height);

			// N6
			nVec = iVec.yx + int2(1, 1 - (iVec.y & 1));
			hasNeighbor = _Mask[nVec]; // Needs terrain check
			nCount += hasNeighbor;
			nTotal += hasNeighbor * normalize((_FluidMap[nVec].r - height) * NEIGHBOR_NORMAL_LATERALS[6] + float3(0, 1, 0));
			*/
			//v.vertex.y = nCount;
			//v.vertex.y = _Mask[iVec.yx];
			//v.vertex.y = iVec.y & 1;

			//v.vertex.xyz += normalize(nTotal);

			v.normal = normalize(nTotal);

			/*
			// North
			hasNeighbor = iVec.y < (_Res - 1);
			nCount += hasNeighbor;
			nTotal += normalize(float3(0, 1, hasNeighbor * (height - _FluidMap[iVec.xy + uint2(0, 1)].r)));

			// East
			hasNeighbor = iVec.x < (_Res - 1);
			nCount += hasNeighbor;
			nTotal += normalize(float3(hasNeighbor * (height -_FluidMap[iVec.xy + uint2(1, 0)].r), 1, 0));

			// South
			hasNeighbor = iVec.y > 0;
			nCount += hasNeighbor;
			nTotal += normalize(float3(0, 1, hasNeighbor * (_FluidMap[iVec.xy - uint2(0, 1)].r - height)));

			// West
			hasNeighbor = iVec.x > 0;
			nCount += hasNeighbor;
			nTotal += normalize(float3(hasNeighbor * (_FluidMap[iVec.xy - uint2(1, 0)].r - height), 1, 0));
			*/
			//v.normal = normalize(nTotal / nCount);
			//v.normal = float3(0, 1, 0);

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
