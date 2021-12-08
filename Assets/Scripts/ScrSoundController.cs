using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrSoundController : MonoBehaviour
{
    public void PlayAt(AudioClip clip, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(clip, pos, 1);
    }
}
