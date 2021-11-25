// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GlitterNails_v04"
{
	Properties
	{
		_BasesColor("Bases Color", Color) = (0,0,0,0)
		[Header(Coat Spec)][Space]_SpecularB("Specular B", Range( 0 , 1)) = 0
		[Header(Coat Spec)][Space]_SpecularA("Specular A", Range( 0 , 1)) = 0
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
		_SmoothnessA("Smoothness A", Range( 0 , 1)) = 0
		_SmoothnessB("Smoothness B", Range( 0 , 1)) = 0
		_Scale("Scale", Float) = 1
		_Falloff("Falloff", Float) = 1
		[Header(Glitter)][NoScaleOffset][Space]_Gliter("Gliter", 2D) = "white" {}
		_GlitterXScale("Glitter X Scale", Float) = 1
		_GlitterYScale("Glitter Y Scale", Float) = 1
		[HDR]_GlitterColor("Glitter Color", Color) = (0,0,0,0)
		_GlitterContrust("Glitter Contrust", Range( 1 , 2)) = 1
		_GlitterBrightness("Glitter Brightness", Range( 1 , 2)) = 1
		[Header(Spec 2)][Space]_Spec2Mix("Spec 2 Mix", Range( 0 , 1)) = 0
		_Spec2Shininess("Spec 2 Shininess", Range( 0 , 10)) = 0
		_Spec2Falloff("Spec 2 Falloff", Range( -1 , 1)) = 0
		[Header(French)][Space]_FrenchColor("French Color", Color) = (0,0,0,0)
		_FrenchSpecular("French Specular", Range( 0 , 1)) = 0
		_FrenchSmoothness("French Smoothness", Range( 0 , 1)) = 0
		[NoScaleOffset][Space]_FrenchColorMap("French Color Map", 2D) = "white" {}
		[Header(Color Maps)][NoScaleOffset][Space]_BaseColorMap("Base Color Map", 2D) = "white" {}
		[NoScaleOffset]_SpecularColorMap("Specular Color Map", 2D) = "white" {}
		[NoScaleOffset]_GlitterColorMap("Glitter Color Map", 2D) = "white" {}
		_GlitterMask("Glitter Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
		};

		uniform float4 _FrenchColor;
		uniform sampler2D _FrenchColorMap;
		uniform float4 _BasesColor;
		uniform sampler2D _BaseColorMap;
		uniform float4 _GlitterColor;
		uniform float _Spec2Falloff;
		uniform float _Spec2Shininess;
		uniform float _Spec2Mix;
		uniform sampler2D _Gliter;
		uniform float _GlitterXScale;
		uniform float _GlitterYScale;
		uniform float _GlitterContrust;
		uniform float _GlitterBrightness;
		uniform sampler2D _GlitterMask;
		uniform float4 _GlitterMask_ST;
		uniform sampler2D _GlitterColorMap;
		uniform float _FrenchSpecular;
		uniform float4 _SpecularColor;
		uniform sampler2D _SpecularColorMap;
		uniform float _Scale;
		uniform float _Falloff;
		uniform float _SpecularA;
		uniform float _SpecularB;
		uniform float _FrenchSmoothness;
		uniform float _SmoothnessA;
		uniform float _SmoothnessB;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_FrenchColorMap157 = i.uv_texcoord;
			float4 FrenchMap158 = tex2D( _FrenchColorMap, uv_FrenchColorMap157 );
			float2 uv_BaseColorMap134 = i.uv_texcoord;
			float4 lerpResult120 = lerp( ( _FrenchColor * ( 1.0 - FrenchMap158 ) ) , ( _BasesColor * tex2D( _BaseColorMap, uv_BaseColorMap134 ) ) , FrenchMap158);
			o.Albedo = lerpResult120.rgb;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float4 transform31 = mul(unity_ObjectToWorld,float4( ase_vertexNormal , 0.0 ));
			float3 appendResult33 = (float3(transform31.x , transform31.y , transform31.z));
			float3 normalizeResult43 = normalize( reflect( -ase_worldlightDir , appendResult33 ) );
			float3 normalizeResult42 = normalize( i.viewDir );
			float dotResult48 = dot( normalizeResult43 , normalizeResult42 );
			float specular2Mask57 = pow( saturate( (0.0 + (dotResult48 - _Spec2Falloff) * (1.0 - 0.0) / (1.0 - _Spec2Falloff)) ) , _Spec2Shininess );
			float2 break13 = i.uv_texcoord;
			float2 appendResult25 = (float2(( break13.x * _GlitterXScale ) , ( break13.y * _GlitterYScale )));
			float2 GlitterUV35 = appendResult25;
			float4 tex2DNode44 = tex2D( _Gliter, GlitterUV35 );
			float fresnelNdotV5 = dot( ase_worldNormal, ase_worldlightDir );
			float fresnelNode5 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV5, 0.5 ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float simplePerlin2D9 = snoise( i.uv_texcoord*5.0 );
			simplePerlin2D9 = simplePerlin2D9*0.5 + 0.5;
			float fresnelNdotV10 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode10 = ( -1.0 + simplePerlin2D9 * pow( 1.0 - fresnelNdotV10, 0.5 ) );
			float2 appendResult14 = (float2(saturate( ( 1.0 - fresnelNode5 ) ) , fresnelNode10));
			float simplePerlin2D29 = snoise( appendResult14*4.0 );
			simplePerlin2D29 = simplePerlin2D29*0.5 + 0.5;
			float glitterBands34 = simplePerlin2D29;
			float lerpResult47 = lerp( tex2DNode44.r , tex2DNode44.b , saturate( glitterBands34 ));
			float glitterTex59 = saturate( ( pow( lerpResult47 , _GlitterContrust ) * _GlitterBrightness ) );
			float temp_output_67_0 = ( specular2Mask57 * glitterTex59 );
			float4 lerpResult86 = lerp( float4( 0,0,0,0 ) , _GlitterColor , saturate( (0.0 + (( ( specular2Mask57 * _Spec2Mix ) + temp_output_67_0 ) - 0.0) * (1.0 - 0.0) / (0.5 - 0.0)) ));
			float4 glitterColorOut89 = lerpResult86;
			float2 uv_GlitterMask = i.uv_texcoord * _GlitterMask_ST.xy + _GlitterMask_ST.zw;
			float4 lerpResult144 = lerp( float4( 0,0,0,0 ) , glitterColorOut89 , tex2D( _GlitterMask, uv_GlitterMask ).a);
			float2 uv_GlitterColorMap138 = i.uv_texcoord;
			float4 lerpResult123 = lerp( float4( 0,0,0,0 ) , ( lerpResult144 * tex2D( _GlitterColorMap, uv_GlitterColorMap138 ) ) , FrenchMap158);
			o.Emission = lerpResult123.rgb;
			float4 temp_cast_3 = (_FrenchSpecular).xxxx;
			float2 uv_SpecularColorMap136 = i.uv_texcoord;
			float fresnelNdotV80 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode80 = ( 0.0 + _Scale * pow( 1.0 - fresnelNdotV80, _Falloff ) );
			float lerpResult150 = lerp( _SpecularA , _SpecularB , tex2D( _GlitterMask, uv_GlitterMask ).a);
			float specular127 = lerpResult150;
			float glitterSpecOut88 = ( temp_output_67_0 + ( fresnelNode80 * specular127 ) );
			float lerpResult143 = lerp( 0.0 , glitterSpecOut88 , tex2D( _GlitterMask, uv_GlitterMask ).a);
			float4 lerpResult125 = lerp( temp_cast_3 , ( ( _SpecularColor * tex2D( _SpecularColorMap, uv_SpecularColorMap136 ) ) * lerpResult143 ) , FrenchMap158);
			o.Specular = lerpResult125.rgb;
			float lerpResult148 = lerp( _SmoothnessA , _SmoothnessB , tex2D( _GlitterMask, uv_GlitterMask ).a);
			float lerpResult132 = lerp( _FrenchSmoothness , lerpResult148 , FrenchMap158.r);
			o.Smoothness = lerpResult132;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
0;0;1280;659;-42.09592;1291.067;1.6;True;True
Node;AmplifyShaderEditor.CommentaryNode;1;-2172.558,-879.8588;Inherit;False;2026.713;648.2758;Comment;12;34;29;14;12;10;9;8;6;5;4;3;2;Glitter Bands;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-2016.622,-661.8967;Inherit;False;Constant;_Float0;Float 0;16;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;3;-2139.99,-566.8857;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-1907.659,-481.5024;Inherit;False;Constant;_scale;scale;13;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;5;-1779.082,-812.4265;Inherit;False;Standard;WorldNormal;LightDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;7;-2156.858,-70.31763;Inherit;False;1323.088;468.853;Comment;8;35;25;20;19;17;16;13;11;Glitter UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;6;-1535.471,-809.6046;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;11;-2106.858,-17.51563;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-1888.315,-325.793;Inherit;False;Constant;_power;power;14;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;9;-1709.011,-583.3608;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1866.297,115.0614;Inherit;False;Property;_GlitterXScale;Glitter X Scale;9;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;18;-2176.866,-1931.341;Inherit;False;2395.55;912.718;Comment;16;57;56;53;52;49;48;45;43;42;39;37;33;32;31;28;26;Glitter Spec;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1868.299,218.7604;Inherit;False;Property;_GlitterYScale;Glitter Y Scale;10;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;13;-1817.682,-20.31763;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.FresnelNode;10;-1424.478,-670.062;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;12;-1354.771,-802.2291;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-1621.54,-18.64063;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;-1131.269,-717.163;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1613.159,125.5294;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;26;-2117.744,-1616.544;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;28;-1985.852,-1862.709;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;31;-1929.45,-1616.825;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;29;-688.3483,-705.2921;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1443.932,51.70337;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NegateNode;32;-1709.647,-1796.259;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-1719.291,-1608.733;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;36;-2159.003,577.7773;Inherit;False;1753.309;506.9113;Comment;11;59;55;54;51;50;47;46;44;41;40;38;Glitter Tex;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;34;-422.1442,-683.2936;Inherit;False;glitterBands;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-1057.77,183.0424;Inherit;False;GlitterUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ReflectOpNode;37;-1505.245,-1728.692;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;-2046.603,869.0154;Inherit;False;34;glitterBands;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;-2021.866,660.6444;Inherit;False;35;GlitterUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;39;-1853.885,-1219.877;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;42;-1367.664,-1210.419;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;44;-1825.751,627.7764;Inherit;True;Property;_Gliter;Gliter;8;2;[Header];[NoScaleOffset];Create;True;1;Glitter;0;0;False;1;Space;False;-1;27734a44c603f2a4b9380852e981d1ce;27734a44c603f2a4b9380852e981d1ce;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;41;-1587.216,872.1733;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;43;-1329.56,-1730.906;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;48;-1017.317,-1548.257;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1433.276,917.4944;Inherit;False;Property;_GlitterContrust;Glitter Contrust;12;0;Create;True;0;0;0;False;0;False;1;0;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-984.0682,-1314.873;Inherit;False;Property;_Spec2Falloff;Spec 2 Falloff;17;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;47;-1424.622,754.0564;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;141;81.20331,632.399;Inherit;True;Property;_GlitterMask;Glitter Mask;29;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PowerNode;51;-1192.736,769.1573;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;49;-770.6454,-1542.918;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1143.732,915.7544;Inherit;False;Property;_GlitterBrightness;Glitter Brightness;13;0;Create;True;0;0;0;False;0;False;1;0;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-960.7343,772.3553;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;-483.0511,-1542.975;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-572.0571,-1379.417;Inherit;False;Property;_Spec2Shininess;Spec 2 Shininess;16;0;Create;True;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;153;110.067,1054.876;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.WireNode;154;-2283.801,1155.273;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SaturateNode;55;-826.7592,777.8644;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;56;-271.7642,-1546.016;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;59;-669.8392,766.6034;Inherit;False;glitterTex;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-59.64124,-1553.462;Inherit;False;specular2Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;58;-2169.599,1238.665;Inherit;False;3112.997;1612.87;Comment;23;89;88;87;86;82;81;80;78;76;75;72;71;70;67;65;63;62;61;60;127;150;151;152;Glitter Comp;1,1,1,1;0;0
Node;AmplifyShaderEditor.WireNode;156;-2324.586,1387.445;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;63;-2065.474,2155.696;Inherit;False;59;glitterTex;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;155;-2246.149,2620.46;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;60;-1703.482,1783.307;Inherit;False;57;specular2Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-2093.518,2061.029;Inherit;False;57;specular2Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1773.883,1887.305;Inherit;False;Property;_Spec2Mix;Spec 2 Mix;15;1;[Header];Create;True;1;Spec 2;0;0;False;1;Space;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-1716.289,2101.796;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-1583.125,2459.443;Inherit;False;Property;_SpecularA;Specular A;2;1;[Header];Create;True;1;Coat Spec;0;0;False;1;Space;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-1583.751,2545.845;Inherit;False;Property;_SpecularB;Specular B;1;1;[Header];Create;True;1;Coat Spec;0;0;False;1;Space;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;151;-1587.78,2629.894;Inherit;True;Property;_TextureSample3;Texture Sample 3;27;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-1460.283,1844.104;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;150;-1234.884,2555.755;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-2155.871,2417.217;Inherit;False;Property;_Scale;Scale;6;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-1258.684,2000.905;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2152.603,2509.989;Inherit;False;Property;_Falloff;Falloff;7;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;76;-266.8182,1962.227;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;80;-1855.377,2321.106;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;-1027.781,2468.82;Inherit;False;specular;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-759.7172,2326.393;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;75;-64.81764,1596.385;Inherit;False;Property;_GlitterColor;Glitter Color;11;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;82;-67.54724,1954.486;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;86;288.2578,1882.906;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;87;-590.5942,2222.993;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;88;-434.4874,2235.628;Inherit;False;glitterSpecOut;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;89;633.6365,1800.55;Inherit;False;glitterColorOut;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;157;630.4677,-1063.859;Inherit;True;Property;_FrenchColorMap;French Color Map;21;1;[NoScaleOffset];Create;True;1;Color Maps;0;0;False;1;Space;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;92;415.4106,524.6707;Inherit;False;88;glitterSpecOut;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;94;647.4688,98.94961;Inherit;False;Property;_SpecularColor;Specular Color;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;91;574.2351,-360.2213;Inherit;False;89;glitterColorOut;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;145;482.5179,-214.0026;Inherit;True;Property;_TextureSample1;Texture Sample 1;27;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;136;559.105,272.3594;Inherit;True;Property;_SpecularColorMap;Specular Color Map;27;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;142;316.2205,632.2168;Inherit;True;Property;_TextureSample0;Texture Sample 0;27;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;158;942.182,-895.2562;Inherit;False;FrenchMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;943.1457,171.3617;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;115;1118.67,-1185.696;Inherit;False;Property;_FrenchColor;French Color;18;1;[Header];Create;True;1;French;0;0;False;1;Space;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;147;816.9741,940.396;Inherit;False;Property;_SmoothnessB;Smoothness B;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;134;730.1693,-602.465;Inherit;True;Property;_BaseColorMap;Base Color Map;26;2;[Header];[NoScaleOffset];Create;True;1;Color Maps;0;0;False;1;Space;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;149;796.2161,1023.43;Inherit;True;Property;_TextureSample2;Texture Sample 2;27;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;143;677.8223,627.4149;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;144;844.12,-218.8045;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;159;1158.761,-985.8473;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;83;795.5232,-792.2378;Inherit;False;Property;_BasesColor;Bases Color;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;93;816.0447,840.4612;Inherit;False;Property;_SmoothnessA;Smoothness A;4;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;138;1013.006,-151.1347;Inherit;True;Property;_GlitterColorMap;Glitter Color Map;28;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;1447.49,-1005.916;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;148;1149.112,949.2914;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;1415.485,385.905;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;131;1282.833,830.8582;Inherit;False;Property;_FrenchSmoothness;French Smoothness;20;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;1357.855,1078.112;Inherit;False;158;FrenchMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;119;1285.232,-382.8984;Inherit;False;158;FrenchMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;1157.524,77.74589;Inherit;False;158;FrenchMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;1325.955,-216.8803;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;135;1111.069,-679.1651;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;101;-2163.291,2913.94;Inherit;False;943.9999;261;Comment;4;98;97;96;99;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;126;1381.617,518.0152;Inherit;False;158;FrenchMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;114;-2151.567,3305.042;Inherit;False;2478.958;697.8691;Comment;16;102;113;116;117;121;122;112;110;111;109;107;108;106;105;104;103;French Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;130;1303.878,271.1384;Inherit;False;Property;_FrenchSpecular;French Specular;19;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;116;-143.7567,3610.202;Inherit;False;Property;_InvertFrench;Invert French;24;0;Create;True;0;0;0;False;0;False;0;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;-2108.948,3747.628;Inherit;False;Property;_ZOffset;Z Offset;23;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;104;-1886.218,3562.937;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;103;-1893.823,3750.663;Inherit;False;zOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;106;-1667.599,3729.151;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;108;-1608.935,3872.766;Inherit;False;Property;_Radius;Radius;22;0;Create;True;1;Radius;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;107;-1476.023,3658.544;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;109;-1232.358,3493.924;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;110;-1225.625,3403.018;Inherit;False;103;zOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;113;57.66105,3609.215;Inherit;False;frenchMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-2113.291,3058.94;Inherit;False;Property;_NormalStrength;Normal Strength;14;0;Create;True;0;0;0;False;0;False;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;99;-1443.291,2977.94;Inherit;False;normalOut;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;132;1624.575,879.8838;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;96;-1756.18,2982.582;Inherit;False;Normal From Height;-1;;1;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;1;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;125;1668.723,337.0457;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;97;-2033.291,2963.94;Inherit;False;59;glitterTex;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;123;1509.379,-239.4614;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Compare;111;-1206.701,3654.584;Inherit;False;3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;112;-906.2792,3537.641;Inherit;False;3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;122;-891.8938,3740.314;Inherit;False;3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;121;-600.3349,3602.312;Inherit;False;Property;_FlipFrench;Flip French;25;0;Create;True;0;0;0;False;0;False;0;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;117;-341.7569,3723.202;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;120;1585.772,-524.822;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;105;-1667.599,3600.575;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2230.254,-122.8114;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;GlitterNails_v04;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;3;2;0
WireConnection;6;0;5;0
WireConnection;9;0;3;0
WireConnection;9;1;4;0
WireConnection;13;0;11;0
WireConnection;10;2;9;0
WireConnection;10;3;8;0
WireConnection;12;0;6;0
WireConnection;20;0;13;0
WireConnection;20;1;16;0
WireConnection;14;0;12;0
WireConnection;14;1;10;0
WireConnection;19;0;13;1
WireConnection;19;1;17;0
WireConnection;31;0;26;0
WireConnection;29;0;14;0
WireConnection;25;0;20;0
WireConnection;25;1;19;0
WireConnection;32;0;28;0
WireConnection;33;0;31;1
WireConnection;33;1;31;2
WireConnection;33;2;31;3
WireConnection;34;0;29;0
WireConnection;35;0;25;0
WireConnection;37;0;32;0
WireConnection;37;1;33;0
WireConnection;42;0;39;0
WireConnection;44;1;38;0
WireConnection;41;0;40;0
WireConnection;43;0;37;0
WireConnection;48;0;43;0
WireConnection;48;1;42;0
WireConnection;47;0;44;1
WireConnection;47;1;44;3
WireConnection;47;2;41;0
WireConnection;51;0;47;0
WireConnection;51;1;46;0
WireConnection;49;0;48;0
WireConnection;49;1;45;0
WireConnection;54;0;51;0
WireConnection;54;1;50;0
WireConnection;53;0;49;0
WireConnection;153;0;141;0
WireConnection;154;0;153;0
WireConnection;55;0;54;0
WireConnection;56;0;53;0
WireConnection;56;1;52;0
WireConnection;59;0;55;0
WireConnection;57;0;56;0
WireConnection;156;0;154;0
WireConnection;155;0;156;0
WireConnection;67;0;61;0
WireConnection;67;1;63;0
WireConnection;151;0;155;0
WireConnection;65;0;60;0
WireConnection;65;1;62;0
WireConnection;150;0;78;0
WireConnection;150;1;152;0
WireConnection;150;2;151;4
WireConnection;70;0;65;0
WireConnection;70;1;67;0
WireConnection;76;0;70;0
WireConnection;80;2;71;0
WireConnection;80;3;72;0
WireConnection;127;0;150;0
WireConnection;81;0;80;0
WireConnection;81;1;127;0
WireConnection;82;0;76;0
WireConnection;86;1;75;0
WireConnection;86;2;82;0
WireConnection;87;0;67;0
WireConnection;87;1;81;0
WireConnection;88;0;87;0
WireConnection;89;0;86;0
WireConnection;145;0;141;0
WireConnection;142;0;141;0
WireConnection;158;0;157;0
WireConnection;137;0;94;0
WireConnection;137;1;136;0
WireConnection;149;0;141;0
WireConnection;143;1;92;0
WireConnection;143;2;142;4
WireConnection;144;1;91;0
WireConnection;144;2;145;4
WireConnection;159;0;158;0
WireConnection;160;0;115;0
WireConnection;160;1;159;0
WireConnection;148;0;93;0
WireConnection;148;1;147;0
WireConnection;148;2;149;4
WireConnection;95;0;137;0
WireConnection;95;1;143;0
WireConnection;139;0;144;0
WireConnection;139;1;138;0
WireConnection;135;0;83;0
WireConnection;135;1;134;0
WireConnection;116;0;121;0
WireConnection;116;1;117;0
WireConnection;103;0;102;0
WireConnection;106;1;103;0
WireConnection;107;0;105;0
WireConnection;107;1;106;0
WireConnection;113;0;116;0
WireConnection;99;0;96;40
WireConnection;132;0;131;0
WireConnection;132;1;148;0
WireConnection;132;2;133;0
WireConnection;96;20;97;0
WireConnection;96;110;98;0
WireConnection;125;0;130;0
WireConnection;125;1;95;0
WireConnection;125;2;126;0
WireConnection;123;1;139;0
WireConnection;123;2;124;0
WireConnection;111;0;107;0
WireConnection;111;1;108;0
WireConnection;112;0;110;0
WireConnection;112;1;109;3
WireConnection;112;3;111;0
WireConnection;122;0;110;0
WireConnection;122;1;109;3
WireConnection;122;2;111;0
WireConnection;121;0;112;0
WireConnection;121;1;122;0
WireConnection;117;0;121;0
WireConnection;120;0;160;0
WireConnection;120;1;135;0
WireConnection;120;2;119;0
WireConnection;105;0;104;1
WireConnection;105;1;104;3
WireConnection;0;0;120;0
WireConnection;0;2;123;0
WireConnection;0;3;125;0
WireConnection;0;4;132;0
ASEEND*/
//CHKSM=EF4F72BEE7C60C4870F063CD9FB011E18FA24710