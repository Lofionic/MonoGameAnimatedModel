// SkinnedModelShader.fx

#include "Macros.fxh"

matrix      World;
matrix      View;
matrix      Projection;

float3      AmbientLightColor;
float       AmbientLightIntensity;

float3      DiffuseLightDirection;
float3      DiffuseLightColor;
float       DiffuseLightIntensity;

struct VertexShaderInputSkinned {
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float3 Normal : NORMAL0;
    float4 BlendWeight : BLENDWEIGHT0;
    uint4  BlendIndex : BLENDINDICES0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput {
    float4 Position : SV_Position;
    float4 Color : COLOR0;
    float3 Normal: TEXCOORD0;
    float2 TextureCoordinate : TEXCOORD2;
};

#include "Skinning.fxh"

VertexShaderOutput VS(VertexShaderInputSkinned input) {
    VertexShaderOutput output;

    Skin(input, 4);

    float4 world = mul(input.Position, World);
    float4 view = mul(world, View);
    output.Position = mul(view, Projection);

    output.Normal = mul(float4(input.Normal, 0), World).xyz;
    output.Color = input.Color;

    output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 PS(VertexShaderOutput input) : SV_TARGET {
    float3 ambient = saturate(AmbientLightIntensity * AmbientLightColor);

    float diffuseIntensity = saturate(dot(-DiffuseLightDirection, normalize(input.Normal)));    
    float3 diffuse = DiffuseLightColor * DiffuseLightIntensity * diffuseIntensity; 

    float4 color = float4(1.0, 1.0, 1.0, 1.0);
    color.xyz *= saturate(ambient + diffuse);

    return color;
}

TECHNIQUE(Render, VS, PS);
