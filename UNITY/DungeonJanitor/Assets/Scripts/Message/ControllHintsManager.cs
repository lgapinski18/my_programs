using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ControllHintsManager : MonoBehaviour
{
    private static ControllHintsManager instance = null;

    public enum Keys { A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, LMB, CMB, RMB, LEFT, RIGHT, UP, DOWN, TAB };

    public GameObject ControllHintsContent;
    public ControllHint ControllHintPrefab;

    public Sprite[] KeySprites = new Sprite[34];
    //public Dictionary<Keys, Sprite> KeyMapping = new Dictionary<Keys, Sprite>();

    private List<ControllHint> ControllHints = new List<ControllHint>();


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }


    public static ControllHintsManager Instance()
    {
        return instance;
    }

    public ControllHint AddControllHint(Keys key, string controllDescription)
    {
        ControllHint controllHint = Instantiate(ControllHintPrefab);

        controllHint.SetHintData(KeySprites[(int)key], controllDescription);

        controllHint.gameObject.transform.parent = ControllHintsContent.transform;
        ControllHints.Add(controllHint);

        return controllHint;
    }

    public void RemoveControllHint(ControllHint controllHint)
    {
        ControllHints.Remove(controllHint);
        Destroy(controllHint.gameObject);
    }
}
