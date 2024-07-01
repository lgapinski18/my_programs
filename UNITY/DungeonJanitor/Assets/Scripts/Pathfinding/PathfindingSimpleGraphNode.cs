using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using UnityEngine;

public class PathfindingSimpleGraphNode
{
    private PathfindingNode value;
    public PathfindingNode Value { get => value; set => this.value = value; }


    private List<PathfindingSimpleGraphNode> linkedNodes = new List<PathfindingSimpleGraphNode>();

    public int LinkedCount { get => linkedNodes.Count; }

    public PathfindingSimpleGraphNode this[int key]
    {
        get => linkedNodes[key];
    }

    public PathfindingSimpleGraphNode()
    {
    }

    public PathfindingSimpleGraphNode(PathfindingNode value)
    {
        this.value = value;
    }

    private void JustAddLinkedNode(PathfindingSimpleGraphNode node)
    {
        if (!linkedNodes.Contains(node))
        {
            linkedNodes.Add(node);
        }
    }

    public void AddLinkedNode(PathfindingSimpleGraphNode node)
    {
        if (!linkedNodes.Contains(node))
        {
            linkedNodes.Add(node);
            node.JustAddLinkedNode(this);
        }
    }
}