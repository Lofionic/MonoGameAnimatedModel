#define     SKINNED_EFFECT_MAX_BONES 72
float4x3    Bones[SKINNED_EFFECT_MAX_BONES]  _vs(c26) _cb(c22);

void Skin(inout VertexShaderInputSkinned vin, uniform int boneCount) {
    float4x3 skinning = 0;

    [unroll]
    for (int i = 0; i < boneCount; i++)
    {
        skinning += Bones[vin.BlendIndex[i]] * vin.BlendWeight[i];
    }

    vin.Position.xyz = mul(vin.Position, skinning);
    vin.Normal = mul(vin.Normal, (float3x3)skinning);
}
