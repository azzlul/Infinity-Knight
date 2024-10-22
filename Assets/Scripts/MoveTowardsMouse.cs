using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsMouse : MonoBehaviour
{
    Vector3 mousePos;
    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(mousePos * speed * Time.deltaTime);
    }
}
