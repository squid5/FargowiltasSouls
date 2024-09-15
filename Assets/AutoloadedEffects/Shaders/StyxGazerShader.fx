sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);

float globalTime;
float3 mainColor;
bool fadeStart;

matrix uWorldViewProjection;

// These 3 are required if using primitives. They are the same for any shader being applied to them
// so you can copy paste them to any other prim shaders and use the VertexShaderOutput input in the
// PixelShaderFunction.
// -->
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float3 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float3 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    float4 pos = mul(input.Position, uWorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;

    return output;
}
// <--

// The X coordinate is the trail completion, the Y coordinate is the same as any other.
// This is simply how the primitive TextCoord is layed out in the C# code.
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // This can also be copy pasted along with the above.
    float4 color = input.Color;
    float2 coords = input.TextureCoordinates;
    coords.y = (coords.y - 0.5) / input.TextureCoordinates.z + 0.5;
    
    float texX = coords.x * 4;
    float4 tex1 = tex2D(uImage1, float2(frac(texX - globalTime * 2), coords.y));
    float4 tex2 = tex2D(uImage1, float2(frac(texX - globalTime * 2.64), coords.y + sin(coords.x * 68 - globalTime * 6.283) * 0.1));
    float4 tex3 = tex2D(uImage1, float2(frac(texX - globalTime * 5.12), coords.y));
    float4 fadeMapColor = tex1 * 0.4 + tex2 * 0.4 + tex3 * 0.4;
    
    // Use the red value for the opacity, as the provided image *should* be grayscale.
    float opacity = fadeMapColor.r;
    // Lerp between the base color, and the provided color based on the opacity of the fademap.
    float4 changedColor = lerp(float4(mainColor, 1), color, 0.1);
    float4 colorCorrected = lerp(color, changedColor, fadeMapColor.r);
    
    // Fade out at the top and bottom of the streak.
    float y = 0.5 - abs(coords.y - 0.5);
    if (y < 0.2)
        opacity *= pow(y / 0.2, 6);
    //if (coords.y < 0.2)
    //    opacity *= pow(coords.y / 0.2, 6);
    //if (coords.y > 0.8)
    //    opacity *= pow(1 - (coords.y - 0.8) / 0.8, 6);
    
    // Fade out at the end of the streak.
    if (fadeStart)
    {
        float startFade = 0.2;
        if (coords.x < startFade)
            opacity *= pow(coords.x / startFade, 2);
    }

    float endFade = 0.1;
    float endFader = 1;
    if (coords.x > endFade)
        endFader = pow(1 - (coords.x - endFade) / (1 - endFade), 2);
    if (endFader < 0.75)
        endFader = 0.75;
   
    return colorCorrected * opacity * endFader * 1.2;
}

technique Technique1
{
    pass AutoloadPass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
