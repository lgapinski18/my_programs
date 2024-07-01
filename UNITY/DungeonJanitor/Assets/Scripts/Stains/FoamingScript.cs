using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FoamingScript : MonoBehaviour
{
    [SerializeField]
    private Color32 baseStainColor = Color.white;
    [SerializeField]
    private Color32 redDetergentColor = Color.white;
    [SerializeField]
    private Color32 blueDetergentColor = Color.white;
    [SerializeField]
    private Color32 greenDetergentColor = Color.white;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private ASubstainType[] parentSubstains;
    private int appliedCounter = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = baseStainColor;

        parentSubstains = transform.parent.GetComponents<ASubstainType>();
        foreach (ASubstainType substainType in parentSubstains)
        {
            substainType.OnAppliedDetergent += OnAppliedDetergent;
            substainType.OnDetergentEffectEnded += OnAppiedDetergentEnded;
        }
        transform.parent.GetComponent<SubstainController>().OnPartlyCleaned += OnPartlyCleanedEvent;

        spriteRenderer.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnAppliedDetergent(ASubstainType substainType, ADetergent detergent)
    {
        appliedCounter++;
        if (detergent is RedDetergent)
        {
            spriteRenderer.color += redDetergentColor;
        }
        else if (detergent is GreenDetergent)
        {
            spriteRenderer.color += greenDetergentColor;
        }
        else if (detergent is BlueDetergent)
        {
            spriteRenderer.color += blueDetergentColor;
        }
        if (appliedCounter == 1)
        {
            gameObject.SetActive(true);
            spriteRenderer.enabled = true;
            animator.Play("Foaming");
        }
    }

    private void OnAppiedDetergentEnded(ASubstainType substainType, ADetergent detergent)
    {
        appliedCounter--;
        if (detergent is RedDetergent)
        {
            spriteRenderer.color -= redDetergentColor;
        }
        else if (detergent is GreenDetergent)
        {
            spriteRenderer.color -= greenDetergentColor;
        }
        else if (detergent is BlueDetergent)
        {
            spriteRenderer.color -= blueDetergentColor;
        }

        if (appliedCounter == 0)
        {
            gameObject.SetActive(false);
            spriteRenderer.enabled = false;
        }
    }

    private void OnPartlyCleanedEvent(SubstainController substain)
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, (substain.Dirtiness / (float)substain.MaxDirtiness) * 0.8f + 0.2f);
    }
}
