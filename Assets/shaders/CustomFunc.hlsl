#ifndef CustomFunc
#define CustomFunc

struct Shape
{
    float3 position;
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