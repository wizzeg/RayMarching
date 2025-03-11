using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

public enum ShapeType
{
    Sphere,
    Cube,
    Donut
}
public class RayMarchingShapesInjector : MonoBehaviour
{
    public Material RayMarchMaterial;
    public ComputeBuffer ShapesBuffer;
    public int ComputeBufferLength = 1;
    [System.Serializable]
    public struct Shape
    {
        public Vector3 position;
        public Vector4 color;
        public ShapeType type;
    }
    [SerializeField]
    public List<Shape> Shapes = new();
    private int oldCount;

    private bool newShape = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (RayMarchMaterial == null)
        { 
            enabled = false;
        }
        if (ComputeBufferLength > 0)
        {
            ShapesBuffer = new ComputeBuffer(ComputeBufferLength, Marshal.SizeOf<Shape>());
        }
        oldCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Shapes.Count != oldCount)
        {
            if (ComputeBufferLength >= 1 && Shapes.Count > ComputeBufferLength)
            {
                ShapesBuffer.Dispose();
                ShapesBuffer.Release();
                ShapesBuffer = null;

                Debug.Log("Doubling buffer count");
                ComputeBufferLength = ComputeBufferLength * 2;
                ShapesBuffer = new ComputeBuffer(ComputeBufferLength, Marshal.SizeOf<Shape>());
            }
            else if (ComputeBufferLength > 1 && ComputeBufferLength / Shapes.Count > 2)
            {
                ShapesBuffer.Dispose();
                ShapesBuffer.Release();
                ShapesBuffer = null;

                Debug.Log("Halving buffer count");
                ComputeBufferLength = ComputeBufferLength / 2;
                ShapesBuffer = new ComputeBuffer(ComputeBufferLength, Marshal.SizeOf<Shape>());
            }
            Debug.Log("Count is: " + Shapes.Count);
            ShapesBuffer.SetData(Shapes.ToArray());  
            RayMarchMaterial.SetBuffer("ShapesBuffer", ShapesBuffer);
            RayMarchMaterial.SetInt("ShapesCount", Shapes.Count);
            oldCount = Shapes.Count;
        }
        
    }

    private void OnDisable()
    {
        Debug.Log("Disabled");
        ShapesBuffer.Release();
    }

    void OnDestroy()
    {
        Debug.Log("Destroyed");
        ShapesBuffer.Release();
        ShapesBuffer = null;
    }
}
