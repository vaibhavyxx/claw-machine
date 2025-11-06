using UnityEngine;
using System.Collections.Generic;
public class DrawHelix : MonoBehaviour
{
    public LineRenderer _lineRenderer;

    public Vector3[] points;
    //public float radius = 100.0f;
    public float coilWidth = 10.0f;
    public float lineWidth = 0.1f;
    public int coils = 0;

    //Previous values
    float prevWidth = 0.0f, prevRadius = 0.0f, prevOffset = 0.0f, prevTotal = 0, prevSpeed = 0;
    public int totalPoints = 200;

    float theta = 0;
    float t = 0;
    float d = 0;

    public float speed = 1.0f;
    bool start = false;

    void Start()
    {
        points = new Vector3[totalPoints];
        _lineRenderer.positionCount = totalPoints;
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
        start = true;
    }

    void Update()
    {
        //Updates line's size
        if (prevWidth != lineWidth)
            _lineRenderer.startWidth = _lineRenderer.endWidth = lineWidth;

        //Updates the total points in the linerenderer
        if(prevTotal != totalPoints)
        {
            points = new Vector3[totalPoints];
            _lineRenderer.positionCount = totalPoints;
            start = true;
        }
        //Resets the coil values
        if ( prevOffset != coilWidth || prevSpeed != speed)
            start = true;

        if (start)
        {
            t += Time.deltaTime;
            theta = speed * t;
            float z = this.transform.position.z;
            d += (speed * t);
            for (int i = 0; i < totalPoints; i++)
            {
                z += (coilWidth * t);
                Vector3 pt = new Vector3(
                this.transform.position.x + Mathf.Cos(theta * i),
                this.transform.position.y + Mathf.Sin(theta * i), z);
                points[i] = (pt);

                Debug.Log("theta: " + Mathf.Cos(theta * i));
            }
            _lineRenderer.SetPositions(points);
            start = false;
        }
        if (!start)
            t = 0.0f;

        prevOffset = coilWidth;
        //prevRadius = radius;
        prevWidth = lineWidth;
        prevTotal = totalPoints;
        prevSpeed = speed;
    }
}
