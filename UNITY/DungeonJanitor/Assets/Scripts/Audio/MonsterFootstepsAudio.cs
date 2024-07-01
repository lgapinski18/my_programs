using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFootstepsAudio : MonoBehaviour
{
    [SerializeField] AudioSource audio_source;
    [SerializeField] AudioClip[] left_footsteps;
    [SerializeField] AudioClip[] right_footsteps;
    [SerializeField] MonsterMovementComponent movement;

    [SerializeField] public float magnitude_cutoff;
    [SerializeField] public int typical_bpm;

    [SerializeField] public float max_sound_distance;

    private Rigidbody2D rigidbody;
    private float typical_speed;
    private float max_lenght;
    private float time_counter = 0.0f;
    private float current_time_interval;

    // 0 - left, 1 - right
    private bool foot;
    private Player player;

    public int BPM
    {
        get 
        {
            return (int) ((rigidbody.velocity.magnitude * (float)typical_bpm) / typical_speed);
        }
    }
    
    private float TimeInterval
    {
        get
        {
            float return_value = 60.0f / (float)BPM;
            if(return_value < max_lenght)
            {
                return max_lenght + 0.01f;
            }
            else
            {
                return return_value + 0.01f;
            }
        }
    }
   
    private void SetVolume()
    {
        float distance = (transform.position - player.transform.position).magnitude;

        audio_source.volume = (-1.0f/ max_sound_distance) * distance + 1.0f;       
    }

    private void SetPan()
    {
        Vector2 direction = ((Vector2)transform.position - (Vector2)player.transform.position);
        direction.Normalize();

        audio_source.panStereo = Vector2.Dot(Vector2.right, direction); 

    }

    private void ChangeFoot()
    {
        foot = !foot;
    }

    private IEnumerator LoadNextStep()
    {
        yield return new WaitForSeconds(audio_source.clip.length);
        if (foot)
        {
            audio_source.clip = left_footsteps[Random.Range(0, left_footsteps.Length)];
        }
        else
        {
            audio_source.clip = right_footsteps[Random.Range(0, right_footsteps.Length)];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = gameObject.GetComponent<Rigidbody2D>(); 
        this.typical_speed = movement.speed;
        max_lenght = float.MinValue;

        foreach(var a in left_footsteps) 
        {
            if(a.length > max_lenght)
            { 
                max_lenght = a.length;
            }
        }

        foreach (var a in right_footsteps)
        {
            if (a.length > max_lenght)
            {
                max_lenght = a.length;
            }
        }

        
        current_time_interval = 60.0f / (float)typical_bpm;

        this.player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody.velocity.magnitude > magnitude_cutoff)
        {
            time_counter += Time.deltaTime;

            if (time_counter > current_time_interval)
            {
                SetVolume();
                SetPan();
                current_time_interval = TimeInterval;
                time_counter = 0.0f;
                audio_source.Play();
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
