Shader "Custom/MenyMeshesShader" {
	SubShader{
		Pass{
		CGPROGRAM

		#pragma vertex vert
		#pragma geometry geom
		#pragma fragment frag

		#include "UnityCG.cginc"

		struct Particle {
		float3 pos :POSITION;
		float3 start;
		float3 end;
		float time;
		float lifeTime;
		int active;
		};

		struct Out {
			float4 pos :SV_POSITION;
		};

		StructuredBuffer<Particle> particleBuffer;
	
		// 頂点シェーダ
		Out vert(uint id : SV_VertexID)
		{
			Out output;
			output.pos = float4(particleBuffer[id].pos,1);

			return output;
		}

		// ジオメトリシェーダ
		[maxvertexcount(4)]
		void geom(point Out input[1], inout TriangleStream<Out> outStream)
		{
			Out output;
			float4 pos = input[0].pos;

			for (int x = 0; x < 2; x++)
			{
				for (int y = 0; y < 2; y++)
				{
					output.pos = pos + float4(float2(x, y) * 0.015, 0, 0);
					output.pos = mul(UNITY_MATRIX_VP, output.pos);

					outStream.Append(output);
				}
			}

			outStream.RestartStrip();
		}

		//フラグメントシェーダー
		fixed4 frag(Out i) : COLOR
		{
			return float4(1,1,1,1);
		}

		ENDCG
	}
	}
}