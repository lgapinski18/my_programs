using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using UnityEngine;

public class PathfindingPath
{
    private Stack<Vector3> pathStack;

    private Vector3 lastReturned;

    public int Count { get =>  pathStack.Count; }

    public PathfindingPath(Vector3[] path)
    {
        path.Reverse();
        pathStack = new Stack<Vector3>(path);
    }

    public Vector3 NextNode()
    {
        if (pathStack.Count > 0)
        {
            lastReturned = pathStack.Pop();
        }
        return lastReturned;
    }

    public bool AchievedEnd()
    {
        return pathStack.Count == 0;
    }

    public PathfindingPath GetSubPath(int startIndex, int numberOfPathNodes)
    {
        Vector3[] pathCopy = pathStack.ToArray();
        return new PathfindingPath(pathCopy[startIndex..(startIndex + numberOfPathNodes)]);
    }

    static public PathfindingPath operator+(PathfindingPath one, PathfindingPath other)
    {
        Vector3[] pathCopy = one.pathStack.ToArray();
        Vector3[] pathCopy2 = other.pathStack.ToArray();
        pathCopy.AddRange(pathCopy2);

        return new PathfindingPath(pathCopy);
    }

    public PathfindingPath CombinePath(PathfindingPath path)
    {
        PathfindingPath joinedPath = this + path;
        pathStack = joinedPath.pathStack;

        return this;
    }

    public Vector3 Destination()
    {
        if (pathStack.Count > 0)
        {
            return pathStack.Last<Vector3>();
        }
        else
        {
            return lastReturned;
        }
    }
}
