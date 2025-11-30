using UnityEngine;
using System.Collections.Generic;

public enum Direction { X, Y, Z };

public class RenderHelix : MonoBehaviour
{
    [Header("Control points")]
    [SerializeField] Transform startTransform;
    [SerializeField] Transform endTransform;

    Vector3 direction;
    Vector3[] points;

    public LineRenderer _lineRenderer;

    [Header("Line Renderer's properties")]
    public float lineWidth = 0.1f;
    public int coilsCount = 0;  //Manage helix uses it
    public int totalPoints = 200;

    [Header("Coil's properites")]
    public float sensitivity = 0.1f;
    public float thetaDelta = 1.0f;
    float coilWidth = 10.0f;

    //Previous values
    float prevWidth = 0.0f, prevSensitivity = 0.0f,prevCoilWidth = 0.0f, prevTotal = 0, prevDelta = 0;
    Vector3 prevDiff = Vector3.zero;

    float theta = 0.0f;
    float circumference = 0.0f;

    //Optimization purposes
    bool start = false;
    bool findCircumference = false;

    void Start()
    {
        points = new Vector3[totalPoints];
        _lineRenderer.positionCount = totalPoints;
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
        //pos = this.transform.position;
        start = true;
    }

    void Update()
    {
        Vector3 difference = endTransform.position - startTransform.position;
        float distance = difference.magnitude;
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
        if (prevCoilWidth != coilWidth || prevDelta != thetaDelta ||
            prevDiff != difference || prevSensitivity != sensitivity)  //Speed prevents jittering
        {
            //circumference = 0.0f;
            start = true;
        }

        if (start)
        {
            theta = thetaDelta * sensitivity;
            float z = this.transform.position.z;
            Quaternion rotation = Quaternion.LookRotation(direction);

            for (int i = 0; i < totalPoints; i++)
            {
                float deltaAngle = theta * i;
                z += (coilWidth * sensitivity);
                Vector3 pt = Vector3.zero;

                pt = new Vector3(
                        Mathf.Cos(deltaAngle),
                        Mathf.Sin(deltaAngle),
                        z);

                points[i] = rotation * pt + startTransform.position;
                if (i > 0 && !findCircumference)
                {
                    float d = Vector3.Distance(points[i - 1], points[i]);
                    circumference += d;
                }
            }
            if (circumference > 0) findCircumference = true;
            _lineRenderer.SetPositions(points);

            // --- Coil count calculation ---
            float finalAngle = theta * (totalPoints - 1);
            coilsCount = Mathf.RoundToInt(finalAngle / (2f * Mathf.PI));
            start = false;
        }

        prevCoilWidth = coilWidth;
        prevWidth = lineWidth;
        prevTotal = totalPoints;
        prevDelta = thetaDelta;
        prevDiff = difference;
        prevSensitivity = sensitivity;
        coilWidth = distance / coilsCount;
    }
}
