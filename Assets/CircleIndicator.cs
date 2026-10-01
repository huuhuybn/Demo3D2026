using UnityEngine;

public class CircleIndicator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float radius = 3f;
    public int segments = 64;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.useWorldSpace = false;
        line.loop = true;

        line.startWidth = 0.03f;
        line.endWidth = 0.03f;

        DrawCircle();
    }

    void DrawCircle()
    {
        line.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            line.SetPosition(i, new Vector3(x, 0.02f, z));
        }
    }
}
