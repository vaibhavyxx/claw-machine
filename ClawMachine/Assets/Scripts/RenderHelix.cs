using UnityEngine;
using System.Collections.Generic;
public class Helix : MonoBehaviour
{
    public LineRenderer _lineRenderer;

    public Vector3[] points;
    public float radius = 100.0f;
    public float coilWidth = 10.0f;
    public float width = 0.1f;

    //Previous values
    float prevWidth = 0.0f, prevRadius = 0.0f, prevOffset = 0.0f, prevTotal =0, prevSpeed = 0;
    public int total = 200;


    float theta = 0;
    float t = 0;
    float d = 0;

    public float speed = 1.0f;
    bool start = false;

    void Start()
    {
        points = new Vector3[total];
        _lineRenderer.positionCount = total;
        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;
        start = true;
    }

    void Update()
    {
        //Resets the
        if(prevWidth != width ||  prevRadius != radius || prevOffset!=coilWidth ||
            prevTotal != total || prevSpeed != speed)
            start = true;
        
        if (start)
        {
            t += Time.deltaTime;
            theta = t * speed;
            float z = this.transform.position.z;
            d += (speed * t);
            for (int i = 0; i < total; i++)
            {
                z += (coilWidth * t);
                Vector3 pt = new Vector3(
                this.transform.position.x + Mathf.Cos(theta * i),
                this.transform.position.y + Mathf.Sin(theta * i), z);
                points[i] = (pt);
            }
            _lineRenderer.SetPositions(points);
            start = false;
        }
        if (!start)
            t = 0.0f;
            

        prevOffset = coilWidth;
        prevRadius = radius;
        prevWidth = width;
        prevTotal = total;
        prevSpeed = speed;
    }
}
