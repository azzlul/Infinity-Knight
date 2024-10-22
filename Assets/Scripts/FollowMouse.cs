using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float publicAngle;
    private IEnumerator shootArrow;
    public GameObject projectile1;
    public GameObject projectile2;
    public GameObject projectile3;
    public GameObject projectile4;
    private float angle;
    private bool arrowCooldown;
    private PlayerController playerController;
    public float arrowDamage = 20f;
    public float arrowCount= 1;
    public GameObject sword;
    public GameObject bow1;
    public GameObject bow2;
    private SpawnManager spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        arrowCount = 1;
        if (Difficulty.isNewGamePlus)
            arrowCount = 4;
        playerController = GameObject.Find("PlayerBody").GetComponent<PlayerController>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMousePoint();
        ShootAtMouse();
    }
    void ShootAtMouse()
    {
        if (Input.GetKey(KeyCode.Mouse1) && !playerController.isPlayerStunned && !arrowCooldown && !Input.GetKey(KeyCode.Mouse0) && !spawnManager.isGamePaused)
        {
            arrowCooldown = true;
            StartCoroutine(ShootArrow());
        }
        if(Input.GetKeyUp(KeyCode.Mouse1) && !playerController.isPlayerStunned && arrowCooldown)
        {
        }
    }
    void FollowMousePoint()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        publicAngle = angle;
        if (angle >= 90 || angle <= -90)
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, -angle - 270));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270));
    }
    IEnumerator ShootArrow()
    {    
        while (Input.GetKey(KeyCode.Mouse1))
        {
            sword.GetComponent<MeshRenderer>().enabled = false;
            bow1.GetComponent<MeshRenderer>().enabled = true;
            bow2.GetComponent<MeshRenderer>().enabled = true;
            if (arrowCount == 1)
            {
                GameObject firedProjectile = Instantiate(projectile1, bow1.transform.position, transform.rotation);
                firedProjectile.GetComponent<Rigidbody>().velocity = transform.up * 20f;
                yield return new WaitForSeconds(0.5f);
            }
            else if (arrowCount == 2)
            {
                GameObject firedProjectile = Instantiate(projectile2, bow1.transform.position, transform.rotation);
                firedProjectile.GetComponent<Rigidbody>().velocity = transform.up * 20f;
                yield return new WaitForSeconds(0.5f);
            }
            else if (arrowCount == 3)
            {
                GameObject firedProjectile = Instantiate(projectile3, bow1.transform.position, transform.rotation);
                firedProjectile.GetComponent<Rigidbody>().velocity = transform.up * 20f;
                yield return new WaitForSeconds(0.5f);
            }
            else if (arrowCount == 4)
            {
                GameObject firedProjectile = Instantiate(projectile4, bow1.transform.position, transform.rotation);
                firedProjectile.GetComponent<Rigidbody>().velocity = transform.up * 20f;
                yield return new WaitForSeconds(0.5f);
            }
        }
        arrowCooldown = false;
    }
}
