using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    private PlayerController player;
    private Image image;
    private bool cooldown;
    public GameObject chargeBorder;
    void Start()
    {
        player = GameObject.Find("PlayerBody").GetComponent<PlayerController>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
 
        if(image.fillAmount >= 0.15)
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }
        if (PlayerController.playerHealth != 0)
        {
            image.fillAmount = player.jumpTimer / player.maxJumpTimer;
        }
        if(Input.GetKey(KeyCode.W) && image.fillAmount >= 0.15)
        {
            chargeBorder.gameObject.SetActive(true);
        }
        else if(image.fillAmount != 1)
        {
            chargeBorder.gameObject.SetActive(false);
        }
        if(image.fillAmount == player.maxJumpTimer && !cooldown)
        {
            cooldown = true;
            StartCoroutine(ChangeColor());            
        }
        if(image.fillAmount == 0)
        {
            image.color = new Color32(0, 50, 255, 255);
            cooldown = false;
        }
        if(PlayerController.playerHealth == 0)
        {
            image.fillAmount = 0;
        }
    }
    IEnumerator ChangeColor()
    {
        while (image.fillAmount == player.maxJumpTimer)
        {
            image.color = new Color32(255, 255, 255, 255);
            yield return new WaitForSeconds(0.15f);
            image.color = new Color32(0, 50, 255, 255);
            yield return new WaitForSeconds(0.15f);
        }
        cooldown = false;
    }
}
