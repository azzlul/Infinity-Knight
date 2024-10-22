using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private float speed = 2.0f;
    private float leftSpeed = 2.0f;
    private float rightSpeed = 2.0f;
    [SerializeField] private int direction = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.parent.Translate(new Vector3(0, direction, 0) * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PRB"))
        {
            direction = -1;
            speed = rightSpeed;
        }
        if (other.gameObject.CompareTag("PLB"))
        {
            direction = 1;
            speed = leftSpeed;
        }
    }
}
