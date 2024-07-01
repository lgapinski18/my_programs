using System.Collections.Generic;
using UnityEngine;

public class PatrolArea
{
    private Vector3[] outerPoints;
    private Vector3[] middlePoints;
    private float outerIgnoreRadius;
    private float middleIgnoreRadius;

    private bool inOuter = false;

    Vector3 lastOuterPoint = Vector3.zero;
    Vector3 lastMiddlePoint = Vector3.zero;

    public PatrolArea(Vector3[] outerPoints, Vector3[] middlePoints, float outerIgnoreRadius, float middleIgnoreRadius)
    {
        this.outerPoints = outerPoints;
        this.middlePoints = middlePoints;
        this.outerIgnoreRadius = outerIgnoreRadius;
        this.middleIgnoreRadius = middleIgnoreRadius;
    }

    public PathfindingPath GetPath(Vector3 currentPosition)
    {
        Vector3 destination = Vector3.zero;
        if (inOuter)
        {
            List<Vector3> points = new List<Vector3>();

            foreach (Vector3 point in middlePoints) 
            { 
                if (!(Vector3.Distance(point, lastMiddlePoint) <= middleIgnoreRadius))
                {
                    points.Add(point);
                }    
            }

            if (points.Count == 0)
            {
                lastMiddlePoint = middlePoints[Random.Range(0, middlePoints.Length)];
            }
            else
            {
                lastMiddlePoint = points[Random.Range(0, points.Count)];
            }

            destination = lastMiddlePoint;
        }
        else
        {
            List<Vector3> points = new List<Vector3>();

            foreach (Vector3 point in outerPoints)
            {
                if (!(Vector3.Distance(point, lastOuterPoint) <= outerIgnoreRadius))
                {
                    points.Add(point);
                }
            }

            if (points.Count == 0)
            {
                lastOuterPoint = outerPoints[Random.Range(0, outerPoints.Length)];
            }
            else
            {
                lastOuterPoint = points[Random.Range(0, points.Count)];
            }

            destination = lastOuterPoint;
        }
        return PathfindingController.Instance.GetPath(currentPosition, destination);
    }
    public Vector3 GetDestination()
    {
        if (inOuter)
        {
            inOuter = false;
            return middlePoints[Random.Range(0, middlePoints.Length)];
        }
        else
        {
            inOuter = true;
            return outerPoints[Random.Range(0, outerPoints.Length)];
        }
    }

    public void DrawDebug()
    {
        foreach (Vector3 position in outerPoints)
        {
            Debug.DrawRay(position, new Vector3(1.1f, 0.0f, 0.0f), Color.green);
        }
        foreach (Vector3 position in middlePoints)
        {
            Debug.DrawRay(position, new Vector3(1.1f, 0.0f, 0.0f), Color.yellow);
        }
    }
}

