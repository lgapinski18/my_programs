using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasedEntiity : MonoBehaviour, IGasedEntity
{
    [Range(0f, 1f)]
    [SerializeField] public float gased_level_increase;
    [Range(0f, 0.16f)]
    [SerializeField] public float gased_level_decay;
    [SerializeField] public float MAX_GASED_LEVEL = 100.0f;
    [SerializeField] public float min_gased_interval = 30.0f; // jak czesto kaszle gdy minimalnie tj. nie zero, ale najmniejsza wartosc, wieksza od zera
    [SerializeField] public float max_gased_interval = 5.0f;  // jak czesto kaszle gdy zagazowany na maxa - co 5 sekund
    [SerializeField]
    private Animator coughAnimator;

    [SerializeField] public bool debug_mode = true;

    private float gased_level = 0.0f;
    private float current_cough_interval = float.PositiveInfinity;
    private float cough_timer = 0.0f;
    private bool is_gased = false;

    #region COUGHING_SPEED_INFULENCE_MANAGEMENT

    [SerializeField]
    private float speedModifier = 0.2f;
    [SerializeField]
    private float actionSpeedModifier = 0.5f;
    [SerializeField]
    private float coughLastingTime = 2.0f;

    private ModifierHandlesGroup speedModifiersHandles = new ModifierHandlesGroup();
    private ModifierHandlesGroup actionSpeedModifiersHandles = new ModifierHandlesGroup();
    private IEnumerator coroutineSpeedInfluence = null;

    private IEnumerator CreateCoroutineSpeedInfluence(float timeout)
    {
        yield return new WaitForSeconds(timeout);

        speedModifiersHandles.RemoveSelf();
        actionSpeedModifiersHandles.RemoveSelf();
        coroutineSpeedInfluence = null;
    }

    private void StartCoughSpeedSlow()
    {
        if (coroutineSpeedInfluence == null)
        {
            speedModifiersHandles = GetComponent<ModifiersManagerComponent>().AddModifierForPropertyName("actionMovementSlow", speedModifier);
            actionSpeedModifiersHandles = GetComponent<ModifiersManagerComponent>().AddModifierForPropertyName("actionSpeed", actionSpeedModifier);
            coroutineSpeedInfluence = CreateCoroutineSpeedInfluence(coughLastingTime);
            StartCoroutine(coroutineSpeedInfluence);
        }
    }
    

    #endregion

    bool IGasedEntity.IsGased
    {
        get => is_gased;
        set => is_gased = value;
    }

    private float GetCoughInterval()
    {
        if (gased_level <= 0.1f * MAX_GASED_LEVEL)
        {
            return float.PositiveInfinity;
        }
        else if (gased_level > 0.1f * MAX_GASED_LEVEL && gased_level < 0.9f * MAX_GASED_LEVEL)
        {
            return ((max_gased_interval - min_gased_interval) / MAX_GASED_LEVEL) * gased_level + min_gased_interval;
        }
        else 
        {
            if (gased_level > MAX_GASED_LEVEL)
            {
                gased_level = MAX_GASED_LEVEL;
            }
            return ((max_gased_interval - min_gased_interval) / MAX_GASED_LEVEL) * gased_level + min_gased_interval;
        } 
    }

    public void GasEntity()
    {
        if(is_gased) 
        {
            gased_level += gased_level_increase;
            current_cough_interval = GetCoughInterval();
        }
    }

    public void DecayGas()
    {
        if(!is_gased)
        {
            gased_level -= gased_level_decay;
        }   
    }

    public void Cough()
    {
        Debug.Log("cough!");
        //GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        StartCoughSpeedSlow();

        coughAnimator.Play("Cough");
        GetComponent<AudioSource>().Play();
    }

    void Start()
    {
        current_cough_interval = GetCoughInterval();
    }

    void Update()
    {
        GasEntity();
        DecayGas();
        gased_level = Mathf.Clamp(gased_level, 0.0f, MAX_GASED_LEVEL);

        if (gased_level > gased_level_decay) 
        {
            cough_timer += Time.deltaTime;
        }
        else
        {
            cough_timer = 0;
        }
        
        if (cough_timer > current_cough_interval)
        {
            Cough();
            cough_timer = 0.0f;
            current_cough_interval = GetCoughInterval();
        }

        //print($"gased_level: {gased_level}");
        //print($"cough_timer: {cough_timer}");
        //print($"current_cough_interval: {current_cough_interval}");
    }
}
