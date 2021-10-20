// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/AdvancedGemShader/Gemstone_cheap"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_MainColor("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)

		[Header(Inner Renderer)]
		_InnerRefraction("Inner Refraction", Cube) = "dummy.jpg" {}
		_InnerColor("Inner Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_InnerMulti("Inner Multiplier", float) = 1

		[Header(Outer Renderer)]
		_OuterRefraction("Outer Refraction", Cube) = "dummy.jpg" {}
		_OuterColor("Outer Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_OuterMulti("Outer Multiplier", float) = 1

		[Header(Animation Effect)]
		_RotationSpeed("Rotation Speed", Float) = 2.0
	}
		SubShader
		{
			Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

			LOD 100

			//Render the inside
			Pass{

			Cull Front
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			
		struct v2f
		{
			float2 uv : TEXCOORD0;
			UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
			float3 normalDir : TEXCOORD3;
			float3 viewDir : TEXCOORD4;

		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		float4 _MainColor;
		float4 _OuterColor;
		float4 _InnerColor;
		float _InnerMulti;
		float _Alpha;
		samplerCUBE _OuterRefraction;
		samplerCUBE _InnerRefraction;
		float _RotationSpeed;

		v2f vert(appdata_full v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

			float sinX = sin(_RotationSpeed * _Time);
			float cosX = cos(_RotationSpeed * _Time);
			float sinY = sin(_RotationSpeed * _Time);
			float3x3 rotationMatrix = float3x3
				(cosX, -sinX, 0,
					sinY, cosX, 0,
					0, 0, 1);



			UNITY_TRANSFER_FOG(o,o.vertex);
			o.normalDir = normalize(v.normal);
			o.viewDir = normalize(WorldSpaceViewDir(v.vertex)) * -1;


			return o;
		}

		fixed4 frag(v2f i) : SV_Target{

			float3 reflectedDir = reflect(i.viewDir, normalize(i.normalDir));
			float4 refractCol = texCUBE(_InnerRefraction, reflectedDir);


			fixed4 col = _InnerColor;
			UNITY_APPLY_FOG(i.fogCoord, col);

			return col * _InnerMulti * refractCol * _MainColor;
		}


			ENDCG
		}

			//Render the Outside
			Pass{
				Cull Back
				ZWrite On
				Blend SrcAlpha OneMinusSrcAlpha
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"



			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 normalDir : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainColor;
			float4 _OuterColor;
			float _OuterMulti;

			samplerCUBE _OuterRefraction;
			samplerCUBE _InnerRefraction;
			float _RotationSpeed;
			v2f vert(appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				float sinX = sin(_RotationSpeed * _Time);
				float cosX = cos(_RotationSpeed * _Time);
				float sinY = sin(_RotationSpeed * _Time);
				float3x3 rotationMatrix = float3x3
					(cosX, -sinX, 0,
						sinY, cosX, 0,
						0, 0, 1);



				UNITY_TRANSFER_FOG(o,o.vertex);
				o.normalDir = normalize(v.normal);
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex)) * -1;
				o.viewDir = normalize(ObjSpaceViewDir(v.vertex)) * -1;
				//o.viewDir = mul(normalize(WorldSpaceViewDir(v.vertex)) , rotationMatrix);


				return o;
			}

			fixed4 frag(v2f i) : SV_Target{

				float sinX = sin(_RotationSpeed * _Time);
			float cosX = cos(_RotationSpeed * _Time);
			float sinY = sin(_RotationSpeed * _Time);
			float3x3 rotationMatrix = float3x3
				(cosX, -sinX, 0,
					sinY, cosX, 0,
					0, 0, 1);



				float3 reflectedDir = reflect(i.viewDir, normalize(i.normalDir));
				//float4 reflectCol = texCUBE(_OuterRefraction, reflectedDir);
				float4 reflectCol = texCUBE(_OuterRefraction, mul(reflectedDir, rotationMatrix));



				fixed4 tex = tex2D(_MainTex, i.uv);
				fixed4 col = _OuterColor * tex * _MainColor;


				UNITY_APPLY_FOG(i.fogCoord, col);

				return (col * _OuterMulti * reflectCol);
			}
				ENDCG
	}
			
			
		


			//This activates shadow casting
			//UsePass "VertexLit/SHADOWCASTER"


		}
		//Or use this
		FallBack "Specular"
}
