Shader "Temple Run/Terrain (Opaque)" {
Properties {
 _MainTex ("_MainTex", 2D) = "black" {}
 _Lightmap ("_Lightmap", 2D) = "black" {}
}
SubShader { 
 Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="True" "RenderType"="Opaque" }
 Pass {
  Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="True" "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord1", TexCoord1
  }
  SetTexture [_MainTex] { combine texture }
  SetTexture [_Lightmap] { combine previous * texture }
 }
}
Fallback "Diffuse"
}