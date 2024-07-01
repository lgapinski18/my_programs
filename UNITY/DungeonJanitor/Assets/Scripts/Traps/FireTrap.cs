using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : Trap
{
    bool playerHit;
    public AttackEffect[] attackEffects = new AttackEffect[0];

    private void Start()
    {
        SetupTrapMask();
    }

    // Update is called once per frame
    void Update()
    {
        Animator a = GetComponent<Animator>();
        
        if ((int)Time.time % interval == 0)
        {
            
            a.Play("FireTrapActive");
        }
        else
        {
            a.Play("FireTrapIdle");
            playerHit = false;
        }


    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (activated && playerHit == false && other.tag == "Player" && !other.isTrigger)
        {
            //Debug.Log("Player is playerHit by a fire trap: " + other.gameObject.name.ToString());
            playerHit = true;
            if (other.gameObject.GetComponent<Player>().isDamageable) { CameraShake.Shake(0.5f, new Vector3(0.25f, 0.25f, 0)); }
            other.gameObject.GetComponent<IAttackable>().Attack(new AttackArgs(gameObject, (AttackEffect[])attackEffects.Clone()));
        }
        if (activated && other.gameObject.GetComponent<Corpse>() != null && ValidateIsTarget(other.gameObject))
        {
            Debug.Log("Fire trap destroys: " + other.gameObject.name.ToString());
            other.gameObject.GetComponent<Corpse>().Eat(ResultCorpseTypeMask);
        }

    }
}
