Shader "Unlit/ZWriteOff"
{
   
    SubShader
    {
        Tags { "RenderType"="Opaque" }
      

        Pass
        {
          ZWrite Off
          ZTest Always
          ColorMask 0
        }
    }
}

