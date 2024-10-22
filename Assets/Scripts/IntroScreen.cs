using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    public Animator animator;
    public Animator transitionAnimator;
    private static bool hasIntroPlayed = false;

    private void Awake()
    {
        animator.SetBool("hasIntroPlayed", hasIntroPlayed);
        if(hasIntroPlayed == true)
        {
            transitionAnimator.SetTrigger("return");
        }
    }
    public void CheckIntroEnd()
    {
        hasIntroPlayed = true;
    }
}
