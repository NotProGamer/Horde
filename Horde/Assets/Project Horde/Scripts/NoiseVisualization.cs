using UnityEngine;
using System.Collections;

public class NoiseVisualization : MonoBehaviour
{
    public int segments;
    public float radius;
    public float increment;
    public float maxRadius;
    public float minRadius;
    private float nextIncrement = 0f;
    public float incrementDelay = 1f;

    LineRenderer line;


    private Tapper m_tapperScript = null;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        radius = minRadius;
    }

    void Update()
    {



        if (Time.time > nextIncrement)
        {

            increment = (maxRadius - minRadius) / 10;
            radius += increment;
            nextIncrement = Time.time + incrementDelay;
        }

        //clamps
        if (radius > maxRadius)
        {
            radius = minRadius;
        }
        CreatePoints();

    }


    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;

        float angle = 0f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle);
            y = Mathf.Cos(Mathf.Deg2Rad * angle);
            line.SetPosition(i, new Vector3(x, y, z) * radius);
            angle += (360f / segments);
        }
    }
}