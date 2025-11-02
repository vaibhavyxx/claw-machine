using UnityEngine;
using System.Collections.Generic;
public class Helix : MonoBehaviour
{
    public LineRenderer _lineRenderer;
    public Vector3[] points;
    public float radius = 50.0f;
    public float widtth = 0.1f;
    public int total = 50;
    int count = 0;
    float t = 0;
    float theta = 0;
    public float speed = 1.0f;

    void Start()
    {
        points = new Vector3 [total];
        _lineRenderer.positionCount = total;
        _lineRenderer.startWidth = widtth;
        _lineRenderer.endWidth = widtth;
    }

    void Update()
    {
        theta += Time.deltaTime * speed;

   
        //for(int i = 0; i < total; i++) 
        if( count < total)
        {
            //if( < total)
            {
                Vector3 pt = new Vector3(
                Mathf.Cos(theta) * radius,
                count * radius,
                 Mathf.Sin(theta) * radius);
                points[count] = (pt);
            }
            count++;
        }
        _lineRenderer.SetPositions(points);
    }
}
