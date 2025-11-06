using UnityEngine;
using System.Collections.Generic;
public class Helix : MonoBehaviour
{
    public LineRenderer _lineRenderer;
    public GameObject sphere;
    public Vector3[] points;
    public float radius = 100.0f;
    public float offset = 10.0f;
    public float width = 0.1f;
    //Previous values
    float prevWidth = 0.0f, prevRadius = 0.0f, prevOffset = 0.0f;
    public int total = 200;
    //int count = 0;
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
        if(prevWidth != width ||  prevRadius != radius || prevOffset!=offset)
        {
            start = true;
        }
        if (start)
        {
            t += Time.deltaTime;
            theta = t * speed;
            float z = this.transform.position.z;
            d += (speed * t);
            for (int i = 0; i < total; i++)
            {
                z += (offset * t);
                Vector3 pt = new Vector3(
                this.transform.position.x + Mathf.Cos(theta * i),
                this.transform.position.y + Mathf.Sin(theta * i), z);
                points[i] = (pt);
                //Instantiate(sphere, pt, Quaternion.identity);
            }
            _lineRenderer.SetPositions(points);
            start = false;
        }

        prevOffset = offset;
        prevRadius = radius;
        prevWidth = width;
    }
}
