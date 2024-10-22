using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public GameObject[] music;
    private bool isPaused = true;
    private int index = 0;
    private int[] song = {0, 1, 2 ,3};
    private void Start()
    {
        Randomize(song);
    }
    void Update()
    {
        if (Time.timeScale == 0)
        {
            music[song[index]].gameObject.GetComponent<AudioSource>().Pause();
            isPaused = true;
        }
        if(Time.timeScale == 1 && isPaused)
        {
            music[song[index]].gameObject.GetComponent<AudioSource>().Play();
            isPaused = false;
        }
        if (!isPaused && music[song[index]].gameObject.GetComponent<AudioSource>().isPlaying == false)
        {
            index++;
            if (index == 4) index = 0;
            music[song[index]].gameObject.GetComponent<AudioSource>().Play();
            isPaused = false;
        }      
    }
    void Randomize(int[] song)
    {
        song[0] = Random.Range(0, 3);
        song[1] = Random.Range(0, 3);
        if (song[1] == song[0])
        {
            if (song[0] == 0) song[1] = 3;
            else song[1]--;
        }
        song[2] = Random.Range(0, 3);
        while (song[2] == song[0] || song[2] == song[1])
        {
            if (song[2] == 0) song[2] = 3;
            else song[2]--;
        }
        song[3] = 6 - song[0] - song[1] - song[2];
    }
}

