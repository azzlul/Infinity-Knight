using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public static bool isNewGamePlus;

    public void ChangeDifficultyNGP()
    {
        isNewGamePlus = true;
    }
    public void ChangeDifficultyNG()
    {
        isNewGamePlus = false;
    }
}
