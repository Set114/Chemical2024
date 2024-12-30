Shader "SoftKitty/LiquidMask"
{
   SubShader
   {
	   Tags{"RenderType" = "Qpaque"}
	   ZWrite Off

	   Stencil
   {
	   Ref 3
	   Comp Always
	   pass replace
   }
	   ColorMask 0
	   Pass{ }
   }
}
