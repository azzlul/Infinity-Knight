using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;
    private float leftSpeed = 4.0f;
    private float rightSpeed = -2.0f;
    private Rigidbody slimeRb;
    [SerializeField] private int direction = -1;
    private float yBound = -10f;
    public float slimeHealth;
    private float initialSlimeHealth = 80f;
    private SwordHitbox sword;
    private FollowMouse arrow;
    public Material flashMaterial;
    public Material initialMaterial;
    private MeshRenderer slimeRenderer;
    private LayerMask enemyLayer = 3;
    private bool isSlimeStunned;
    public GameObject slimeAnimator;
    float currentSpeed;
    public Material mediumMaterial;
    public Material hardMaterial;
    public Material attackMaterial;
    [SerializeField] private int difficulty = 1;
    private int maxDifficulty = 1;
    private GameObject player;
    public GameObject projectile;
    private float spawnDelay = 4f;
    private float spawnInterval = 4f;
    [SerializeField] private bool manualDifficulty = false;
    public Animator slimeAnim;
    public AudioSource HitSound;
    private bool shouldShoot = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerBody");
        slimeAnimator.GetComponent<Animator>().SetBool("slimeBounce", true);
        slimeRb = GetComponent<Rigidbody>();
        sword = GameObject.Find("Sword Hitbox").GetComponent<SwordHitbox>();
        arrow = GameObject.Find("Camera Follow").GetComponent<FollowMouse>();
        Physics.IgnoreLayerCollision(enemyLayer, enemyLayer, true);
        slimeRenderer = slimeAnimator.GetComponent<MeshRenderer>();
        if(!manualDifficulty)
        SetDifficulty();
        ChangeMaterials();
        ChangeSpeed();
        if(difficulty == 3)
            InvokeRepeating("ShootAtPlayer", spawnDelay, spawnInterval);
        initialMaterial = slimeRenderer.material;
        slimeHealth = initialSlimeHealth + (int)(Time.timeSinceLevelLoad / 3);
        if (slimeHealth > 320 || Difficulty.isNewGamePlus)
            slimeHealth = 320;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckDirection();
    }
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Left Border") && !(difficulty == 2))
        {
            direction = 1;
            speed = rightSpeed;
            transform.rotation = Quaternion.Euler(-90, -135, 0);
        }
        if (other.gameObject.CompareTag("Right Border") && !(difficulty == 2))
        {
            direction = -1;
            speed = leftSpeed;
            transform.rotation = Quaternion.Euler(-90, -45, 0);
        }
        if (other.gameObject.CompareTag("Sword"))
        {
            slimeHealth -= sword.swordDamage;
            if (HitSound.isPlaying == false)
                HitSound.Play();
            if (slimeHealth <= 0)
            {
                StartCoroutine(DeathAnimation(other));
            }
            else
            {
                StartCoroutine(IFrames(other));
            }
        }
        if (other.gameObject.CompareTag("Arrow") || other.gameObject.CompareTag("Spike"))
        {
            slimeHealth -= arrow.arrowDamage;
            if (HitSound.isPlaying == false)
                HitSound.Play();
            if (slimeHealth <= 0)
            {
                StartCoroutine(DeathAnimation(other));
            }
            else
            {
                StartCoroutine(IFrames(other));
            }
        }
    }
    IEnumerator IFrames(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            currentSpeed = speed;
            speed = 0;
            if (Mathf.Sign(transform.position.x - other.transform.parent.position.x) == 1)
            {
                slimeRb.AddForce(new Vector3(Mathf.Sign(transform.position.x - other.transform.parent.position.x), 0, 0) * 3, ForceMode.Impulse);
            }
            if (Mathf.Sign(transform.position.x - other.transform.parent.position.x) == -1)
            {
                slimeRb.AddForce(new Vector3(Mathf.Sign(transform.position.x - other.transform.parent.position.x), 0, 0) * 5, ForceMode.Impulse);
            }
            isSlimeStunned = true;
        }
        slimeRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        isSlimeStunned = false;
        slimeRenderer.material = initialMaterial;
        speed = currentSpeed;
    }
    IEnumerator DeathAnimation(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            slimeRb.AddForce(new Vector3(Mathf.Sign(transform.position.x - other.transform.parent.position.x), 0, 0) * 3, ForceMode.Impulse);
            isSlimeStunned = true;
        }
        gameObject.GetComponent<BoxCollider>().enabled = false;
        shouldShoot = false;
        slimeRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        if(difficulty == 1)
        player.GetComponent<PlayerController>().score += 50;
        else if(difficulty == 2)
            player.GetComponent<PlayerController>().score += 100;
        else
            player.GetComponent<PlayerController>().score += 200;
        slimeRenderer.enabled = false;
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
    void CheckDirection()
    {
        if(direction == 1)
        {
            speed = rightSpeed;
        }
        else
        {
            speed = leftSpeed;
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
        if (difficulty == 2)
        {
            slimeRenderer.material = mediumMaterial;
        }
        if (difficulty == 3)
        {
            slimeRenderer.material = hardMaterial;
        }
    }
    void Movement()
    {
        if (difficulty == 1 || difficulty == 3)
        {
            if (!isSlimeStunned)
                slimeRb.MovePosition(transform.position + new Vector3(direction, 0, 0) * speed * Time.deltaTime);
        }
     if(difficulty == 2)
        {
            if (!isSlimeStunned)
            {
                if (Mathf.Sign(player.transform.position.x - transform.position.x) == -1)
                {
                    slimeRb.MovePosition(transform.position + new Vector3(Mathf.Sign(player.transform.position.x - transform.position.x), 0, 0) * leftSpeed * Time.fixedDeltaTime);
                    transform.rotation = Quaternion.Euler(-90, -45, 0);
                }
                if (Mathf.Sign(player.transform.position.x - transform.position.x) == 1)
                {
                    slimeRb.MovePosition(transform.position + new Vector3(Mathf.Sign(player.transform.position.x - transform.position.x), 0, 0) * rightSpeed * Time.fixedDeltaTime);
                    transform.rotation = Quaternion.Euler(-90, -135, 0);
                }
                
            }
        }
        if (transform.position.y < yBound)
            Destroy(gameObject);
    }
    void ChangeSpeed()
    {
        if(difficulty == 2)
        {
            leftSpeed = 4.5f;
            rightSpeed = 1.5f;
        }
    }
    void ShootAtPlayer()
    {
        if (difficulty == 3)
        {
            slimeAnim.SetTrigger("attack");
        }
    }
    public void Shoot()
    {
        if(shouldShoot)Instantiate(projectile, transform.position, projectile.transform.rotation);
    }
        
}
