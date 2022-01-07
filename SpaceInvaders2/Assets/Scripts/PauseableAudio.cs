using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseableAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource = null;

    private void Update()
    {
        if (Pause.Instance.IsGamePaused() && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else if (!Pause.Instance.IsGamePaused() && (!audioSource.isPlaying))
        {
            audioSource.Play();
        }
    }
}
