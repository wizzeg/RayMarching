using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;


public class RayMarchingShapesInjector : MonoBehaviour
{
    public Material RayMarchMaterial;
    public ComputeBuffer ShapesBuffer;
    public int ComputeBufferLength = 1;
    
    [SerializeField]
    public List<Shape> Shapes = new();
    public List<ShapeObject> ShapeObjects = new();
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

        foreach (var shapeObject in ShapeObjects)
        {
            Shapes.Add(shapeObject.shape);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Shapes.Count != oldCount)
        {
            Debug.Log("Count is: " + Shapes.Count);
            AdjustComputeBufferLength(Shapes.Count, ref ShapesBuffer, ref ComputeBufferLength, Marshal.SizeOf<Shape>());
            oldCount = Shapes.Count;
        }

        ShapesBuffer.SetData(Shapes.ToArray());
        RayMarchMaterial.SetBuffer("ShapesBuffer", ShapesBuffer);
        RayMarchMaterial.SetInt("ShapesCount", Shapes.Count);
    }

    private void AdjustComputeBufferLength(int count, ref ComputeBuffer buffer, ref int length, int size )
    {
        if (length >= 1 && count > length)
        {
            buffer.Dispose();
            buffer.Release();
            buffer = null;

            Debug.Log("Doubling buffer count");
            length = length * 2;
            buffer = new ComputeBuffer(length, size);
        }
        else if (length > 1 && length / count > 2)
        {
            buffer.Dispose();
            buffer.Release();
            buffer = null;

            Debug.Log("Halving buffer count");
            length = length / 2;
            buffer = new ComputeBuffer(length, size);
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
