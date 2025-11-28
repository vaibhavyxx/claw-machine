using UnityEngine;
using System.Collections.Generic;

public enum Direction { X, Y, Z };

public class RenderHelix : MonoBehaviour
{
    public LineRenderer _lineRenderer;

    public Vector3[] points;
    //public float radius = 100.0f;
    public float coilWidth = 10.0f;
    public float lineWidth = 0.1f;
    public int coilsCount = 0;
    public Direction direction = Direction.X;

    //Previous values
    float prevWidth = 0.0f, prevRadius = 0.0f,
        prevCoilWidth = 0.0f,
        prevTotal = 0, prevSpeed = 0;
    public int totalPoints = 200;

    float theta = 0.0f;
    float time = 0.0f;

    public float speed = 1.0f;
    public Vector3 pos = Vector3.zero;
    bool start = false;

    void Start()
    {
        points = new Vector3[totalPoints];
        _lineRenderer.positionCount = totalPoints;
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
        pos = this.transform.position;
        start = true;
    }

    void Update()
    {
        //Updates line's size
        if (prevWidth != lineWidth)
            _lineRenderer.startWidth = _lineRenderer.endWidth = lineWidth;

        //Updates the total points in the linerenderer
        if (prevTotal != totalPoints)
        {
            points = new Vector3[totalPoints];
            _lineRenderer.positionCount = totalPoints;
            start = true;
        }
        //Resets the coil values
        if (prevCoilWidth != coilWidth || prevSpeed != speed)  //Speed prevents jittering
            start = true;

        if (start)
        {
            time += Time.deltaTime;
            theta = speed * time;
            float z = pos.z;

            for (int i = 0; i < totalPoints; i++)
            {
                float deltaAngle = theta * i;
                z += (coilWidth * time);
                Vector3 pt = Vector3.zero;
                switch (direction)
                {
                    case Direction.X:
                        pt = new Vector3(
                                z,
                                pos.x + Mathf.Cos(deltaAngle),
                                pos.y + Mathf.Sin(deltaAngle));
                        break;

                    case Direction.Y:
                        pt = new Vector3(
                                pos.x + Mathf.Cos(deltaAngle),
                                z,
                                pos.y + Mathf.Sin(deltaAngle));
                        break;

                    case Direction.Z:
                        pt = new Vector3(
                                pos.x + Mathf.Cos(deltaAngle),
                                pos.y + Mathf.Sin(deltaAngle),
                                z);
                        break;
                }


                points[i] = pt;
            }

            _lineRenderer.SetPositions(points);

            // --- Coil count calculation ---
            float finalAngle = theta * (totalPoints - 1);
            coilsCount = Mathf.RoundToInt(finalAngle / (2f * Mathf.PI));
            start = false;
        }
        else
            time = 0.0f;

        prevCoilWidth = coilWidth;
        prevWidth = lineWidth;
        prevTotal = totalPoints;
        prevSpeed = speed;

    }
}
