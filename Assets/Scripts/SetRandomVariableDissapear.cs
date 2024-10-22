using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomVariableDissapear : MonoBehaviour
{
    public float delayTime;
    public bool isDelayed;
    void Start()
    {
        delayTime = Random.Range(0, 2f);
        isDelayed = Random.value > 0.5f;
    }
}
