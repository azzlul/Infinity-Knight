using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public Animator animator;
    private Animator logoAnimator;
    private bool isOn = true;
    public GameObject anyKeyText;
    private static bool hasIntroPlayed;
    private void Awake()
    {
        logoAnimator = GameObject.Find("Logo").GetComponent<Animator>();
        if (!hasIntroPlayed)
        {
            StartCoroutine(WaitTime());
        }
    }
    private void Update()
    {
        logoAnimator.SetBool("hasIntroPlayed", hasIntroPlayed);
        if(Input.anyKey && isOn && hasIntroPlayed)
        {
            animator.SetTrigger("anyKey");
            anyKeyText.SetActive(false);
            isOn = false;
        }
    }
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(3f);
        hasIntroPlayed = true;
    }
}
