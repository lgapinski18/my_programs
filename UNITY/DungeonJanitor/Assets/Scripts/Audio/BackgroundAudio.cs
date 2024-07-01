using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundAudio : MonoBehaviour
{
    [SerializeField] public AudioClip[] audio_clips;
    [SerializeField] public AudioMixerGroup amg;

    private int clip_ctr = 0;
    private AudioSource audio_source;

    private bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.loop = false;
        audio_source.clip = audio_clips[0];
        audio_source.playOnAwake = false;
        audio_source.outputAudioMixerGroup = amg;
    }

    private IEnumerator NextClip() 
    {
        yield return new WaitForSeconds(audio_clips[clip_ctr].length);
        playing = false;
        clip_ctr++;
        if(clip_ctr == audio_clips.Length)
        {
            clip_ctr = 0;
        }
        audio_source.clip = audio_clips[clip_ctr];
    }

    private void PlayClip()
    {
        audio_source.Play();
        playing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!playing)
        {
            PlayClip();
            NextClip();
        }
    }
}
