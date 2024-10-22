using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearingPlatform : MonoBehaviour
{
    public Material transparentMaterial;
    private Material initialMaterial;
    private BoxCollider platformCollider;
    private MeshRenderer platformRenderer;
    private float delayTime;
    private bool isDelayed;
    [SerializeField] private bool isSecond;
    // Start is called before the first frame update
    void Start()
    {
        
        platformCollider = GetComponent<BoxCollider>();
        platformRenderer = GetComponent<MeshRenderer>();
        initialMaterial = platformRenderer.material;
        StartCoroutine(Dissapear());
    }
    private IEnumerator Dissapear()
    {
        yield return new WaitForEndOfFrame();
        delayTime = GetComponentInParent<SetRandomVariableDissapear>().delayTime;
        isDelayed = GetComponentInParent<SetRandomVariableDissapear>().isDelayed;
        if (isDelayed && !isSecond)
        {
            yield return new WaitForSeconds(3.2f + delayTime);
        }
        else if(!isDelayed && isSecond)
        {
            yield return new WaitForSeconds(3.2f + delayTime);
        }
        else
        {
            yield return new WaitForSeconds(delayTime);
        
        }
        float waitTime = 1.20f;
        float elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            platformRenderer.material = initialMaterial;
            yield return new WaitForSeconds(0.2f);
            platformRenderer.material = transparentMaterial;
            yield return new WaitForSeconds(0.2f);
            elapsedTime += 0.4f;
        }
        platformCollider.enabled = false;
        yield return new WaitForSeconds(3.2f);
        while (true)
        {
            platformRenderer.material = initialMaterial;
            platformCollider.enabled = true;
            yield return new WaitForSeconds(2f);
            waitTime = 1.20f;
            elapsedTime = 0f;
            while(elapsedTime < waitTime)
            {   platformRenderer.material = initialMaterial;                
                yield return new WaitForSeconds(0.2f);
                platformRenderer.material = transparentMaterial;
                yield return new WaitForSeconds(0.2f);
                elapsedTime += 0.4f;
            }
            platformCollider.enabled = false;
            yield return new WaitForSeconds(3.2f);          
        }
        
    }
}
