using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    private GameObject player;
    private float speed = 10f;
    private float xBound = 15f;
    private float yBound = 10f;
    Vector3 lookDirection;
    public int projectilePlacement;
    public bool isDifficulty3;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerBody");
        lookDirection = (player.transform.position - transform.position).normalized;
        if (projectilePlacement == 1)
        {
            lookDirection.x -= 0.15f;
            if (lookDirection.x <= 0)
                lookDirection.y += 0.15f;
            else
                lookDirection.y -= 0.15f;
        }
        if (projectilePlacement == 3)
        {
            lookDirection.x += 0.15f;
            if (lookDirection.x <= 0)
                lookDirection.y -= 0.15f;
            else
                lookDirection.y += 0.15f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(lookDirection * speed * Time.deltaTime);
        if (transform.position.x < -xBound || transform.position.x > xBound || transform.position.y > yBound || transform.position.y < -yBound)
            Destroy(gameObject);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("ShieldPlatform"))
        {
            Destroy(gameObject);
        }
    }
}
