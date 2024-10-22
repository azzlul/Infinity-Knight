using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFly : MonoBehaviour
{
    private bool isFacingForwards = true;
    private bool isFacingBackwards = false;
    private GameObject player;
    private void Start()
    {
        player = GameObject.Find("PlayerBody");
    }
    void Update()
    {if(player != null)
        LookAtPlayer();
        ChangeRotation();
    }
    void LookAtPlayer()
    {
        Vector3 lookVector = player.transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
    }
    void ChangeRotation()
    {
        if (transform.rotation.y >= 0 && isFacingForwards)
        {
            
            transform.GetChild(0).Rotate(0, 90, 0);
            isFacingForwards = false;
            isFacingBackwards = true;
        }
        if (transform.rotation.y <= 0 && isFacingBackwards)
        {
          
            transform.GetChild(0).Rotate(0, -90, 0);
            isFacingBackwards = false;
            isFacingForwards = true;
        }
    }
}
