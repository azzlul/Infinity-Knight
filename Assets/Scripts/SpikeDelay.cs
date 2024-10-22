using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDelay : MonoBehaviour
{
    public GameObject spikes;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spike());
    }

    private IEnumerator Spike()
    {
        yield return new WaitForSeconds(2.5f);
        spikes.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
