// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/FluidOLD"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		//Cull Back

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma surface surf Lambert vertex:vert addshadow

			// Use shader model 3.0 target, to get nicer looking lighting
			//#pragma target 3.0

			#include "UnityCG.cginc"

			int _Res;

			#ifdef SHADER_API_D3D11
			StructuredBuffer<float> _HeightData;
			#endif

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			//UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			//UNITY_INSTANCING_BUFFER_END(Props);

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata_full v) {
				v2f o;
				//float height = HeightData[round(v.vertex.TEXCOORD0)];

				//v.vertex.
				o.uv = float2(0, 0);
				//v.vertex.xyz += float3(0, 1.0f, 0);
				o.vertex = float4(v.vertex.x, v.vertex.y, v.vertex.z, 1);
				return o;
			}

			fixed4 _Color;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(1, 1, 1, 1);//tex2D(_MainTex, i.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}

			ENDCG
		}
	}
}
