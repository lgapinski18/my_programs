//#define DEBUG_PATHNODE_CONECTIONS
//#define DEBUG_PATHNODE_CONECTIONS_ADDITIONAL

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingController : MonoBehaviour
{
    private class AStarNode
    {
        private PathfindingGraphNode node;
        private AStarNode root;
        private float costFromStart;
        private float heuristicCost;

        public AStarNode(PathfindingGraphNode node, AStarNode root, float costFromStart, float heuristicCost)
        {
            this.node = node;
            this.root = root;
            this.costFromStart = costFromStart;
            this.heuristicCost = heuristicCost;
        }

        public PathfindingGraphNode Node { get => node; }
        public AStarNode Root { get => root; }
        public float CostFromStart { get => costFromStart; }
        public float HeuristicCost { get => heuristicCost; }
    }


    private static PathfindingController instance = null;
    public static PathfindingController Instance { get => instance; } //set => instance = value; 

    private PathfindingGraphNode[] graphNodes = null;

    [SerializeField]
    private float maximalLinkingDistance = 1.5f;

    [SerializeField]
    private LayerMask[] ViewBlockingLayers;
    private LayerMask resultViewBlockingMask;
    public LayerMask ResultViewBlockingMask { get => resultViewBlockingMask; }

    [Header("Patrol Area"), Space(10)]
    [SerializeField]
    float maxDistance = 10.0f;
    [SerializeField]
    int hullParts = 5;
    [SerializeField]
    float middleFactor = 0.4f;

    [Header("Auto Mapping")]
    public bool AutoMappingEnable = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }


        foreach (LayerMask layer in ViewBlockingLayers)
        {
            resultViewBlockingMask |= layer;
        }

        if (AutoMappingEnable)
        {
            MapNodes();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void MapNodes()
    {
        PathfindingNode[] pathfindingNodes = Object.FindObjectsByType<PathfindingNode>(FindObjectsSortMode.None);
        
        graphNodes = new PathfindingGraphNode[pathfindingNodes.Length];

        for (int i = 0; i < pathfindingNodes.Length; i++)
        {
            graphNodes[i] = new PathfindingGraphNode(pathfindingNodes[i]);
        }

        for (int i = 0; i < graphNodes.Length; i++) 
        { 
            for (int j = i + 1; j < graphNodes.Length; j++)
            {
                if (Vector3.Distance(graphNodes[i].Value.position, graphNodes[j].Value.position) <= maximalLinkingDistance)
                {
                    Vector3 direction = graphNodes[j].Value.position - graphNodes[i].Value.position;

                    float directionFactor = 0.8f;

                    Vector2 directionH = direction; 
                    Vector2 directionV = direction; 
                    directionH.y *= directionFactor;
                    directionV.x *= directionFactor;
                    Quaternion quaternionLeft = Quaternion.Euler(new Vector3(0.0f, 0.0f, 5.0f));
                    Quaternion quaternionRight = Quaternion.Euler(new Vector3(0.0f, 0.0f, -5.0f));

                    Vector3 leftRotated = quaternionLeft * direction;
                    Vector3 rightRotated = quaternionRight * direction;

                    bool horizontalFree = true;
                    bool verticalFree = true;

                    Vector2 origin = graphNodes[i].Value.position;

                    //if (directionH.x != 0.0f)
                    {
                        //RaycastHit2D raycastResult2DH = Physics2D.Raycast(origin, directionH, 1.0f, resultViewBlockingMask, -0.1f, 0.1f);//
                        //RaycastHit2D raycastResult2DH = Physics2D.Raycast(origin, directionH, maximalLinkingDistance, resultViewBlockingMask, -0.1f, 0.1f);

                        RaycastHit2D raycastResult2DH = Physics2D.Raycast(origin, leftRotated.normalized, maximalLinkingDistance, resultViewBlockingMask);//

                        horizontalFree = raycastResult2DH.collider == null;
                    }

                    //if (directionV.y != 0.0f)
                    {
                        //RaycastHit2D raycastResult2DV = Physics2D.Raycast(origin, directionV, 1.0f, resultViewBlockingMask, -0.1f, 0.1f);//
                        //RaycastHit2D raycastResult2DV = Physics2D.Raycast(origin, directionV, maximalLinkingDistance, resultViewBlockingMask, -0.1f, 0.1f);//

                        RaycastHit2D raycastResult2DV = Physics2D.Raycast(origin, rightRotated.normalized, maximalLinkingDistance, resultViewBlockingMask);//

                        verticalFree = raycastResult2DV.collider == null;
                    }

                    if (horizontalFree && verticalFree)
                    {
                        //graphNodes[i].AddLinkedNode(graphNodes[j], 1);
                        graphNodes[i].AddLinkedNode(graphNodes[j], direction.magnitude);

#if DEBUG_PATHNODE_CONECTIONS
                        Debug.DrawRay(origin, direction, Color.green, 100.0f);
#endif
                    }
#if DEBUG_PATHNODE_CONECTIONS_ADDITIONAL
                    //*
                    else
                    {
                        Debug.DrawRay(origin, direction, Color.red, 100.0f);
                    }
                    //Debug.DrawRay(origin, directionV.normalized * maximalLinkingDistance, Color.blue, 100.0f);

                    Debug.DrawRay(origin, leftRotated.normalized * maximalLinkingDistance, Color.blue, 100.0f);
                    if (!horizontalFree)
                    {
                        Debug.DrawRay(origin, leftRotated.normalized * maximalLinkingDistance, Color.magenta, 100.0f);
                    }
                    //Debug.DrawRay(origin, directionH.normalized * maximalLinkingDistance, Color.yellow, 100.0f);
                    Debug.DrawRay(origin, rightRotated.normalized * maximalLinkingDistance, Color.yellow, 100.0f);
                    if (!verticalFree)
                    {
                        Debug.DrawRay(origin, rightRotated.normalized * maximalLinkingDistance, Color.cyan, 100.0f);
                    }/**/
#endif
                }
            }
        }

        //Debug.Log("Ended generating graph");
    }

    private float ChebyshevDistance(Vector3 vector1, Vector3 vector2)
    {
        Vector3 delta = vector1 - vector2;
        return Mathf.Max(Mathf.Abs(delta.x), Mathf.Max(Mathf.Abs(delta.y), Mathf.Abs(delta.z)));
    }

    private float ManhattanDistance(Vector3 vector1, Vector3 vector2)
    {
        Vector3 delta = vector1 - vector2;
        return Mathf.Abs(delta.x) + Mathf.Abs(delta.y) + Mathf.Abs(delta.z);
    }

    private float CalculateHeuristicDistance(Vector3 vector1, Vector3 vector2)
    {
        //return ChebyshevDistance(vector1, vector2);
        return Vector3.Distance(vector1, vector2);
    }

    public PathfindingPath GetPath(Vector3 start, Vector3 destination)
    {
        //Znajdowanie wêz³a pocz¹tkowego
        PathfindingGraphNode closestToStart = GetClosestGraphNode(start);

        //Znajdowanie wêz³a koñcowego
        PathfindingGraphNode closestToDestination = GetClosestGraphNode(destination);

        //PRZYGOTOWANIA DO ALGORYTMU ASTAR
        //Tworzenie kolejki priorytetowej
        PriorityQueue<AStarNode, float> priorityQueue = new PriorityQueue<AStarNode, float>((float priority1, float priority2) => { return priority1 < priority2; });

        //Tworzenie wêz³a Ÿród³owego algorytmu ASTAR
        AStarNode rootOrigin = new AStarNode(closestToStart, null, 0.0f, 
                                            CalculateHeuristicDistance(closestToStart.Value.position, closestToDestination.Value.position));
         
        priorityQueue.Enqueue(rootOrigin, rootOrigin.CostFromStart + rootOrigin.HeuristicCost);

        AStarNode tempAStarNode = null;

        AStarNode result = null;

        bool continueLoop = true;

        //ALGORYTM ASTAR

        while (continueLoop && priorityQueue.Count > 0)
        {
            AStarNode processedNode = priorityQueue.Dequeue();
            //Debug.Log("Loop");
            if (processedNode.Node == closestToDestination)
            {
                //Debug.Log("Closest to destination found");
                result = processedNode;
                continueLoop = false;
                break;
            }
            //else
            for (int i = 0; i < processedNode.Node.LinkedCount; i++)
            {
                //Debug.Log("Processing Linked");

                if (!processedNode.Node[i].Visited)
                {
                    //Debug.Log("Visiting");
                    processedNode.Node[i].Visited = true;

                    tempAStarNode = new AStarNode(processedNode.Node[i], processedNode, processedNode.CostFromStart + processedNode.Node.Edge(i).Weight,
                        CalculateHeuristicDistance(processedNode.Node[i].Value.position, closestToDestination.Value.position));

                    priorityQueue.Enqueue(tempAStarNode, tempAStarNode.CostFromStart + tempAStarNode.HeuristicCost);
                }
            }
        }

        foreach (PathfindingGraphNode node in graphNodes)
        {
            node.Visited = false;
        }
        //Debug.Log("Ending AStar");

        //Weryfikacja czy algorytm odnalaz³ œcie¿kê
        //Debug.Log("Result: " + (result == null));
        PathfindingPath returnedPath = null;
        if (result != null)
        {
            //Odtwarzanie œcie¿ki na podstawie rezultatu z algorytmu AStar
            List<Vector3> pathList = new List<Vector3>();
            pathList.Add(destination);

            AStarNode processedResultNode = result;

            while (processedResultNode != null)
            {
                pathList.Add(processedResultNode.Node.Value.position);
                processedResultNode = processedResultNode.Root;
            }


            //pathList.Add(closestToStart.Value.transform.position);
            pathList.Add(start);


            //if (Vector3.Distance(pathList[1], pathList[2]) > Vector3.Distance(pathList[0], pathList[2]))
            //{
            //    pathList.RemoveAt(1);
            //}
            while (pathList.Count >= 3 && ChebyshevDistance(pathList[1], pathList[2]) > ChebyshevDistance(pathList[0], pathList[2]))
            {
                //Debug.Log("1PF rm: " + pathList.Count);
                pathList.RemoveAt(1);
                //Debug.Log("2PF rm: " + pathList.Count);
            }

            pathList.Reverse();

            while (pathList.Count >= 3 && ChebyshevDistance(pathList[1], pathList[2]) > ChebyshevDistance(pathList[0], pathList[2]))
            {
                //Debug.Log("1PF rm2: " + pathList.Count);
                pathList.RemoveAt(1);
                //Debug.Log("2PF rm2: " + pathList.Count);
            }

            pathList.Reverse();

            returnedPath = new PathfindingPath(pathList.ToArray());
        }
        else
        {
            returnedPath = new PathfindingPath(new Vector3[] { start });
            returnedPath.NextNode();
        }

        return returnedPath;
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 returnedPosition = graphNodes[Random.Range(0, graphNodes.Length)].Value.position;

        return returnedPosition;
    }

    private PathfindingGraphNode GetClosestGraphNode(Vector3 position)
    {
        PathfindingGraphNode closestNode = graphNodes[0];

        float minDistance = Vector3.Distance(closestNode.Value.position, position);
        float tempMinDist = 0.0f;
        for (int i = 1; i < graphNodes.Length; i++)
        {
            if (graphNodes[i].LinkedCount != 0) // ¿êby nie dosz³o do wybrania wêz³a do którego nie mo¿e dojœæ bo nie ma z nim po³¹czenia
            {
                tempMinDist = Vector3.Distance(graphNodes[i].Value.position, position);
                if (tempMinDist < minDistance)
                {
                    minDistance = tempMinDist;
                    closestNode = graphNodes[i];
                }
            }
        }

        return closestNode;
    }

    static private float Orientation(Vector3 pointA, Vector3 pointB, Vector3 point)
    {
        Vector3 direction = pointB - pointA;
        Vector3 pointDirection = point - pointA;
        return ((direction.x * pointDirection.y) - (direction.y * pointDirection.x));
    }

    static private List<Vector3> GetLeftSidePoints(Vector3 pointA, Vector3 pointB, List<Vector3> points)
    {
        List<Vector3> leftPoints = new List<Vector3>();

        foreach (Vector3 point in points)
        {
            if (Orientation(pointA, pointB, point) < 0.0f)
            {
                leftPoints.Add(point);
            }
        }

        return leftPoints;
    }


    static public List<Vector3> QuickhullRecurence(Vector3 baseLineLeft,  Vector3 baseLineRight, List<Vector3> pointsSet)
    {
        //Debug.Log("Quickhull.Recurence: " + pointsSet.Count);
        if (pointsSet.Count <= 1)
        {
            return pointsSet;
        }

        Vector3 direction = baseLineRight - baseLineLeft;
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward);

        Vector3 furtherest = pointsSet[0];
        float currentDistance = Vector3.Dot(perpendicular, furtherest - baseLineLeft);

        for (int i = 1; i < pointsSet.Count; i++)
        {
            float distance = Vector3.Dot(perpendicular, pointsSet[i] - baseLineLeft);
            if (distance > currentDistance)
            {
                furtherest = pointsSet[i];
                currentDistance = distance;
            }
        }

        List<Vector3> result = new List<Vector3>();

        //Left divide
        result.AddRange(QuickhullRecurence(baseLineLeft, furtherest, GetLeftSidePoints(baseLineLeft, furtherest, pointsSet)));

        result.Add(furtherest);
        
        //Right divide
        result.AddRange(QuickhullRecurence(furtherest, baseLineRight, GetLeftSidePoints(furtherest, baseLineRight, pointsSet)));

        return result;
    }

    static public Vector3[] Quickhull(Vector3[] positions)
    {
        //Debug.Log("Quickhull.Start: " + positions.Length);
        if (positions.Length <= 3)
        {
            return positions;
        }
        List<Vector3> result = new List<Vector3>();


        Vector3 minX = positions[0];
        Vector3 maxX = positions[0];

        foreach (Vector3 position in positions)
        {
            if (position.x < minX.x)
            {
                minX = position;
            }
            else if (position.x > maxX.x)
            { 
                maxX = position;
            }
        }

        result.Add(minX);

        result.AddRange(QuickhullRecurence(minX, maxX, GetLeftSidePoints(minX, maxX, new List<Vector3>(positions))));

        result.Add(maxX);

        result.AddRange(QuickhullRecurence(maxX, minX, GetLeftSidePoints(maxX, minX, new List<Vector3>(positions))));


        return result.ToArray<Vector3>();

    }

    public PatrolArea GetPatrolingArea(Vector3 startPosition, float maxDistance, int hullParts, float middleFactor, float outerIgnoreRadius, float middleIgnoreRadius)
    {
        PathfindingGraphNode startNode = GetClosestGraphNode(startPosition);

        //startNode.Visited = true;

        //Queue<PathfindingGraphNode> nodesQueue = new Queue<PathfindingGraphNode>();
        //Queue<AStarNode> astarNodesQueue = new Queue<AStarNode>();
        PriorityQueue<AStarNode, float> astarNodesQueue = new PriorityQueue<AStarNode, float>((float priority1, float priority2) => { return priority1 < priority2; });

        //nodesQueue.Enqueue(startNode);

        astarNodesQueue.Enqueue(new AStarNode(startNode, null, 0, 0), 0.0f);

        List<Vector3> resultPositions = new List<Vector3>();
        List<Vector3> allPositions = new List<Vector3>();

        AStarNode tempAStarNode;
        while (astarNodesQueue.Count > 0)
        {
            //PathfindingGraphNode processedNode = nodesQueue.Dequeue();
            AStarNode processedNode = astarNodesQueue.Dequeue();

            //allPositions.Add(processedNode.Node.Value.transform.position); //old pos
            allPositions.Add(processedNode.Node.Value.position);
            //processedNode.Node.Visited = true;

            int counter = 0;
            bool isEdgeNode = false;
            for (int i = 0; i < processedNode.Node.LinkedCount; i++)
            {
                //Debug.Log("Processing Linked");
                //float distanceFromStart = processedNode.CostFromStart + processedNode.Node.Edge(i).Weight;
                //float distanceFromStart = ChebyshevDistance(processedNode.Node.Value.transform.position, processedNode.Node[i].Value.transform.position);
                //float distanceFromStart = ChebyshevDistance(startPosition, processedNode.Node[i].Value.transform.position);
                //float distanceFromStart = Vector3.Distance(startPosition, processedNode.Node[i].Value.transform.position); //old pos
                float distanceFromStart = Vector3.Distance(startPosition, processedNode.Node[i].Value.position);
        
                if (distanceFromStart <= processedNode.CostFromStart)
                {
                    counter++;
                }
        
                if (!processedNode.Node[i].Visited)
                {
                    if (distanceFromStart < maxDistance)
                    {
                        //Debug.Log("Visiting");
                        processedNode.Node[i].Visited = true;
        
                        tempAStarNode = new AStarNode(processedNode.Node[i], processedNode, distanceFromStart, 0.0f);
        
                        astarNodesQueue.Enqueue(tempAStarNode, distanceFromStart);
                    }
                    else
                    {
                        isEdgeNode = true;
                        //resultPositions.Add(processedNode.Node[i].Value.transform.position);
                    }
        
                }
                else
                {
                    //counter++;
                }
            }
            if (counter == processedNode.Node.LinkedCount || isEdgeNode)
            {
                //resultPositions.Add(processedNode.Node.Value.transform.position); //old pos
                resultPositions.Add(processedNode.Node.Value.position);
            }
        
        }

        //Vector3[] hullPoints = Quickhull(resultPositions.ToArray<Vector3>());
        //Vector3[] hullPoints = resultPositions.ToArray<Vector3>();
        //Vector3[] hullPoints = allPositions.ToArray<Vector3>();
        Vector3[] hullPoints = Quickhull(allPositions.ToArray<Vector3>());

        float overallDistance = 0.0f;
        for (int i = 1; i < hullPoints.Length; i++)
        {
            overallDistance += Vector3.Distance(hullPoints[i - 1], hullPoints[i]);
        }

        //int hullParts = 5;

        float distancePart = overallDistance / hullParts;
        float currentDistance = 0.0f;
        float expectedDistance = distancePart;

        List<Vector3> chosenPoints = new List<Vector3>();
        int begin = Random.Range(0, hullPoints.Length);
        int itr = begin;
        do
        {
            int nextIndex = (itr + 1) % hullPoints.Length;

            currentDistance += Vector3.Distance(hullPoints[itr], hullPoints[nextIndex]);
            if (currentDistance > expectedDistance)
            {
                expectedDistance += distancePart;
                chosenPoints.Add(hullPoints[nextIndex]);
            }

            itr = nextIndex;
        }
        while (itr != begin);

        Vector3[] middlePoints = new Vector3[hullPoints.Length];

        //float middleFactor = 0.4f;

        for (int i = 1; i < chosenPoints.Count; i++)
        {
            Vector3 avaragePosition = (chosenPoints[i - 1] + chosenPoints[i]) / 2;
            avaragePosition += avaragePosition * middleFactor + (1.0f - middleFactor) * startPosition;
            middlePoints[i] = avaragePosition;
        }
        middlePoints[0] = ((chosenPoints[0] + chosenPoints[chosenPoints.Count - 1]) / 2) * middleFactor + (1.0f - middleFactor) * startPosition;

        for (int i = 0; i < middlePoints.Length; i++)
        {
            //middlePoints[i] = GetClosestGraphNode(middlePoints[i]).Value.transform.position; //old pos
            middlePoints[i] = GetClosestGraphNode(middlePoints[i]).Value.position;
        }

        //return resultPositions.ToArray<Vector3>();
        return new PatrolArea(hullPoints, middlePoints, outerIgnoreRadius, middleIgnoreRadius);
    }
}
