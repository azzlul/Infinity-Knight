using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStart : MonoBehaviour
{
    public GameObject cameraMain;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<AudioSource>().isPlaying && !cameraMain.gameObject.GetComponent<AudioSource>().isPlaying) gameObject.GetComponent<AudioSource>().Play();
    }
}
