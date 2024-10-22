using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private BoxCollider platformCollider;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerBody");
        platformCollider = GetComponent<BoxCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < transform.position.y)
            platformCollider.enabled = false;
        else
            platformCollider.enabled = true;
    }
}
