Shader "SoftKitty/LiquidMobile"
{
	Properties
	{
		_SpecColor("Specular Color",Color)=(1,1,1,1)
		_Size("Size", Float) = 2.5
		_WaterLine("WaterLine", Range( 0 , 1)) = 0.2
		[HDR]_TopColor("TopColor", Color) = (0.8773585,0.8773585,0.8773585,1)
		[HDR]_BottomColor("BottomColor", Color) = (0.8773585,0.8773585,0.8773585,1)
		_ColorBlend("ColorBlend", Range( 0 , 1)) = 1
		_OpacityTop("OpacityTop", Range( 0 , 1)) = 0
		_OpacityBottom("OpacityBottom", Range( 0 , 1)) = 0
		[HDR]_EdgeColor("EdgeColor", Color) = (0,0,0,0)
		_EdgeFade("EdgeFade", Range( 0 , 1)) = 1
		_EdgeIntensity("EdgeIntensity", Range( 0 , 1)) = 0
		[HDR]_GlowColor("GlowColor", Color) = (0,0,0,0)
		_GlowIntensity("GlowIntensity", Range( 0 , 1)) = 0.5
		_WaveIntensity("WaveIntensity", Range( 0 , 2)) = 0.5
		_WaveTexture("WaveTexture", 2D) = "white" {}
		_WaveTile("WaveTile", Range(1 , 7)) = 3
		_Specular("Specular", Range( 0 , 1)) = 1
		_Gloss("Gloss", Range( 0 , 1)) = 0.04
		_GlossScale("GlossScale", Float) = 4
		_GlossTexture("GlossTexture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		_Stencil("Stencil",Int) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent-2" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha

		stencil
		{
		 Ref[_Stencil]
		 Comp LEqual
		 pass replace
		}
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float3 worldNormal;
			float3 viewDir;
		};

		uniform float _Size;
		uniform float _WaterLine;
		uniform sampler2D _GlossTexture;
		uniform sampler2D _WaveTexture;
		uniform float _WaveIntensity;
		uniform float4 _TopColor;
		uniform float4 _BottomColor;
		uniform float _ColorBlend;
		uniform float _GlossScale;
		uniform float _Gloss;
		uniform float4 _EdgeColor;
		uniform float _EdgeIntensity;
		uniform float _EdgeFade;
		uniform float _GlowIntensity;
		uniform float4 _GlowColor;
		uniform float _Specular;
		uniform float _OpacityTop;
		uniform float _OpacityBottom;
		uniform float _WaveTile;


		float2 voronoihash167( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi167( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash167( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			
			 		}
			 	}
			}
			return F1;
		}


		

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 transform87 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float temp_output_224_0 = ( _Size * 1.0 );
			float temp_output_227_0 = ( _WaterLine - 0.5 );
			float temp_output_201_0 = ( ( temp_output_224_0 * temp_output_227_0 ) + ( 1.0 - 1.0 ) );
			float mulTime156 = _Time.y * 0.1;
			float4 transform77 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float4 appendResult76 = (float4(transform77.x , transform77.z , 0.0 , 0.0));
			float4 tex2DNode154 = tex2Dlod( _GlossTexture, float4( ( ( sin( mulTime156 ) + appendResult76 ) * 0.7 ).xy, 0, 0.0) );
			float mulTime114 = _Time.y * 0.5;
			float4 appendResult112 = (float4(( sin( _Time.y ) * 1.0 ) , ( sin( mulTime114 ) * 0.2 ) , 0.0 , 0.0));
			float temp_output_155_0 = ( tex2DNode154.g + tex2Dlod( _WaveTexture, float4( ( appendResult76 + appendResult112 ).xy*_WaveTile, 0, 0.0) ).r );
			float ifLocalVar83 = 0;
			if( transform87.y > temp_output_201_0 )
				ifLocalVar83 = ( temp_output_201_0 + ( ( temp_output_155_0 - 0.5 ) * 0.03 * _WaveIntensity ) );
			else if( transform87.y == temp_output_201_0 )
				ifLocalVar83 = temp_output_201_0;
			else if( transform87.y < temp_output_201_0 )
				ifLocalVar83 = transform87.y;
			float4 appendResult88 = (float4(transform87.x , ifLocalVar83 , transform87.z , 0.0));
			float4 transform82 = mul(unity_WorldToObject,( appendResult88 - transform87 ));
			v.vertex.xyz += transform82.xyz;
			v.vertex.w = 1;
			float temp_output_130_0 = ( ( temp_output_155_0 - 0.5 ) * 0.5 );
			float4 appendResult128 = (float4(temp_output_130_0 , 1.0 , temp_output_130_0 , 0.0));
			float4 transform96 = mul(unity_WorldToObject,appendResult128);
			float3 ase_vertexNormal = v.normal.xyz;
			float4 ifLocalVar93 = 0;
			if( transform87.y >= temp_output_201_0 )
				ifLocalVar93 = transform96;
			else
				ifLocalVar93 = float4( ase_vertexNormal , 0.0 );
			v.normal = ifLocalVar93.xyz;
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float temp_output_224_0 = ( _Size * 1.0 );
			float temp_output_227_0 = ( _WaterLine - 0.5 );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 transform120 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float clampResult215 = clamp( ( ( ( ( temp_output_224_0 * temp_output_227_0 ) + 0.05 ) - transform120.y ) / temp_output_224_0 ) , 0.0 , 1.0 );
			float temp_output_169_0 = pow( clampResult215 , ( ( _ColorBlend * 5.0 ) + 0.1 ) );
			float clampResult117 = clamp( temp_output_169_0 , 0.0 , 1.0 );
			float4 lerpResult119 = lerp( _TopColor , _BottomColor , clampResult117);
			float4 transform87 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float temp_output_201_0 = ( ( temp_output_224_0 * temp_output_227_0 ) + ( 1.0 - 1.0 ) );
			float mulTime156 = _Time.y * 0.1;
			float4 transform77 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float4 appendResult76 = (float4(transform77.x , transform77.z , 0.0 , 0.0));
			float4 tex2DNode154 = tex2D( _GlossTexture, ( ( sin( mulTime156 ) + appendResult76 ) * 0.7 ).xy );
			float time167 = 0.0;
			float2 voronoiSmoothId0 = 0;
			float2 coords167 = i.uv_texcoord * ( _GlossScale * 50.0 );
			float2 id167 = 0;
			float2 uv167 = 0;
			float voroi167 = voronoi167( coords167, time167, id167, uv167, 0, voronoiSmoothId0 );
			float clampResult239 = clamp( ( pow( ( tex2DNode154.r * 6.0 * voroi167 ) , 10.0 ) * _Gloss ) , 0.0 , 1.0 );
			float ifLocalVar162 = 0;
			if( transform87.y >= temp_output_201_0 )
				ifLocalVar162 = clampResult239;
			else
				ifLocalVar162 = 0.0;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV123 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode123 = ( 0.0 + _EdgeIntensity * pow( 1.0 - fresnelNdotV123, ( 5.0 - ( _EdgeFade * 5.0 ) ) ) );
			float clampResult200 = clamp( fresnelNode123 , 0.0 , 0.7 );
			float4 lerpResult199 = lerp( ( lerpResult119 + ifLocalVar162 ) , _EdgeColor , clampResult200);
			o.Albedo = lerpResult199.rgb;
			float4 transform189 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float dotResult190 = dot( transform189 , float4( i.viewDir , 0.0 ) );
			float clampResult192 = clamp( pow( ( ( dotResult190 + 1.0 ) * 0.5 ) , ( 10.0 - ( 10.0 * _GlowIntensity ) ) ) , 0.0 , 1.0 );
			o.Emission = ( clampResult192 * _GlowColor ).rgb;
			o.Specular = _Specular;
			float clampResult170 = clamp( temp_output_169_0 , 0.0 , 1.0 );
			float lerpResult168 = lerp( _OpacityTop , _OpacityBottom , clampResult170);
			o.Alpha = lerpResult168;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf BlinnPhong keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
			sampler3D _DitherMaskLOD;
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
				vertexDataFunc( v );
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
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}