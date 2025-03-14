#ifndef CustomFunc
#define CustomFunc

struct Shape
{
    float3 position;
    float3 scale;
    float4 color;
    uint type;
};

StructuredBuffer<Shape> ShapesBuffer;
int ShapesCount;

void TestFunc_float(out float shapes)
{
#ifdef SHADERGRAPH_PREVIEW
    for (int i = 0; i < 3; i++)
    {
        shapes = i + 1;
    }
#else
    for (int i = 0; i < ShapesCount; i++)
    {
        shapes = i + 1;
    }
#endif
}
#endif  
float SignedDistanceFunction(float3 rayPos, float3 spherePos)
{
    float radius = 0.5;
    float dist = distance(rayPos, spherePos);
    return dist - radius; // if you do it wrong here, it renders only what would be outside the cube
}

float3 GetSphereNormal(float3 spherePos, float3 rayPos)
{
    return normalize(rayPos - spherePos);
}

void RayMarch_float(float3 objectPos, float3 rayDir, float3 fragPos, out float4 color)
{
    float3 spherePos = float3(0, 0, 0) + objectPos;
    float threshold = 0.0001f;
    float maxDist = 99999;
    float3 rayPos = fragPos;
    color = float4(1,1,1,0);
    for (int i = 0; i < 100; i++)
    {
        float dist = SignedDistanceFunction(rayPos, spherePos);
        if (abs(dist) < threshold)
        {
            color = float4(GetSphereNormal(rayPos.xyz, spherePos), 1);
            break;
        }
        rayPos = rayPos + rayDir * dist;
    }
}


