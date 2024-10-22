using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    private float speed = 10.0f;
    private float groundSpeed = 3.0f;
    private float jumpForce = 700f;
    public float jumpTimer;
    public float maxJumpTimer = 1f;
    private float minJumpTimer = 0.80f;
    public bool isOnGround;
    private Rigidbody playerRb;
    private MeshRenderer playerRenderer;
    private SphereCollider playerCollider;
    public Material flashMaterial;
    public Material hurtMaterial;
    public Material deadMaterial;
    private float xBound = 12f;
    private bool flashCounter = true;
    private float damageSpeed= 150.0f;
    private LayerMask playerLayer = 6;
    private LayerMask enemyLayer = 3;
    public bool isPlayerStunned;
    public static float playerHealth = 3;
    public GameObject player;
    private float deathLayer = -10f;
    public bool test;
    private SwordHitbox sword;
    private FollowMouse arrow;
    private float angle;
    public GameObject sphere;
    public GameObject plane1;
    public GameObject plane2;
    public GameObject helmet;
    public GameObject swordRenderer;
    public GameObject cameraFollow;
    public GameObject swordHitbox;
    private GameObject spawnManager;
    private Material initialMaterialHelmet;
    private Material initialMaterialPlane1;
    private Material initialMaterialPlane2;
    private Material initialMaterialSphere;
    private Material[] newSwordMaterials;
    public GameObject bow;
    public bool isDead = false;
    public GameObject GUI;
    public GameObject gameOverMenu;
    private bool isOnCooldown;
    public int score;
    public TextMeshProUGUI scoreText;
    public bool isGOactive = false;
    public float fallMultiplier = 3.5f;
    private bool jumpCooldown = true;
    private Animator playerAnim;
    public AudioSource HitSound;
    public AudioSource Jump;
    public AudioSource BigJump;
    public AudioSource ppsound;
    void Start()
    {
        playerHealth = 3;
        score = 0;
        scoreText.text = "000000";
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GameObject.Find("Player").GetComponent<Animator>();
        playerRenderer = GetComponent<MeshRenderer>();
        playerCollider = GetComponent<SphereCollider>();
        sword = GameObject.Find("Sword Hitbox").GetComponent<SwordHitbox>();
        arrow = GameObject.Find("Camera Follow").GetComponent<FollowMouse>();
        spawnManager = GameObject.Find("Spawn Manager");
        initialMaterialHelmet = helmet.GetComponent<MeshRenderer>().material;
        initialMaterialPlane1 = plane1.GetComponent<MeshRenderer>().material;
        initialMaterialSphere = sphere.GetComponent<MeshRenderer>().material;
        initialMaterialPlane2 = plane2.GetComponent<MeshRenderer>().material;
        ChangeMaterialsNewGamePlus();
    }

    
    void Update()
    {
        if (!isDead)
        {
            PlayerJump();
            ChargeJump();
            UpdateScore();          
            ConstrainPlayer();
            FollowMousePlayer();
            PlayerFall();
        }
        DestroyOutOfBounds();
        KeepDeadFace();
        CheckForCooldown();
    }
    private void FixedUpdate()
    {
        if (!isDead)
        {
            CheckForFalling();
            MovePlayer();
            KillPlayer();
        }      
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("ShieldPlatform"))
        {
            isOnGround = true;
            transform.parent.SetParent(collision.gameObject.transform);
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            if (!isOnCooldown)
            {
                if (HitSound.isPlaying == false)
                    HitSound.Play();
                playerHealth--;
                isOnCooldown = true;
                playerRb.AddForce(new Vector3(Mathf.Sign(transform.position.x - collision.gameObject.transform.position.x), 1, 0) * damageSpeed, ForceMode.Impulse);
                if (playerHealth != 0)
                {
                    StartCoroutine(IFrames(collision));
                }
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("ShieldPlatform"))
        {
            transform.parent.SetParent(collision.gameObject.transform);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("ShieldPlatform"))
        {
            transform.parent.parent = null;
        }
    }
    IEnumerator IFrames(Collision collision)
    {
        isPlayerStunned = true;
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        sphere.GetComponent<MeshRenderer>().material = hurtMaterial;
        yield return new WaitForSeconds(0.5f);
        isPlayerStunned = false;
        float waitTime = 2.0f;
        float timeElapsed = 0f;
        while (timeElapsed <= waitTime)
        {
            helmet.GetComponent<MeshRenderer>().enabled = false;
            plane1.GetComponent<MeshRenderer>().enabled = false;
            sphere.GetComponent<MeshRenderer>().enabled = false;
            plane2.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            helmet.GetComponent<MeshRenderer>().enabled = true;
            plane1.GetComponent<MeshRenderer>().enabled = true;
            sphere.GetComponent<MeshRenderer>().enabled = true;
            plane2.GetComponent<MeshRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            timeElapsed += 0.2f;
        }
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        if (!isDead)
        {
            sphere.GetComponent<MeshRenderer>().material = initialMaterialSphere;
        }
        isOnCooldown = false;
    }

    void PlayerJump()
    {
        if (!isPlayerStunned)
        {
            if (Input.GetKey(KeyCode.Space) && isOnGround && jumpCooldown)
            {   
                jumpCooldown = false;
                isOnGround = false;              
                if (jumpTimer >= maxJumpTimer)
                    jumpTimer = maxJumpTimer;
                else
                    jumpTimer = minJumpTimer;
                if (jumpTimer == minJumpTimer) Jump.Play();
                else BigJump.Play();
                float force = jumpForce * jumpTimer;
                playerRb.AddForce(Vector3.up * force, ForceMode.Impulse);
                jumpTimer = 0;
                flashCounter = true;
                StartCoroutine(JumpCooldown());
            }
        }
    }
    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.75f);
        jumpCooldown = true;
    }
    void ChargeJump()
    {
        if (!isPlayerStunned)
        {
            if (Input.GetKey(KeyCode.W) && isOnGround)
            {
                jumpTimer += Time.deltaTime;
                if (jumpTimer >= maxJumpTimer && flashCounter)
                {
                    StartCoroutine(Flash());
                    flashCounter = false;
                }
            }
            if (Input.GetKeyUp(KeyCode.W) && jumpTimer <= maxJumpTimer)
            {
                jumpTimer = 0;
            }
        }
    }
    void ConstrainPlayer ()
    {
        if (transform.position.x < -xBound)
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        if (transform.position.x > xBound)
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
    }
    IEnumerator Flash()
    {
        helmet.GetComponent<MeshRenderer>().material = flashMaterial;
        sphere.GetComponent<MeshRenderer>().material = flashMaterial;
        plane1.GetComponent<MeshRenderer>().material = flashMaterial;
        plane2.GetComponent<MeshRenderer>().material = flashMaterial;
        yield return new WaitForSeconds(0.2f);
        helmet.GetComponent<MeshRenderer>().material = initialMaterialHelmet;
        sphere.GetComponent<MeshRenderer>().material = initialMaterialSphere;
        plane1.GetComponent<MeshRenderer>().material = initialMaterialPlane1;
        plane2.GetComponent<MeshRenderer>().material = initialMaterialPlane2;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bottom Border"))
        {
            test = true;
            Physics.IgnoreCollision(GetComponent<SphereCollider>(), other.gameObject.transform.parent.GetComponent<BoxCollider>(), true);
        }
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Enemy Projectile") || other.gameObject.CompareTag("Spike"))
        {
            if (!isOnCooldown)
            {
                if (HitSound.isPlaying == false)
                    HitSound.Play();
                playerHealth--;
                isOnCooldown = true;
                playerRb.AddForce(new Vector3(Mathf.Sign(transform.position.x - other.gameObject.transform.position.x), 1, 0) * damageSpeed, ForceMode.Impulse);
                if (playerHealth != 0)
                {
                    StartCoroutine(IFrames2(other));
                }
            }
        }
        if (other.gameObject.CompareTag("Life"))
        {
            ppsound.Play();
            playerHealth++;
            score += 1000;
            if (playerHealth > 6)
            {
                playerHealth = 6;
                score += 4000;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("SwordBuff"))
        {
            ppsound.Play();
            sword.swordDamage = sword.swordDamage + 40f;
            score += 1000;
            if(sword.swordDamage == 80f)
            {
                newSwordMaterials = swordRenderer.GetComponent<MeshRenderer>().materials;
                newSwordMaterials[1] = sword.swordMaterials[0];
                newSwordMaterials[3] = sword.swordMaterials[1];
                newSwordMaterials[2] = sword.swordMaterials[0];
                newSwordMaterials[0] = sword.swordMaterials[1];
                swordRenderer.GetComponent<MeshRenderer>().materials = newSwordMaterials;
            }
            if (sword.swordDamage == 120f)
            {
                newSwordMaterials = swordRenderer.GetComponent<MeshRenderer>().materials;
                newSwordMaterials[1] = sword.swordMaterials[2];
                newSwordMaterials[3] = sword.swordMaterials[3];
                newSwordMaterials[2] = sword.swordMaterials[2];
                newSwordMaterials[0] = sword.swordMaterials[3];
                swordRenderer.GetComponent<MeshRenderer>().materials = newSwordMaterials;
            }
            if (sword.swordDamage == 160f)
            {
                newSwordMaterials = swordRenderer.GetComponent<MeshRenderer>().materials;
                newSwordMaterials[1] = sword.swordMaterials[4];
                newSwordMaterials[3] = sword.swordMaterials[5];
                newSwordMaterials[2] = sword.swordMaterials[4];
                newSwordMaterials[0] = sword.swordMaterials[5];
                swordRenderer.GetComponent<MeshRenderer>().materials = newSwordMaterials;
            }
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("ArrowBuff"))
        {
            ppsound.Play();
            arrow.arrowCount++;
            score += 1000;
            if (arrow.arrowCount == 2)
            {
                bow.GetComponent<MeshRenderer>().material = sword.swordMaterials[0];
            }
            if (arrow.arrowCount == 3)
            {
                bow.GetComponent<MeshRenderer>().material = sword.swordMaterials[2];
            }
            if (arrow.arrowCount == 4)
            {
                bow.GetComponent<MeshRenderer>().material = sword.swordMaterials[5];
            }
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("JumpBorder"))
        {
            isOnGround = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Bottom Border"))
        {
            Physics.IgnoreCollision(GetComponent<SphereCollider>(), other.gameObject.transform.parent.GetComponent<BoxCollider>(), false);
        }
    }
    IEnumerator IFrames2(Collider other)
    {
        isPlayerStunned = true;
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        sphere.GetComponent<MeshRenderer>().material = hurtMaterial;
        yield return new WaitForSeconds(0.5f);
        isPlayerStunned = false;
        float waitTime = 2.0f;
        float timeElapsed = 0f;
        while (timeElapsed <= waitTime)
        {
            helmet.GetComponent<MeshRenderer>().enabled = false;
            plane1.GetComponent<MeshRenderer>().enabled = false;
            sphere.GetComponent<MeshRenderer>().enabled = false;
            plane2.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            helmet.GetComponent<MeshRenderer>().enabled = true;
            plane1.GetComponent<MeshRenderer>().enabled = true;
            sphere.GetComponent<MeshRenderer>().enabled = true;
            plane2.GetComponent<MeshRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            timeElapsed += 0.2f;
        }
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        sphere.GetComponent<MeshRenderer>().material = initialMaterialSphere;
        isOnCooldown = false;
    }
    void CheckForFalling()
    {
        if (playerRb.velocity.y < -1 || playerRb.velocity.y > 1)
        {
            isOnGround = false;
        }
        else if (playerRb.velocity.y == 0)
        {
            isOnGround = true;
        }
    }
    void MovePlayer()
    {
        if (!isPlayerStunned && ((Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) || (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))))
{
                float horizontalInput = Input.GetAxis("Horizontal");
                if (horizontalInput < 0 && isOnGround)
                    transform.Translate((Vector3.right * -groundSpeed * horizontalInput * Time.deltaTime), Space.World);
                else if (horizontalInput > 0 && isOnGround)
                    transform.Translate((Vector3.right * groundSpeed * horizontalInput * Time.deltaTime), Space.World);
                transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime, Space.World);
        }
    }
    void KillPlayer()
    {
        if (playerHealth <= 0)
        {
            sphere.GetComponent<MeshRenderer>().material = deadMaterial;
            playerCollider.enabled = false;
            playerRb.velocity = new Vector3(0, 0, 0);
            playerRb.AddForce(Vector3.up * jumpForce / 4, ForceMode.Impulse);
            isDead = true;
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            spawnManager.GetComponent<SpawnManager>().UpdateStatsGameOver();
            cameraFollow.SetActive(false);
        }
        if (transform.position.y < deathLayer)
        {
            sphere.GetComponent<MeshRenderer>().material = deadMaterial;
            playerCollider.enabled = false;
            playerRb.velocity = new Vector3(0, 0, 0);
            playerRb.AddForce(Vector3.up * jumpForce * 1.25f, ForceMode.Impulse);
            isDead = true;
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            spawnManager.GetComponent<SpawnManager>().UpdateStatsGameOver();
            cameraFollow.SetActive(false);
        }
    }
    void FollowMousePlayer()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (angle >= 90 || angle <= -90)
            transform.rotation = Quaternion.Euler(new Vector3(angle+180, -140, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(-angle, 140, 0));
    }
    void DestroyOutOfBounds()
    {
        if(transform.position.y < deathLayer && isDead)
        {
            isGOactive = true;
            if (PPMODE.isPPModeOn) spawnManager.GetComponent<SpawnManager>().PPModeaudio.Play();
            else spawnManager.GetComponent<SpawnManager>().GameOverAudio.Play();
            Destroy(gameObject);
        }
    }
    void KeepDeadFace()
    {
        if(isDead)
        {
            sphere.GetComponent<MeshRenderer>().material = deadMaterial;
        }
    }
    void UpdateScore()
    {
        if (score >= 0 && score <= 9)
        {
            scoreText.text = "00000"+ score;
        }
        else if (score >= 10 && score <= 99)
        {
            scoreText.text = "0000" + score;
        }
        else if (score >= 100 && score <= 999)
        {
            scoreText.text = "000" + score;
        }
        else if (score >= 1000 && score <= 9999)
        {
            scoreText.text = "00" + score;
        }
        else if (score >= 10000 && score <= 99999)
        {
            scoreText.text = "0" + score;
        }
        else if (score >= 100000 && score <= 999999)
        {
            scoreText.text = "" + score;
        }
        else
        {
            score = 999999;
        }
    }
    void ChangeMaterialsNewGamePlus()
    {
        if(Difficulty.isNewGamePlus)
        {
            newSwordMaterials = swordRenderer.GetComponent<MeshRenderer>().materials;
            newSwordMaterials[1] = sword.swordMaterials[4];
            newSwordMaterials[3] = sword.swordMaterials[5];
            newSwordMaterials[2] = sword.swordMaterials[4];
            newSwordMaterials[0] = sword.swordMaterials[5];
            swordRenderer.GetComponent<MeshRenderer>().materials = newSwordMaterials;
            bow.GetComponent<MeshRenderer>().material = sword.swordMaterials[5];
            playerHealth = 6;
        }
    }
    void CheckForCooldown()
    {
        if(!isOnCooldown)
        {
            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        }
    }
    void PlayerFall()
    {
        if(Input.GetKey(KeyCode.S) && !isOnGround)
        {
            //playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
           playerRb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }
}
