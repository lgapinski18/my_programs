using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Knockback : MonoBehaviour
{
    [SerializeField] public MonsterAttacker attacker;
    [SerializeField] public AudioClip knockback_clip;
    [SerializeField] public AudioMixerGroup amg;

    private AudioSource audio_source;

    private void Start()
    {
        attacker.attackEevent += PlayKnockbackSound;
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.loop = false;
        audio_source.outputAudioMixerGroup = amg;
        audio_source.clip = knockback_clip; 
    }


    void PlayKnockbackSound()
    {
        audio_source.Play();
    }
}
