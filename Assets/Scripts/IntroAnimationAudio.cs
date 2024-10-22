using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimationAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nr1, nr2, nr3, go;
    public void PlayAudio1()
    {
        nr1.GetComponent<AudioSource>().Play();
    }
    public void PlayAudio2()
    {
        nr2.GetComponent<AudioSource>().Play();
    }
    public void PlayAudio3()
    {
        nr3.GetComponent<AudioSource>().Play();
    }
    public void PlayAudioGo()
    {
        go.GetComponent<AudioSource>().Play();
    }
}
