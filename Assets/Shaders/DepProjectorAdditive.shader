Shader "Unlit/DepProjector/Additive"
{
	Properties
	{
		_Shadow ("Shadow", 2D) = "white" {}
		_FalloffTex("FallOff", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="true" "DisableBatching"="true" }
		LOD 100

		Pass
		{
			ZWrite Off
			ColorMask RGB
			Blend SrcAlpha One
			Offset -1, -1
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 proj : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _Shadow;
			sampler2D _FalloffTex;
			sampler2D _CameraDepthTexture;

			float4x4 internal_WorldToProjector;
			float4x4 internal_WorldToProjectorClip;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.proj = ComputeGrabScreenPos(o.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 screenUV = i.proj.xy / i.proj.w;
				float depth = tex2D(_CameraDepthTexture, screenUV).r;
				fixed4 projPos = fixed4(screenUV.x * 2 - 1, screenUV.y * 2 - 1, -depth * 2 + 1, 1);

				projPos = mul(unity_CameraInvProjection, projPos);
				projPos = mul(unity_MatrixInvV, projPos);
				half4 clippos = mul(internal_WorldToProjectorClip, projPos);
				clippos /= clippos.w;
				projPos = mul(internal_WorldToProjector, projPos);
				projPos /= projPos.w;

				fixed2 pjUV = projPos.xy*0.5 + 0.5;

				fixed2 discardAlpha = step(0, pjUV.xy)*step(pjUV.xy, 1);

				fixed4 col = tex2D(_Shadow, pjUV);
				fixed4 texF = tex2D(_FalloffTex, clippos.xy);

				col.rgb *= discardAlpha.x*discardAlpha.y*texF.g;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
