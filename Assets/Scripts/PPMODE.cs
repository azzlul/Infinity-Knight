using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PPMODE : MonoBehaviour
{

    private string[] ppMode = new string[] { "p", "p", "m", "o", "d", "e" };
    private int index = 0;
    public GameObject PPModeText;
    public static bool isPPModeOn;
    // Update is called once per frame

    private void Start()
    {
        if (isPPModeOn)
            PPModeText.gameObject.SetActive(true);
        else
            PPModeText.gameObject.SetActive(false);
    }
    void Update()
    {
        if(Input.anyKeyDown)
        {
            if (Input.GetKeyDown(ppMode[index]))
                index++;
            else
                index = 0;
        }
        if(index == ppMode.Length)
        {
            if (PPModeText.gameObject.activeSelf)
            {
                PPModeText.gameObject.SetActive(false);
                isPPModeOn = false;
            }
            else
            {
                PPModeText.gameObject.SetActive(true);
                isPPModeOn = true; 
            }
            index = 0;
        }
    }
}
