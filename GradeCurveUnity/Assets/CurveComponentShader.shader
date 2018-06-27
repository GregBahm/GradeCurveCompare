Shader "Unlit/CurveComponentShader"
{
	Properties
	{
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 objSpace : TEXCOORD1;
				float3 modifiedObjSpace : TEXCOORD2;
				float4 vertex : SV_POSITION;
				float height : TEXCOORD3;
			};

			float _A;
			float _B;
			float _C;
			float _D;
			float _E;

			float GetCurvePoint(float param)
			{
				float ab = lerp(_A, _B, param);
				float bc = lerp(_B, _C, param);
				float cd = lerp(_C, _D, param);
				float de = lerp(_D, _E, param);

				float abc = lerp(ab, bc, param);
				float bcd = lerp(bc, cd, param);
				float cde = lerp(cd, de, param);

				float abcd = lerp(abc, bcd, param);
				float bcde = lerp(bcd, cde, param);

				return lerp(abcd, bcde, param);
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.objSpace = v.vertex + .5;

				float heightStartLerp = saturate(o.objSpace.z * 2);
				float heightEndLerp = saturate(o.objSpace.z * 2 - 1);
				float height = GetCurvePoint(o.objSpace.z);
				v.vertex.y = (o.objSpace.y) * height;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.modifiedObjSpace = v.vertex + .5;
				o.uv = v.uv;
				o.height = height;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 col = lerp(float3(0, 1, 0), float3(1, 0, 0), i.objSpace.z);
				col *= pow(i.height, 2.5) / 5000;
				float stripe = pow(i.objSpace.y,30);
				col = lerp(col, col * 2, stripe);
				return float4(col, 1);
			}
			ENDCG
		}
	}
}
