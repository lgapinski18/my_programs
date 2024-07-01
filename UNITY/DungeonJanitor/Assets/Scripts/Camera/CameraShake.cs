using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private void Awake() => Instance = this;

    private void OnShake(float duration, Vector3 strength)
    {
        transform.DOShakePosition(duration, strength);
        transform.DOShakeRotation(duration, strength);
    }

    public static void Shake(float duration, Vector3 strength) => Instance.OnShake(duration, strength);
}