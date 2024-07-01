using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Vector3 offset = Vector3.zero;

    public Vector3 position { get => transform.position + offset; }
}
