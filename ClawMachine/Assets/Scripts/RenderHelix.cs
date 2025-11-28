using UnityEngine;
using System.Collections.Generic;

public enum Direction { X, Y, Z };

public class RenderHelix : MonoBehaviour
{
    [SerializeField] Transform startTransform;
    [SerializeField] Transform endTransform;
    public Vector3 direction;
    public LineRenderer _lineRenderer;

    public Vector3[] points;
    public float coilWidth = 10.0f;
    public float lineWidth = 0.1f;
    public int coilsCount = 0;
    //public Direction direction = Direction.X;
    public int totalPoints = 200;

    //Previous values
    float prevWidth = 0.0f, //prevRadius = 0.0f,
        prevCoilWidth = 0.0f, prevTotal = 0, prevLen = 0;

    float theta = 0.0f;
    float time = 0.0f;

    public float length = 1.0f;
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
        Vector3 difference = endTransform.position - startTransform.position;
        direction = difference.normalized;

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
        if (prevCoilWidth != coilWidth || prevLen != length)  //Speed prevents jittering
            start = true;

        if (start)
        {
            time += Time.deltaTime;
            theta = length * time;

            float z = this.transform.position.z;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Debug.Log("dir:" + direction);
            for (int i = 0; i < totalPoints; i++)
            {
                float deltaAngle = theta * i;
                z += (coilWidth * time);
                Vector3 pt = Vector3.zero;

                pt = new Vector3(
                        Mathf.Cos(deltaAngle),
                        Mathf.Sin(deltaAngle),
                        z);

                points[i] = rotation * pt + startTransform.position;
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
        prevLen = length;
    }
}
