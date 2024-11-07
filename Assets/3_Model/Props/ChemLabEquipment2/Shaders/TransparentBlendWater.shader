Shader "ChemLab/TransparentBlendWater"
{
	Properties
	{
		_Albedo("Albedo", Color) = (0,0,0,0)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+1" "IgnoreProjector" = "True" }
		Cull Back
		Blend DstColor One
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard addshadow fullforwardshadows 
		struct Input
		{
			half filler;
		};

		uniform float4 _Albedo;
		half _Glossiness;
        half _Metallic;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Albedo.rgb;
			o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			o.Alpha = _Albedo.a;
		}

		ENDCG
	}
	Fallback "Diffuse"
}