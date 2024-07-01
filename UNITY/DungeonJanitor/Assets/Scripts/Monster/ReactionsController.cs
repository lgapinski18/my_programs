using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ReactionsController : MonoBehaviour
{
    #region NESTED_TYPES

    [Serializable]
    public struct Reaction
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public Sprite reactionSprite;
        [SerializeField]
        public Color32 color;
        [SerializeField]
        public Vector3 position;
        [SerializeField]
        public float time;
    }

    [SerializeField]
    private Reaction[] reactions;

    #endregion

    private SpriteRenderer spriteRenderer;
    private IEnumerator coroutineReaction = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public bool MakeReaction(string reactionName)
    {
        float time = 0.0f;
        bool notFound = true;

        foreach (Reaction reaction in reactions)
        {
            if (reaction.name == reactionName)
            {
                notFound = false;
                spriteRenderer.sprite = reaction.reactionSprite;
                spriteRenderer.color = reaction.color;
                spriteRenderer.enabled = true;
                transform.localPosition = reaction.position;
                time = reaction.time;
                break;
            }
        }

        if (notFound)
        {
            return false;
        }

        if (coroutineReaction != null)
        {
            StopCoroutine(coroutineReaction);
        }

        spriteRenderer.enabled = true;
        coroutineReaction = CreateCoroutineEndingReaction(time);
        StartCoroutine(coroutineReaction);

        return true;
    }

    public IEnumerator CreateCoroutineEndingReaction(float time)
    {
        yield return new WaitForSeconds(time);

        EndReaction();
    }

    private void EndReaction()
    {
        spriteRenderer.enabled = false;
        coroutineReaction = null;
    }
}
