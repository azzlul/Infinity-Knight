using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitboxActivation : MonoBehaviour
{
    public GameObject swordHitbox;
    void ActivateHitBox()
    {
        swordHitbox.GetComponent<BoxCollider>().enabled = true;
    }
    void DeactivateHitBox()
    {
        swordHitbox.GetComponent<BoxCollider>().enabled = false;
    }
}
