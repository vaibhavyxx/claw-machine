using UnityEngine;

public class LineDraw : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform endPoint;
    public Transform startPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, endPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(startPoint.position);
        //Debug.Log(endPoint.position);   
        // Update the positions of the line renderer to match the start and end points
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
