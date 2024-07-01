using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class FootstepsAudio : MonoBehaviour
{
    [SerializeField] public AudioSource audio_player;
    [SerializeField] private AudioClip[] left_footstep_clips;
    [SerializeField] private AudioClip[] right_footstep_clips;

    [SerializeField] public float magnitude_cutoff;
    [SerializeField] public int typical_bpm;

    private MovementComponent mc;
    private Rigidbody2D rigidbody;

    private float typical_speed;
    private float max_lenght;
    private float time_counter = 0.0f;
    private float current_time_interval;

    // 0 - left, 1 - right
    private bool foot;


    public int BPM
    {
        get
        {
            return (int)((rigidbody.velocity.magnitude * (float)typical_bpm) / typical_speed);
        }
    }

    private float TimeInterval
    {
        get
        {
            float return_value = 60.0f / (float)BPM;
            if (return_value < max_lenght)
            {
                return max_lenght + 0.01f;
            }
            else
            {
                return return_value + 0.01f;
            }
        }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        mc = GetComponent<MovementComponent>();
        typical_speed = mc.speed;
        max_lenght = float.MinValue;

        foreach (var a in left_footstep_clips)
        {
            if (a.length > max_lenght)
            {
                max_lenght = a.length;
            }
        }

        foreach (var a in right_footstep_clips)
        {
            if (a.length > max_lenght)
            {
                max_lenght = a.length;
            }
        }


        current_time_interval = 60.0f / (float)typical_bpm;
    }

    private void ChangeFoot()
    {
        foot = !foot;
    }

    private IEnumerator LoadNextStep()
    {
        yield return new WaitForSeconds(audio_player.clip.length);
        if (foot) 
        {
            audio_player.panStereo = -0.25f;
            audio_player.clip = left_footstep_clips[Random.Range(0, left_footstep_clips.Length)];
        }
        else
        {
            audio_player.panStereo = 0.25f;
            audio_player.clip = right_footstep_clips[Random.Range(0, right_footstep_clips.Length)];
        }
    }

    void Update()
    {

        if (rigidbody.velocity.magnitude > magnitude_cutoff)
        {
            time_counter += Time.deltaTime;

            if (time_counter > current_time_interval)
            {
                current_time_interval = TimeInterval;
                time_counter = 0.0f;
                audio_player.Play();
                ChangeFoot();
                StartCoroutine(LoadNextStep());
            }
        }
        else
        {
            time_counter = 0.0f;
        }
    }
}
