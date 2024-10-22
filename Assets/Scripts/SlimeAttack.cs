using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject slime;

    public void Shoot()
    {
        slime.GetComponent<SlimeMovement>().Shoot();
    }
}
