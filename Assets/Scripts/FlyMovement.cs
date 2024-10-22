using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{

    private float speed = 2;
    private float xBound = 18f;
    private Rigidbody flyRb;
    private int direction = -1;
    public GameObject projectile1;
    public GameObject projectile2;
    public GameObject projectile3;
    private float spawnDelay = 4f;
    private float spawnInterval = 4f;
    public Vector3 worldPosition;
    public float flyHealth;
    private float initialFlyHealth = 40f;
    private SwordHitbox sword;
    private FollowMouse arrow;
    public Material[] flashMaterials;
    public Material[] attackMaterials;
    private Material[] initialMaterialsBody;
    private Material initialMaterialWings;
    public GameObject body;
    public GameObject wing1;
    public GameObject wing2;
    public Material[] mediumMaterials;
    public Material[] hardMaterials;
    private int difficulty = 1;
    private int maxDifficulty = 1;
    private GameObject player;
    public Animator flyAnim;
    public AudioSource HitSound;
    private bool shouldShoot = true;
    // Start is called before the first frame update
    void Start()
    {
        SetDifficulty();
        ChangeMaterials();
        flyRb = GetComponent<Rigidbody>();
        ChangeSpeed();
        InvokeRepeating("ShootAtPlayer", spawnDelay, spawnInterval);
        player = GameObject.Find("PlayerBody");
        sword = GameObject.Find("Sword Hitbox").GetComponent<SwordHitbox>();
        arrow = GameObject.Find("Camera Follow").GetComponent<FollowMouse>();
        initialMaterialsBody = body.GetComponent<MeshRenderer>().materials;
        initialMaterialWings = wing1.GetComponent<MeshRenderer>().material;
        initialMaterialWings = wing2.GetComponent<MeshRenderer>().material;
        flyHealth = initialFlyHealth + (int)(Time.timeSinceLevelLoad / 6);
        if (flyHealth > 160 || Difficulty.isNewGamePlus)
            flyHealth = 160;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        PPMode();
    }

    void PPMode()
    {
        if(PPMODE.isPPModeOn || Time.timeSinceLevelLoad >= 1800)
        {
            Shoot();
        }
    }
    void Movement()
    {
        worldPosition = transform.TransformPoint(transform.position);
        if (worldPosition.x <= xBound+1) {
            flyRb.MovePosition(transform.position + new Vector3(direction, 0, 0) * speed * Time.deltaTime);        
             }
        else
            flyRb.MovePosition(transform.position + Vector3.left * 3.0f * Time.deltaTime);
        if (worldPosition.x < -xBound - 2)
            Destroy(gameObject);
    }
    void ShootAtPlayer()
    {
        flyAnim.SetTrigger("attack");
    }
    public void Shoot()
    {
        if (shouldShoot)
        {
            if (difficulty == 1)
            {
                Instantiate(projectile2, transform.position, projectile2.transform.rotation);
            }
            if (difficulty == 2)
            {
                Instantiate(projectile1, transform.position, projectile2.transform.rotation);
                Instantiate(projectile3, transform.position, projectile2.transform.rotation);
            }
            if (difficulty == 3)
            {
                Instantiate(projectile2, transform.position, projectile2.transform.rotation);
                Instantiate(projectile1, transform.position, projectile2.transform.rotation);
                Instantiate(projectile3, transform.position, projectile2.transform.rotation);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            flyHealth -= sword.swordDamage;
            if (HitSound.isPlaying == false)
                HitSound.Play();
            if (flyHealth <= 0)
            {
                shouldShoot = false;
                StartCoroutine(DeathAnimation());
            }
            else
            {
                StartCoroutine(IFrames(other));
            }
        }
        if (other.gameObject.CompareTag("Arrow"))
        {
            flyHealth -= arrow.arrowDamage;
            if (HitSound.isPlaying == false)
                HitSound.Play();
            if (flyHealth <= 0)
            {
                shouldShoot = false;
                StartCoroutine(DeathAnimation());
            }
            else
            {
                StartCoroutine(IFrames(other));
            }
        }
        IEnumerator IFrames(Collider other)
        {

            body.GetComponent<MeshRenderer>().materials = flashMaterials;
            wing1.GetComponent<MeshRenderer>().material = flashMaterials[0];
            wing2.GetComponent<MeshRenderer>().material = flashMaterials[0];
            yield return new WaitForSeconds(0.1f);
            body.GetComponent<MeshRenderer>().materials = initialMaterialsBody;
            wing1.GetComponent<MeshRenderer>().material = initialMaterialWings;
            wing2.GetComponent<MeshRenderer>().material = initialMaterialWings;
            yield return new WaitForSeconds(0.9f);
        }
        IEnumerator DeathAnimation()
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            body.GetComponent<MeshRenderer>().materials = flashMaterials;
            wing1.GetComponent<MeshRenderer>().material = flashMaterials[0];
            wing2.GetComponent<MeshRenderer>().material = flashMaterials[0];
            yield return new WaitForSeconds(0.1f);
            if (difficulty == 1)
                player.GetComponent<PlayerController>().score += 50;
            else if (difficulty == 2)
                player.GetComponent<PlayerController>().score += 100;
            else
                player.GetComponent<PlayerController>().score += 200;
            body.GetComponent<MeshRenderer>().enabled = false;
            wing1.GetComponent<MeshRenderer>().enabled = false;
            wing2.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
        }
    }
    void SetDifficulty()
    {
        if (Time.timeSinceLevelLoad >= 240 && Time.timeSinceLevelLoad < 480)
        {
            maxDifficulty = 2;
        }
        else if (Time.timeSinceLevelLoad >= 480 || Difficulty.isNewGamePlus)
        {
            maxDifficulty = 3;
        }
        difficulty = Random.Range(1, maxDifficulty + 1);
    }
    void ChangeMaterials()
    {
        if(difficulty == 2)
        {
            body.GetComponent<MeshRenderer>().materials = mediumMaterials;
        }
        if (difficulty == 3)
        {
            body.GetComponent<MeshRenderer>().materials = hardMaterials;
        }
    }
    void ChangeSpeed()
    {
        if (difficulty == 2)
            speed = 1.75f;
        if (difficulty == 3)
            speed = 1.5f;
    }
}
