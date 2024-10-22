using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePowerup : MonoBehaviour
{
    public GameObject lifePickup;
    public GameObject swordPickup;
    public GameObject arrowPickup;
    public bool isLife;
    public bool isSword;
    public bool isArrow;
    public bool isGroundLife;
    private SwordHitbox sword;
    private FollowMouse arrow;
    private PlayerController player;
    public Material[] materials;
    private Material[] newMaterials;
    private MeshRenderer powerupRenderer;
    public bool isSoulHeart = false;
    // Start is called before the first frame update
    void Start()
    {
        sword = GameObject.Find("Sword Hitbox").GetComponent<SwordHitbox>();
        arrow = GameObject.Find("Camera Follow").GetComponent<FollowMouse>();
        player = GameObject.Find("PlayerBody").GetComponent<PlayerController>();
        powerupRenderer = GetComponentInChildren<MeshRenderer>();
        if (isLife)
        {
            if (arrow.arrowCount < 4 && sword.swordDamage < 160 && PlayerController.playerHealth >= 3)
            {
                int range = Random.Range(0, 2);
                if (range == 0)
                {
                    Instantiate(swordPickup, transform.position, swordPickup.transform.rotation);
                    Destroy(gameObject);
                }
                else if (range == 1)
                {
                    Instantiate(arrowPickup, transform.position, arrowPickup.transform.rotation);
                    Destroy(gameObject);
                }
            }
            else if (arrow.arrowCount < 4 && PlayerController.playerHealth >= 3)
            {
                Instantiate(arrowPickup, transform.position, arrowPickup.transform.rotation);
                Destroy(gameObject);
            }
            else if (sword.swordDamage < 160 && PlayerController.playerHealth >= 3)
            {
                Instantiate(swordPickup, transform.position, swordPickup.transform.rotation);
                Destroy(gameObject);
            }
        }
        if (isSword)
        {
            if (sword.swordDamage >= 160 && arrow.arrowCount < 4 && PlayerController.playerHealth < 3)
            {
                int range = Random.Range(0, 2);
                if (range == 0)
                {
                    Instantiate(lifePickup, transform.position, lifePickup.transform.rotation);
                    Destroy(gameObject);
                }
                else if (range == 1)
                {
                    Instantiate(arrowPickup, transform.position, arrowPickup.transform.rotation);
                    Destroy(gameObject);
                }
            }
            else if (sword.swordDamage >= 160 && arrow.arrowCount < 4)
            {
                Instantiate(arrowPickup, transform.position, arrowPickup.transform.rotation);
                Destroy(gameObject);
            }
            else if (sword.swordDamage >= 160 && arrow.arrowCount >= 4)
            {
                Instantiate(lifePickup, transform.position, lifePickup.transform.rotation);
                Destroy(gameObject);
            }

        }
        if (isArrow)
        {
            if (arrow.arrowCount >= 4 && sword.swordDamage < 160 && PlayerController.playerHealth < 3)
            {
                int range = Random.Range(0, 2);
                if (range == 0)
                {
                    Instantiate(lifePickup, transform.position, lifePickup.transform.rotation);
                    Destroy(gameObject);
                }
                else if (range == 1)
                {
                    Instantiate(swordPickup, transform.position, swordPickup.transform.rotation);
                    Destroy(gameObject);
                }
            }
            else if (arrow.arrowCount >= 4 && sword.swordDamage < 160)
            {
                Instantiate(swordPickup, transform.position, swordPickup.transform.rotation);
                Destroy(gameObject);
            }
            else if (sword.swordDamage >= 160 && arrow.arrowCount >= 4)
            {
                Instantiate(lifePickup, transform.position, lifePickup.transform.rotation);
                Destroy(gameObject);
            }

        }
        if(isSword)
        {
            if(sword.swordDamage == 40)
            {
                newMaterials = powerupRenderer.materials;
                newMaterials[1] = materials[0];
                newMaterials[2] = materials[1];
                powerupRenderer.materials = newMaterials;
            }
            if (sword.swordDamage == 80)
            {
                newMaterials = powerupRenderer.materials;
                newMaterials[1] = materials[2];
                newMaterials[2] = materials[3];
                powerupRenderer.materials = newMaterials;
            }
            if (sword.swordDamage == 120)
            {
                newMaterials = powerupRenderer.materials;
                newMaterials[1] = materials[4];
                newMaterials[2] = materials[5];
                powerupRenderer.materials = newMaterials;
            }
        }
        if(isArrow)
        {
            if(arrow.arrowCount == 1)
            {
                newMaterials = powerupRenderer.materials;
                newMaterials[0] = materials[1];
                newMaterials[1] = materials[0];
                newMaterials[2] = materials[0];
                powerupRenderer.materials = newMaterials;
            }
            if (arrow.arrowCount == 2)
            {
                newMaterials = powerupRenderer.materials;
                newMaterials[0] = materials[3];
                newMaterials[1] = materials[2];
                newMaterials[2] = materials[3];
                powerupRenderer.materials = newMaterials;
            }
            if (arrow.arrowCount == 3)
            {
                newMaterials = powerupRenderer.materials;
                newMaterials[0] = materials[5];
                newMaterials[1] = materials[4];
                newMaterials[2] = materials[5];
                powerupRenderer.materials = newMaterials;
            }
        }
    }
    private void LateUpdate()
    {
        if((isLife || isGroundLife) && PlayerController.playerHealth >= 3 && !isSoulHeart)
        {
            powerupRenderer.material = materials[0];
            isSoulHeart = true;
        }
        else if((isLife || isGroundLife) && PlayerController.playerHealth < 3 && isSoulHeart)
        {
            powerupRenderer.material = materials[6];
            isSoulHeart = false;
        }
    }
}
