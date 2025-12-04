using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using JetBrains.Annotations;

public enum Direction { X, Y, Z };

public class RenderHelix : MonoBehaviour
{
    [Header("Control points")]
    [SerializeField] Transform startTransform;
    [SerializeField] Transform endTransform;
    [SerializeField] float coilAngle = Mathf.PI;

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

        UpdatePositionsArray();

        if (HasAnyPreviousPropertiesChange(difference)) start = true;
        

        if (start)
        {
            if (circumference > 0) findCircumference = true;
            _lineRenderer.SetPositions(UpdateHelixPoints(totalPoints));

            coilsCount = CountTotalCoils();
            start = false;
        }

        SaveLastFramesValues(coilWidth, lineWidth, totalPoints, thetaDelta, difference, sensitivity);
        coilWidth = distance / coilsCount;
    }

    void SaveLastFramesValues(float coilWidth, float lineWidth, int total, float delta, Vector3 diff,
        float sensitivity)
    {
        prevCoilWidth = coilWidth;
        prevWidth = lineWidth;
        prevTotal = total;
        prevDelta = delta;
        prevDiff = diff;
        prevSensitivity = sensitivity;
    }

    bool HasAnyPreviousPropertiesChange(Vector3 diff)
    {
        return (prevCoilWidth != coilWidth || prevDelta != thetaDelta ||
            prevDiff != diff || prevSensitivity != sensitivity);
    }

    int CountTotalCoils()
    {
        float finalAngle = theta * (totalPoints - 1);
        return Mathf.RoundToInt(finalAngle / (2f * Mathf.PI));
    }

    Vector3[] UpdateHelixPoints(int totalPoints)
    {
        theta = thetaDelta * sensitivity;
        float z = this.transform.position.z;
        float coilAngleDiff = coilAngle / totalPoints;

        for (int i = 0; i < totalPoints; i++)
        {
            float diff = i * coilAngleDiff;
            Vector3 endPoint = endTransform.position;

            Quaternion rotation = Quaternion.LookRotation(endPoint);
            float deltaAngle = theta * i;
            z += (coilWidth * sensitivity);
            Vector3 pt = Vector3.zero;

            pt = new Vector3(
                    Mathf.Cos(deltaAngle),
                    Mathf.Sin(deltaAngle),
                    z);

            points[i] = rotation * pt +
                startTransform.position;
            if (i > 0 && !findCircumference)
            {
                circumference += CalculateCircumference(points[i - 1], points[i]);
            }
        }
        return points;
    }

    float CalculateCircumference(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }

    void UpdatePositionsArray()
    {
        if (prevTotal != totalPoints)
        {
            points = new Vector3[totalPoints];
            _lineRenderer.positionCount = totalPoints;
            start = true;
        }
    }

}
