using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using UnityEngine;

public class PathfindingGraphNode
{
    public struct GraphEdge
    {
        private PathfindingGraphNode linkedNode;
        private float weight;

        public PathfindingGraphNode LinkedNode { get => linkedNode; }
        public float Weight { get => weight; }

        public GraphEdge(PathfindingGraphNode linkedNode, float weight)
        {
            this.linkedNode = linkedNode;
            this.weight = weight;
        }
    }

    private PathfindingNode value;
    public PathfindingNode Value { get => value; set => this.value = value; }

    private bool visited = false;
    public bool Visited { get => visited; set => visited = value; }


    private List<GraphEdge> edges = new List<GraphEdge>();

    public int LinkedCount { get => edges.Count; }

    public PathfindingGraphNode this[int key] { get => edges[key].LinkedNode; }

    public PathfindingGraphNode()
    {
    }

    public PathfindingGraphNode(PathfindingNode value)
    {
        this.value = value;
    }

    private void JustAddLinkedNode(PathfindingGraphNode node, float weight)
    {
        if (!edges.Exists(edge => edge.LinkedNode == node))
        {
            edges.Add(new GraphEdge(node, weight));
        }
    }

    public void AddLinkedNode(PathfindingGraphNode node, float weight)
    {
        if (!edges.Exists(edge => edge.LinkedNode == node))
        {
            edges.Add(new GraphEdge(node, weight));
            node.JustAddLinkedNode(this, weight);
        }
    }

    public GraphEdge Edge(int index)
    {
        return edges[index];
    }
    public GraphEdge EdgeTo(PathfindingGraphNode targetNode)
    {
        GraphEdge edge = edges.Find(edge => edge.LinkedNode == targetNode);
        return edge;
    }

    public bool HasLinked(PathfindingGraphNode node)
    {
        return edges.Exists(edge => edge.LinkedNode == node);
    }
}