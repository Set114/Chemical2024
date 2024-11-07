Shader "SoftKitty/Ponding"
{
	Properties
	{
		[HDR]_TopColor("TopColor", Color) = (0.1805447,0.6509434,0.2696517,1)
		[HDR]_BottomColor("BottomColor", Color) = (0.372549,0.5161874,0.7372549,1)
		_Specular("Specular", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_WaveSpeed("WaveSpeed", Range( 0 , 1)) = 0.2772513
		_WaveScale("WaveScale", Range( 0 , 1)) = 0.5
		_MaskTexture("MaskTexture", 2D) = "white" {}
		_WaveTexture("WaveTexture", 2D) = "white" {}
		_OpacityIntensity("OpacityIntensity", Range( 0 , 1)) = 1
		_OpacityRange("OpacityRange", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent-3" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv2_texcoord2;
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _MaskTexture;
		uniform float4 _MaskTexture_ST;
		uniform sampler2D _WaveTexture;
		uniform float _WaveSpeed;
		uniform float _WaveScale;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float4 _BottomColor;
		uniform float4 _TopColor;
		uniform float _Specular;
		uniform float _Smoothness;
		uniform float _OpacityRange;
		uniform float _OpacityIntensity;


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


		float3 PerturbNormal107_g7( float3 surf_pos, float3 surf_norm, float height, float scale )
		{
			// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
			float3 vSigmaS = ddx( surf_pos );
			float3 vSigmaT = ddy( surf_pos );
			float3 vN = surf_norm;
			float3 vR1 = cross( vSigmaT , vN );
			float3 vR2 = cross( vN , vSigmaS );
			float fDet = dot( vSigmaS , vR1 );
			float dBs = ddx( height );
			float dBt = ddy( height );
			float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
			return normalize ( abs( fDet ) * vN - vSurfGrad );
		}


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		

		void vertexDataFunc( inout appdata_full v )
		{
			float2 uv2_MaskTexture = v.texcoord1 * _MaskTexture_ST.xy + _MaskTexture_ST.zw;
			float4 tex2DNode161 = tex2Dlod( _MaskTexture, float4( uv2_MaskTexture, 0, 0.0) );
			float mulTime135 = _Time.y * 0.5;
			float2 temp_cast_0 = (( mulTime135 * _WaveSpeed )).xx;
			float2 uv_TexCoord123 = v.texcoord.xy + temp_cast_0;
			float4 tex2DNode169 = tex2Dlod( _WaveTexture, float4( uv_TexCoord123, 0, 0.0) );
			float simplePerlin2D67 = snoise( uv_TexCoord123*( 3.0 * _WaveScale ) );
			simplePerlin2D67 = simplePerlin2D67*0.5 + 0.5;
			float clampResult69 = clamp( simplePerlin2D67 , 0.0 , 1.0 );
			float clampResult180 = clamp( pow( clampResult69 , 4.32 ) , 0.0 , 1.0 );
			float3 temp_cast_1 = (clampResult180).xxx;
			float temp_output_2_0_g6 = 0.7;
			float temp_output_3_0_g6 = ( 1.0 - temp_output_2_0_g6 );
			float3 appendResult7_g6 = (float3(temp_output_3_0_g6 , temp_output_3_0_g6 , temp_output_3_0_g6));
			float3 temp_output_166_0 = ( tex2DNode161.r * ( pow( tex2DNode169.r , 4.51 ) * float3( 0.1,0,0 ) * ( ( temp_cast_1 * temp_output_2_0_g6 ) + appendResult7_g6 ) ) );
			float3 temp_cast_2 = (0.03).xxx;
			float3 clampResult165 = clamp( ( ( temp_output_166_0 + ( ( 1.0 - tex2DNode161.r ) * 0.05 ) ) - temp_cast_2 ) , float3( 0,0,0 ) , float3( 1,1,1 ) );
			float3 appendResult148 = (float3(0.0 , ( clampResult165 * 0.2 ).x , 0.0));
			v.vertex.xyz += appendResult148;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 surf_pos107_g7 = ase_worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 surf_norm107_g7 = ase_worldNormal;
			float2 uv2_MaskTexture = i.uv2_texcoord2 * _MaskTexture_ST.xy + _MaskTexture_ST.zw;
			float4 tex2DNode161 = tex2D( _MaskTexture, uv2_MaskTexture );
			float mulTime135 = _Time.y * 0.5;
			float2 temp_cast_0 = (( mulTime135 * _WaveSpeed )).xx;
			float2 uv_TexCoord123 = i.uv_texcoord + temp_cast_0;
			float4 tex2DNode169 = tex2D( _WaveTexture, uv_TexCoord123 );
			float simplePerlin2D67 = snoise( uv_TexCoord123*( 3.0 * _WaveScale ) );
			simplePerlin2D67 = simplePerlin2D67*0.5 + 0.5;
			float clampResult69 = clamp( simplePerlin2D67 , 0.0 , 1.0 );
			float clampResult180 = clamp( pow( clampResult69 , 4.32 ) , 0.0 , 1.0 );
			float3 temp_cast_1 = (clampResult180).xxx;
			float temp_output_2_0_g6 = 0.7;
			float temp_output_3_0_g6 = ( 1.0 - temp_output_2_0_g6 );
			float3 appendResult7_g6 = (float3(temp_output_3_0_g6 , temp_output_3_0_g6 , temp_output_3_0_g6));
			float3 temp_output_166_0 = ( tex2DNode161.r * ( pow( tex2DNode169.r , 4.51 ) * float3( 0.1,0,0 ) * ( ( temp_cast_1 * temp_output_2_0_g6 ) + appendResult7_g6 ) ) );
			float height107_g7 = temp_output_166_0.x;
			float scale107_g7 = 3.0;
			float3 localPerturbNormal107_g7 = PerturbNormal107_g7( surf_pos107_g7 , surf_norm107_g7 , height107_g7 , scale107_g7 );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir42_g7 = mul( ase_worldToTangent, localPerturbNormal107_g7);
			o.Normal = worldToTangentDir42_g7;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float3 temp_cast_3 = (0.03).xxx;
			float3 clampResult165 = clamp( ( ( temp_output_166_0 + ( ( 1.0 - tex2DNode161.r ) * 0.05 ) ) - temp_cast_3 ) , float3( 0,0,0 ) , float3( 1,1,1 ) );
			float clampResult238 = clamp( pow( ( clampResult165.x + 0.94 ) , 17.5 ) , 0.0 , 1.0 );
			float lerpResult236 = lerp( clampResult238 , 1.0 , 0.9);
			float4 screenColor184 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ( ase_grabScreenPosNorm * lerpResult236 ) + ( ( 1.0 - lerpResult236 ) * 0.05 ) ).xy);
			float4 lerpResult243 = lerp( _BottomColor , _TopColor , float4( 0.2264151,0.2264151,0.2264151,1 ));
			float4 lerpResult197 = lerp( lerpResult243 , _BottomColor , tex2DNode169.r);
			o.Albedo = ( ( pow( screenColor184 , 14.22 ) * float4( 0.6981132,0.6981132,0.6981132,1 ) ) + lerpResult197 ).rgb;
			float3 temp_cast_6 = (_Specular).xxx;
			o.Specular = temp_cast_6;
			o.Smoothness = _Smoothness;
			float dotResult199 = dot( float3(0,1,0) , ase_worldNormal );
			float clampResult194 = clamp( ( pow( dotResult199 , ( 50.0 * _OpacityRange ) ) * _OpacityIntensity ) , 0.0 , 1.0 );
			o.Alpha = clampResult194;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				float4 customPack1 : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv2_texcoord2;
				o.customPack1.xy = v.texcoord1;
				o.customPack1.zw = customInputData.uv_texcoord;
				o.customPack1.zw = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.uv2_texcoord2 = IN.customPack1.xy;
				surfIN.uv_texcoord = IN.customPack1.zw;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
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