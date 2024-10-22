using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int heartIndex;
    public Sprite emptyHeart;
    public Sprite fullHeart;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerBody").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(heartIndex > 3)
        {
            if (PlayerController.playerHealth < heartIndex)
            {
                gameObject.GetComponent<Image>().enabled = false;
            }
            else
                gameObject.GetComponent<Image>().enabled = true;
        }
        if( heartIndex == 3)
        {
            if (PlayerController.playerHealth < 3)
                GetComponent<Image>().sprite = emptyHeart;
            else
                GetComponent<Image>().sprite = fullHeart;
        }
        if(heartIndex == 2)
        {
            if (PlayerController.playerHealth < 2)
                GetComponent<Image>().sprite = emptyHeart;
            else
                GetComponent<Image>().sprite = fullHeart;
        }
        if(heartIndex == 1)
        {
            if (PlayerController.playerHealth < 1)
                GetComponent<Image>().sprite = emptyHeart;
            else
                GetComponent<Image>().sprite = fullHeart;
        }
    }
}
