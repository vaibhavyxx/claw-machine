using JetBrains.Annotations;
using System.Collections.Generic;
using System.Net;
using Unity.Mathematics;
using UnityEngine;

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
    public float height = 1.0f;

    [Header("Coil's properites")]
    public float sensitivity = 0.1f;
    public float thetaDelta = 1.0f;
    public float coilWidth = 10.0f;

    //Previous values
    float prevWidth, prevSensitivity,prevCoilWidth, prevTotal, prevDistance, prevDelta, prevHeight = 0.0f;
    Vector3 prevDiff = Vector3.zero;

    float ogDist = 0.0f;
    float theta = 0.0f;
    float circumference = 0.0f;

    //Optimization purposes
    bool findCircumference = false;
    bool findDisplacement = false;

    void Start()
    {
        points = new Vector3[totalPoints];
        _lineRenderer.positionCount = totalPoints;
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
        //start = true;
    }

    void Update()
    {
        Vector3 difference = endTransform.position - startTransform.position;
        float distance = difference.magnitude;
        direction = difference.normalized;

        if (!HasAnyPreviousPropertiesChange(difference)) return;

        //Updates line's size
        if (prevWidth != lineWidth)
            _lineRenderer.startWidth = _lineRenderer.endWidth = lineWidth;

        //Height will change as previous distance changes
        if(prevDistance > 0)
        {
            if (distance - prevDistance >= 0.1f)
            {
                float k = distance / prevDistance;
                height = k * height;
            }
        }
        UpdatePositionsArray();
        {
            if (circumference > 0) findCircumference = true;
            _lineRenderer.SetPositions(CoiledArc(totalPoints, Mathf.PI, distance, height));

            coilsCount = CountTotalCoils();
            //start = false;
        }

        SaveLastFramesValues(coilWidth, lineWidth, totalPoints, thetaDelta, difference, sensitivity, height);
        coilWidth = distance / coilsCount;
    }

    void SaveLastFramesValues(float coilWidth, float lineWidth, int total, float delta, Vector3 diff,
        float sensitivity, float prevHeight)
    {
        prevCoilWidth = coilWidth;
        prevWidth = lineWidth;
        prevTotal = total;
        prevDelta = delta;
        prevDiff = diff;
        prevSensitivity = sensitivity;
        height = prevHeight;
    }

    bool HasAnyPreviousPropertiesChange(Vector3 diff)
    {
        return (prevCoilWidth != coilWidth || prevDelta != thetaDelta ||
            prevDiff != diff || prevSensitivity != sensitivity ||
            prevHeight != height);
    }

    int CountTotalCoils()
    {
        float finalAngle = theta * (totalPoints - 1);
        return Mathf.RoundToInt(finalAngle / (2f * Mathf.PI));
    }
    Vector3[] UpdateArc(int totalPoints, float angle, float distance, float height)
    {
        Vector3[] arcPoints = new Vector3[totalPoints];
        
        Vector3 start = startTransform.position;
        Vector3 end = endTransform.position;

        Vector3 dir = end - start;
        float length = dir.magnitude;

        //Testing to find height change over distance
        if(!findDisplacement) ogDist = length;
        findDisplacement = true;

        float deltaDistance = distance / totalPoints;
        float angleDiff = (1* angle) / totalPoints;
        Quaternion rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);

        for (int i = 0; i < totalPoints; i++)
        {
            Vector3 point = Vector3.zero;
            float t = i /(float)(totalPoints - 1);  //Gives a value between 0 and 1
            float x = t * length;
            float y = 4 * height * t * (1 - t);
            Vector3 local = new Vector3(0, y, x);
            arcPoints[i] = start + rotation * local;
        }
        return arcPoints;
    }

    Vector3[] CoiledArc(int totalPoints, float angle, float distance, float height)
    {
        theta = thetaDelta * sensitivity;
        //float z = this.transform.position.z;
        float coilAngleDiff = coilAngle / totalPoints;

        for (int i = 0; i < totalPoints; i++)
        {
            float t = i /(float)(totalPoints -1);
            float z = this.transform.position.z + t * distance; //testing
            float y = 4 * height * t * (1 - t);

            float diff = i * coilAngleDiff;
            Vector3 endPoint = endTransform.position;

            {
                Quaternion rotation = Quaternion.LookRotation(endPoint);
                float deltaAngle = theta * i;
                z += (coilWidth * sensitivity);
                Vector3 pt = Vector3.zero;

                pt = new Vector3(
                        Mathf.Cos(deltaAngle),
                        Mathf.Sin(deltaAngle) + y,
                        z);

                points[i] = rotation * pt + startTransform.position;

            }
            if (i > 0 && !findCircumference)
            {
                circumference += CalculateCircumference(points[i - 1], points[i]);
            }
        }
        return points;
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
            //start = true;
        }
    }

}
