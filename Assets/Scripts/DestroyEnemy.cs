using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : MonoBehaviour
{
    private float xBound = 15f;
    private float yBound = 10f;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -xBound || transform.position.x > xBound || transform.position.y > yBound || transform.position.y < -yBound)
            Destroy(gameObject);
        GetComponent<Rigidbody>().velocity = parent.gameObject.GetComponent<Rigidbody>().velocity;
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
