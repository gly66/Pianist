Shader "Unlit/451Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SecTex ("Second Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			CGPROGRAM
			#pragma vertex MyVert
			#pragma fragment MyFrag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float3 vertexWC : TEXCOORD3;
			};

            // our own matrix
            float4x4 MyXformMat;  // our own transform matrix!!
            fixed4   MyColor;

			// Texture support
			sampler2D _MainTex;
			sampler2D _SecTex;
			float4 _MainTex_ST;
			float4 _SecTex_ST; // must define to support TRANSFORM_TEX

			float MyTexOffset_X;
			float MyTexOffset_Y;
			float MyTexScale_X;
			float MyTexScale_Y;

			float4 LightPosition;
			float4 LightColor;

			float3 DirectionalLight;
			float4 DirColor;

			int FLAG = 1;
			
			v2f MyVert (appdata v)
			{
				v2f o;
                
                // Can use one of the followings:
                // o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);  // Camera + GameObject transform TRS

                o.vertex = mul(MyXformMat, v.vertex);  // use our own transform matrix!
                    // MUST apply before camrea!

                o.vertex = mul(UNITY_MATRIX_VP, o.vertex);   // camera transform only                
				
				// Texture support
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.uv1 = TRANSFORM_TEX(v.uv1, _SecTex);
				o.uv1.x = v.uv1.x * MyTexScale_X + MyTexOffset_X;
				o.uv1.y = v.uv1.y * MyTexScale_Y + MyTexOffset_Y;
	
				o.vertexWC = mul(UNITY_MATRIX_M, v.vertex); // this is in WC space!
                // this is not pretty but we don't have access to inverse-transpose ...
				float3 p = v.vertex + v.normal;
				p = mul(UNITY_MATRIX_M, float4(p, 1)); // now in WC space
				o.normal = normalize(p - o.vertexWC); // NOTE: this is in the world space!!
                return o;
			}
            // our own function
			fixed4 ComputeDiffuse(v2f i)
			{
				float3 l = normalize(LightPosition - i.vertexWC);
				return clamp(dot(i.normal, l), 0, 1) * LightColor;
			}
			fixed4 ComputeDir(v2f i)
			{
			 return clamp(dot(i.normal, DirectionalLight), 0, 1)*DirColor;
			}
			
			fixed4 MyFrag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 c1 = tex2D(_MainTex, i.uv);
				fixed4 c2 = tex2D(_SecTex, i.uv1);
	
				float diff = ComputeDiffuse(i);
				float dir = ComputeDir(i);
				if(FLAG == 1)
					return c2 * (diff + dir);
				else
					return c1 * (diff + dir);

}
			ENDCG
		}
	}
}