using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private BoxCollider hitboxCollider;
    private PlayerController playerController;
    public float swordDamage = 40f;
    public Material[] swordMaterials;
    public GameObject sword;
    public GameObject bow1;
    public GameObject bow2;
    private Animator swordAnimator;
    public bool hasPlayerAttacked;
    private SpawnManager spawnManager;
    public bool cooldown;
    // Start is called before the first frame update
    void Start()
    {
        swordDamage = 40f;
        if (Difficulty.isNewGamePlus)
            swordDamage = 160f;
        hitboxCollider = GetComponent<BoxCollider>();
        playerController = GameObject.Find("PlayerBody").GetComponent<PlayerController>();
        swordAnimator = GameObject.Find("swordAnimator").GetComponent<Animator>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1) && !spawnManager.isGamePaused && !cooldown && !playerController.isPlayerStunned)
        {
            cooldown = true;
            sword.GetComponent<MeshRenderer>().enabled = true;
            bow1.GetComponent<MeshRenderer>().enabled = false;
            bow2.GetComponent<MeshRenderer>().enabled = false;
            swordAnimator.SetBool("swing", true);
            StartCoroutine(Cooldown());          
        }
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.5f);
        swordAnimator.SetBool("swing", false);
        cooldown = false;
    }

}