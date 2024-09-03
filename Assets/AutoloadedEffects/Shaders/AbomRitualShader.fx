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
    float2 noiseUV = pixelatedUV - (anchorPoint / screenSize);
    float noiseMesh1 = tex2D(diagonalNoise, frac(noiseUV * 1.46 + float2(adjustedTime * 0.56, adjustedTime * 1.2))).g;
    float noiseMesh2 = tex2D(diagonalNoise, frac(noiseUV * 1.57 + float2(adjustedTime * -0.56, adjustedTime * 1.2))).g;
    float textureMesh = noiseMesh1 * 0.5 + noiseMesh2 * 0.5;
    
    // Get the distance to the pixel from the player.
    float distToPlayer = distance(playerPosition, worldUV);
    // And get the correct opacity based on it.
    float opacity = 0.25f;
    // Fade in quickly as the player approaches the pixels
    opacity += InverseLerp(900, 500, distToPlayer);
    
    // Define the border and fade
    bool border = worldDistance < radius && opacity > 0;
    float colorMult = 1;
    if (border) 
        colorMult = InverseLerp(radius * 0.97, radius, worldDistance);
    else
    {
        colorMult = InverseLerp(radius * 1.4, radius, worldDistance);
    }
        
    opacity = clamp(opacity, 0, maxOpacity);
    // If the color multi has not been changed (not border pixel) and opacity is 0 OR it's within 
    if (colorMult == 1 && (opacity == 0 || worldDistance < radius))
        return sampleColor;
    
    float4 darkColor = float4(0.67, 0.32, 0.12, 1);
    float4 midColor = float4(0.96, 0.60, 0.09, 1);
    float4 lightColor = float4(0.98, 0.95, 0.79, 1);
    
    float colorLerp = pow(colorMult, 3);
    //colorLerp = lerp(colorLerp, colorLerp * textureMesh + 0.3, 0.2);
    float4 color;
    float split = 0.6;
    if (colorLerp < split)
    {
        colorLerp = colorLerp / split;
        color = lerp(darkColor, midColor, colorLerp);
    }
    else
    {
        colorLerp = pow((colorLerp - split) / (1 - split), 3);
        color = lerp(midColor, lightColor, colorLerp);
    }
    color *= pow(abs(textureMesh), 0.03);

        
    return color * colorMult * opacity;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
