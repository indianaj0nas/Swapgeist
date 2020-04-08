Shader "Sprites/ScreenSpacePattern"
{
Properties
{
[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
_Color ("Main Color", Color) = (1,1,1,1)
_PatternTex ("Pattern", 2D) = "white" {}
_PatternColor ("Pattern Color", Color) = (1,1,1,1)
_ScrollSpeedX("Scroll Speed X",float) = 0
    _ScrollSpeedY("Scroll Speed Y",float) = 0
}

SubShader
{
Tags
{ 
"Queue"="Transparent" 
"IgnoreProjector"="True" 
"RenderType"="Transparent" 
"PreviewType"="Plane"
"CanUseSpriteAtlas"="True"
}

Cull Off
Lighting Off
ZWrite Off
Blend SrcAlpha OneMinusSrcAlpha

Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile DUMMY PIXELSNAP_ON
#include "UnityCG.cginc"

struct appdata_t {

float4 vertex : POSITION;
float4 color : COLOR;
float2 texcoord: TEXCOORD0;
};

struct v2f {

float4 vertex : SV_POSITION;
fixed4 color : COLOR;
half2 texcoord : TEXCOORD0;
float4 screenpos : TEXCOORD1;
};

fixed4 _Color;

v2f vert (appdata_t IN) {

v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color * _Color;
OUT.screenpos = ComputeScreenPos(OUT.vertex);

return OUT;
}

sampler2D _MainTex;
uniform float4 _MainTex_ST;
sampler2D _PatternTex;
uniform float4 _PatternTex_ST;
float _ScrollSpeedX;
float _ScrollSpeedY;
fixed4 _PatternColor;

fixed4 frag(v2f IN) : COLOR {

float2 screenUV = IN.screenpos.xy / IN.screenpos.w;
screenUV *= _PatternTex_ST.xy + _PatternTex_ST.zw;
screenUV.x /= _ScreenParams.y / 1000;
screenUV.y /= _ScreenParams.x / 1000;

// Pattern scrolling (X axis only)
fixed2 scrollingSpeedA = float2(_ScrollSpeedX,0);
screenUV.xy += (scrollingSpeedA * _Time);

// Main texture scrolling (Y axis only)
fixed2 scrollingSpeedB = float2(0,_ScrollSpeedY);
IN.texcoord.xy += (scrollingSpeedB * _Time);

fixed4 c = (tex2D(_MainTex, IN.texcoord) * IN.color) + (tex2D(_PatternTex,screenUV) * tex2D(_MainTex, IN.texcoord).a * _PatternColor) - (tex2D(_PatternTex,screenUV) * _Color);
return c;
}

ENDCG

}
}
}