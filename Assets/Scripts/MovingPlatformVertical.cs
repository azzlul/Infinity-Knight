using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    private float speed = 2.0f;
    [SerializeField] private int direction = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.parent.Translate(new Vector3(0, 0, direction) * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PUB"))
        {
            direction = -1;
        }
        if (other.gameObject.CompareTag("PBB"))
        {
            direction = 1;
        }
    }
}
