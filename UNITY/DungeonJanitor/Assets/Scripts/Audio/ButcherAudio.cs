using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

public class ButcherAudio : MonoBehaviour
{
    [SerializeField] public AudioClip chop_clip;
    [SerializeField] public AudioMixerGroup amg;
    [SerializeField] public float max_sound_distance;

    private MonsterButcher butcher;
    private GameObject player;
    private AudioSource audio_source;

    private void SetVolume()
    {
        float distance = (transform.position - player.transform.position).magnitude;

        audio_source.volume = (-1.0f / max_sound_distance) * distance + 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        butcher = GetComponent<MonsterButcher>();
        player = GameObject.Find("Player");
        audio_source = gameObject.AddComponent<AudioSource>();
        print(audio_source.ToString());
        audio_source.loop = false;  
        audio_source.outputAudioMixerGroup = amg;
        audio_source.clip = chop_clip;
        butcher.OnButcherStartEvent += PlaySound;
        butcher.OnButcherEndedEvent += StopSound;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.C)) 
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        SetVolume();
        SetPan();
        audio_source.loop = true;
        audio_source?.Play();
    }
    void StopSound()
    {
        //SetVolume();
        //SetPan();
        audio_source.loop = false;
        audio_source?.Stop();
    }

    private void SetPan()
    {
        Vector2 direction = ((Vector2)transform.position - (Vector2)player.transform.position);
        direction.Normalize();

        audio_source.panStereo = Vector2.Dot(Vector2.right, direction);

    }
}
