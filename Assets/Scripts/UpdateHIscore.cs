using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateHIscore : MonoBehaviour
{
    private int score;
    private TextMeshProUGUI scoreText;
    public bool isNGPlus;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isNGPlus)
        {
            score = SaveManager.instance.activeSave.hiScore;

            if (score >= 0 && score <= 9)
            {
                scoreText.text = "00000" + score;
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
        else if (isNGPlus)
        {
            score = SaveManager.instance.activeSave.hiScoreNGPlus;

            if (score >= 0 && score <= 9)
            {
                scoreText.text = "00000" + score;
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
    }

}
