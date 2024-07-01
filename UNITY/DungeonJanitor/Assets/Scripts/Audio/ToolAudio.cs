using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ToolAudio : MonoBehaviour
{
    #region SCRIPT_PROPERTY


    #endregion

    AudioSource audioSource;
    AbstractTool tool;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        tool = GetComponent<AbstractTool>();
        tool.OnBeginUsingEvent += PlaySound;
        tool.OnEndUsingEvent += StopSound;
    }


    void PlaySound(AbstractTool tool)
    {
        Debug.Log("Playing Tool Sound");
        audioSource?.Play();
    }

    void StopSound(AbstractTool tool)
    {
        audioSource?.Stop();
    }
}
