using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed = 3f;
    private float xBound = -25f;
    public bool isBackground;
    public float xBoundBackground ;
    private Vector3 spawnPosBackground = new Vector3(0, 0, 0);
    public GameObject backgroundSprite;
    public bool isTitleScreen;
    // Start is called before the first frame update
    void Start()
    {
        if (isTitleScreen)
            Time.timeScale = 1;
        if(isBackground)
        {
            xBoundBackground = -(backgroundSprite.GetComponent<SpriteRenderer>().bounds.size.x / 2);
            transform.position = spawnPosBackground;
            speed = 1f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.position.x < xBound && !isBackground)
            Destroy(gameObject);
        if(isBackground && transform.position.x < xBoundBackground)
        {
            transform.position = spawnPosBackground;
        }
    }
}
