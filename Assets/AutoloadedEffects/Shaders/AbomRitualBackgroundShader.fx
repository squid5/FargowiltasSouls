sampler diagonalNoise : register(s1);

float colorMult;
float time;
float radius;
float maxOpacity;

float2 screenPosition;
float2 screenSize;
float2 anchorPoint;
float2 playerPosition;

// This code has roots in the Providence arena shader in The Calamity Mod.
// I'm still learning the ropes of shader drawing, and like to have a reference point as a foundation.

float InverseLerp(float a, float b, float t)
{
    return saturate((t - a) / (b - a));
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
    float2 worldUV = screenPosition + screenSize * uv;
    float2 provUV = anchorPoint / screenSize;
    float worldDistance = distance(worldUV, anchorPoint);
    float adjustedTime = time * 0.1;
    
    // Pixelate the uvs
    float2 pixelatedUV = worldUV / screenSize;
    pixelatedUV.x -= worldUV.x % (1 / screenSize.x);
    pixelatedUV.y -= worldUV.y % (1 / (screenSize.y / 2) * 2);
    
    // Sample the noise textures
    float noiseMesh1 = tex2D(diagonalNoise, frac(pixelatedUV * 1.46 + float2(adjustedTime * 0.56, adjustedTime * 1.2))).g;
    float noiseMesh2 = tex2D(diagonalNoise, frac(pixelatedUV * 1.57 + float2(adjustedTime * -0.56, adjustedTime * 1.2))).g;
    float textureMesh = noiseMesh1 * 0.5 + noiseMesh2 * 0.5;
    
    float opacity = 1;
    
    // Define the border and mix the inferno for a smooth transition
    bool border = worldDistance < radius && opacity > 0;
    float colorMult = 1;
    if (border) 
        colorMult = 0;
    else
    {
        colorMult = InverseLerp(radius * 1.4, radius, worldDistance);
    }
    
    float blackOpacity = 0;
    float4 black = float4(0, 0, 0, 1);
    if (worldDistance < radius)
        blackOpacity = 1;
    
    blackOpacity = clamp(blackOpacity, 0, maxOpacity);
    
    return black * (1 - colorMult) * blackOpacity;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
