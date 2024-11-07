
Shader "SoftKitty/FlowMobile"
{
	Properties
	{
		_WaveTexture("Wave Texture", 2D) = "white" {}
		[HDR]_ColorTop("Color Top", Color) = (0.2249199,0.735849,0.3968853,1)
		[HDR]_ColorBottom("Color Bottom", Color) = (0.2235294,0.5833746,0.7372549,1)
		_TopOpacity("TopOpacity", Range( 0 , 1)) = 0
		_BottomOpacity("BottomOpacity", Range( 0 , 1)) = 1
		_MaskTexture("Mask Texture", 2D) = "white" {}
		_NormalTexture("Normal Texture", 2D) = "white" {}
		_Specular("Specular", Range( 0 , 1)) = 0.5
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.5
		_Speed("Speed", Range( 0 , 1)) = 0.5
		_Opacity("Opacity", Range( 0 , 1)) = 0.5
		_SpecularTexture("Specular Texture", 2D) = "white" {}
		_BlendLine("BlendLine", Range( 0 , 1)) = 1
		_ReflectionMap("ReflectionMap", CUBE) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		struct SurfaceOutputStandardSpecularCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Specular;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Transmission;
		};

		uniform sampler2D _NormalTexture;
		uniform float _Speed;
		uniform sampler2D _SpecularTexture;
		uniform sampler2D _WaveTexture;
		uniform samplerCUBE _ReflectionMap;
		uniform float4 _ColorTop;
		uniform float4 _ColorBottom;
		uniform float _BlendLine;
		uniform float _Specular;
		uniform float _Smoothness;
		uniform float _TopOpacity;
		uniform float _BottomOpacity;
		uniform sampler2D _MaskTexture;
		uniform float4 _MaskTexture_ST;
		uniform float _Opacity;

		inline half4 LightingStandardSpecularCustom(SurfaceOutputStandardSpecularCustom s, half3 viewDir, UnityGI gi )
		{
			half3 transmission = max(0 , -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
			half4 d = half4(s.Albedo * transmission , 0);

			SurfaceOutputStandardSpecular r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Specular = s.Specular;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandardSpecular (r, viewDir, gi) + d;
		}

		inline void LightingStandardSpecularCustom_GI(SurfaceOutputStandardSpecularCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardSpecularCustom o )
		{
			float mulTime2 = _Time.y * ( _Speed * -2.0 );
			float2 temp_cast_0 = (mulTime2).xx;
			float2 uv_TexCoord5 = i.uv_texcoord * float2( 2,1 ) + temp_cast_0;
			float2 uv_TexCoord44 = i.uv_texcoord * float2( 2,1 );
			float4 appendResult43 = (float4(uv_TexCoord5.x , uv_TexCoord44.y , 0.0 , 0.0));
			o.Normal = tex2D( _NormalTexture, appendResult43.xy ).rgb;
			float2 uv_TexCoord82 = i.uv_texcoord + float2( 0.13,0 );
			float4 color65 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float4 lerpResult64 = lerp( tex2D( _WaveTexture, appendResult43.xy ) , color65 , 0.95);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float4 lerpResult33 = lerp( _ColorTop , _ColorBottom , _BlendLine);
			o.Albedo = ( tex2D( _SpecularTexture, ( float4( uv_TexCoord82, 0.0 , 0.0 ) * lerpResult64 ).rg ) + ( ( ( texCUBE( _ReflectionMap, ( ase_screenPosNorm * lerpResult64 ).xyz ) + lerpResult33 ) / 2.0 ) + ( _ColorTop * 0.2 ) ) ).rgb;
			float3 temp_cast_9 = (_Specular).xxx;
			o.Specular = temp_cast_9;
			o.Smoothness = _Smoothness;
			o.Transmission = lerpResult33.rgb;
			float lerpResult40 = lerp( _TopOpacity , _BottomOpacity , _BlendLine);
			float2 uv_MaskTexture = i.uv_texcoord * _MaskTexture_ST.xy + _MaskTexture_ST.zw;
			float4 clampResult47 = clamp( ( lerpResult40 * ( tex2D( _MaskTexture, uv_MaskTexture ) * _Opacity * 10.0 ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			o.Alpha = clampResult47.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecularCustom alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

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
				float4 screenPos : TEXCOORD3;
				float4 tSpace0 : TEXCOORD4;
				float4 tSpace1 : TEXCOORD5;
				float4 tSpace2 : TEXCOORD6;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandardSpecularCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecularCustom, o )
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
