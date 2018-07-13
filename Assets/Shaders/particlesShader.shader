Shader "Unlit/particlesShader"
{
	SubShader
	{      
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
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
                float4 color :COLOR;
            };

            StructuredBuffer<Particle> particleBuffer;
			
			Out vert (uint id : SV_VertexID)
			{
				Out o;
				o.pos = float4(particleBuffer[id].pos,1);
                o.pos = UnityObjectToClipPos(o.pos);
                o.color = float4(particleBuffer[id].time,particleBuffer[id].time,particleBuffer[id].time,1);
				return o;
			}

			fixed4 frag (Out i) : COLOR
			{
				return i.color;
			}
			ENDCG
		}
	}
}
