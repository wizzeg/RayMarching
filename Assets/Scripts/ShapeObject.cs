using UnityEngine;

public class ShapeObject : MonoBehaviour
{
    public Shape shape = new Shape();

    private void Start()
    {

    }

    private void Update()
    {
        if (shape.Position != transform.position)
        {
            shape.Position = transform.position;
        }
    }
}
