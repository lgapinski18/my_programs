using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ModifiersManagerComponent))]
public class BlueMainStain : MonoBehaviour
{

    #region SCRIPT_PROPERTIES

    [SerializeField]
    private float phase1 = 0.0f;
    [SerializeField]
    private float phase2 = 0.0f;
    [SerializeField]
    private float phase3 = 0.0f;
    [SerializeField]
    private float maxProgress = 0.0f;

    [SerializeField]
    private float gasesDisapearingThreshold = 0.2f;

    public float Phase1 { get => phase1; /* set => phase1 = value; */ }
    public float Phase2 { get => phase2; /* set => phase2 = value; */ }
    public float Phase3 { get => phase3; /* set => phase3 = value; */ }
    public float MaxProgress { get => maxProgress; /* set => maxProgress = value; */ }


    [SerializeField]
    private Animator[] toxicGasesAnimators;

    #endregion

    private int phase = 0;
    private MainStain mainStain;

    #region MODIFIERS

    private float totalProgress;
    private ModifiersManager mmTotalProgress;


    private void OnTotalProgressChanged(ModifiersManager modifiersManager)
    {
        float totalProgress = modifiersManager.GetModifier();
        UpdatePhase();
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mmTotalProgress = new ModifiersManager(ModifiersManager.ModifiersProcessingType.Avarage, this, 0.0f);
        mmTotalProgress.OnChanged += OnTotalProgressChanged;
        //GetComponent<ModifiersManagerComponent>().RegisterModifiersManager("progress", mmTotalProgress);
        UpdatePhase();
        mainStain = GetComponent<MainStain>();


        foreach (Animator animator in toxicGasesAnimators)
        {
            animator.transform.Rotate(-transform.rotation.eulerAngles);
        }

        foreach (SubstainController substain in GetComponentsInChildren<SubstainController>())
        {
            substain.OnCleaned += SubstainCleaned;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Animator animator in toxicGasesAnimators)
        {
            animator.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void SubstainCleaned(SubstainController substain)
    {
        float remainingPercentage = (mainStain.TotalSubstains - mainStain.CleanedSubstains) / (float)mainStain.TotalSubstains;

        float alpha = remainingPercentage * 0.6f + 0.3f;


        foreach (Animator animator in toxicGasesAnimators)
        {
            SpriteRenderer spriteRenderer = animator.GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;

            if (remainingPercentage < gasesDisapearingThreshold)
            {
                animator.gameObject.SetActive(false);
            }
        }
    }

    private void ChangePhase()
    {
        foreach (Animator animator in toxicGasesAnimators)
        {
            animator.SetInteger("Phase", phase);
        }
    }
    void UpdatePhase()
    {
        if (totalProgress >= maxProgress)
        {
            totalProgress = maxProgress;
            phase = 3;
        }
        else if (totalProgress >= phase3)
        {
            phase = 3;
        }
        else if (totalProgress >= phase2)
        {
            phase = 2;
        }
        else if (totalProgress >= phase1)
        {
            phase = 1;
        }
        else
        {
            phase = 0;
        }

        ChangePhase();
    }

    public void Progress(float progress)
    {
        totalProgress += progress / (mainStain.TotalSubstains - mainStain.CleanedSubstains);
        UpdatePhase();
    }

}
