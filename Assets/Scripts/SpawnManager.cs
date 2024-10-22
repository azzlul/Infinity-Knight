using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;
public class SpawnManager : MonoBehaviour
{
    public GameObject[] groundPrefabs;
    public GameObject[] rareGroundPrefabs;
    public GameObject[] powerupPrefabs;
    private float spawnPosXGround = 25f;
    private float spawnPosXFly = 20f;
    private float spawnPosYFly = 5.5f;
    private float spawnPosPupX = 12f;
    private float spawnPosPupy = 5f;
    private float spawnDelayGround = 0;
    private float spawnIntervalGround = 8.32f;
    private float spawnIntervalEnemy = 16.64f;
    private float spawnDelayPowerup = 80f;
    private float spawnIntervalPowerup = 80f;
    public GameObject flyPrefab;
    public GameObject slimePrefab;
    public GameObject playerBody;
    public GameObject menuSound;
    private SwordHitbox sword;
    private FollowMouse arrow;
    private PlayerController player;
    private bool areEnemiesSpawned;
    private int debugPupCount;
    public bool isGamePaused;
    public bool isDead;
    private int cmnGroundSpwnNr = 1;
    private int difficulty = 2;
    public TextMeshProUGUI timerText;
    public GameObject GUI;
    public GameObject pauseScreen;
    private int score;
    public TextMeshProUGUI scoreName;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI proTipText;
    public Animator loadLevelAnimator;
    public GameObject gameOverMenu;
    private int random1, random2;
    public AudioSource PPModeaudio;
    public AudioSource GameOverAudio;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LevelStartSequence());
        InvokeRepeating("SpawnRandomGround", spawnDelayGround, spawnIntervalGround);
        InvokeRepeating("SpawnRandomPowerup", spawnDelayPowerup, spawnIntervalPowerup);
        sword = GameObject.Find("Sword Hitbox").GetComponent<SwordHitbox>();
        arrow = GameObject.Find("Camera Follow").GetComponent<FollowMouse>();
        player = GameObject.Find("PlayerBody").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        isDead = player.isDead;
        if(!areEnemiesSpawned)
        {
            StartCoroutine(SpawnEnemies());
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !isDead)
            PauseGame();
        GameOverSequence();
    }
    private void FixedUpdate()
    {
        if(!isGamePaused && !player.isDead)
        UpdateTimer();
    }
    IEnumerator SpawnEnemies()
    {
        areEnemiesSpawned = true;
        float waitTime = (spawnIntervalEnemy - (Time.timeSinceLevelLoad / 57.69f)) / 2;
        if (Time.timeSinceLevelLoad >= 720f || Difficulty.isNewGamePlus)
            waitTime = 4.16f;
        int range = Random.Range(0, 2);
        if(range == 0)
        {
            SpawnFly();
        }    
        else
        {
            SpawnSlime();
        }
        yield return new WaitForSeconds(waitTime);
       
        areEnemiesSpawned = false;
    }
    void SpawnRandomGround()
    {
        if (Time.timeSinceLevelLoad >= 240f)
        {
            difficulty = 3;
        }
        if (Time.timeSinceLevelLoad >= 720f || Difficulty.isNewGamePlus)
        {
            difficulty = 4;
        }
        if (cmnGroundSpwnNr <= 4 - difficulty)
        {
            int groundIndex = Random.Range(0, groundPrefabs.Length);
            Vector3 spawnPos = new Vector3(spawnPosXGround, 0, 0);
            Instantiate(groundPrefabs[groundIndex], spawnPos, groundPrefabs[groundIndex].transform.rotation);
            cmnGroundSpwnNr++;
        }
        else
        {
            int groundIndex = Random.Range(0, rareGroundPrefabs.Length);
            Vector3 spawnPos = new Vector3(spawnPosXGround, 0, 0);
            Instantiate(rareGroundPrefabs[groundIndex], spawnPos, rareGroundPrefabs[groundIndex].transform.rotation);
            cmnGroundSpwnNr = 1;
        }
    }
    void SpawnFly()
    {
        Vector3 spawnPos = new Vector3(spawnPosXFly, spawnPosYFly, 0);
        Instantiate(flyPrefab, spawnPos, flyPrefab.transform.rotation);
    }
    void SpawnSlime()
    {
        Vector3 spawnPos = new Vector3(spawnPosXFly, -2, 0);
        Instantiate(slimePrefab, spawnPos, slimePrefab.transform.rotation);
    }
    void SpawnRandomPowerup()
    {
        int powerupIndex = Random.Range(0, powerupPrefabs.Length);
        Vector3 spawnPos = new Vector3(spawnPosPupX, spawnPosPupy, 0);
        Instantiate(powerupPrefabs[powerupIndex], spawnPos, powerupPrefabs[powerupIndex].transform.rotation);
        debugPupCount++;
    }
    public void PauseGame()
    {
            isGamePaused = !isGamePaused;
            menuSound.gameObject.GetComponent<AudioSource>().Play();
        if (isGamePaused && Time.timeSinceLevelLoad >0f)
            {
                Time.timeScale = 0;
                pauseScreen.SetActive(true);
                GUI.SetActive(false);
            }
            else if (!player.isDead && Time.timeSinceLevelLoad > 0f)
            {
                Time.timeScale = 1;
                pauseScreen.SetActive(false);
                GUI.SetActive(true);
            }       
    }
       public void RestartGame()
    {
        StartCoroutine(Restart());          
    }
    IEnumerator Restart()
    {
        loadLevelAnimator.SetTrigger("startLevel");
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene("Game");
    }
    void UpdateTimer()
    {
        int minutes = (int)(Time.timeSinceLevelLoad / 60f);
        int seconds = (int)(Time.timeSinceLevelLoad % 60);
        if(minutes >=0 && minutes <=9 && seconds >= 0 && seconds <= 9)
        timerText.text = "0" + minutes + ":" + "0" + seconds;
        else if(minutes >= 0 && minutes <= 9)
        timerText.text = "0" + minutes + ":" + seconds;
        else if(seconds >= 0 && seconds <= 9)
        timerText.text = minutes + ":" + "0" + seconds;
        else
        timerText.text = minutes + ":" + seconds;
        if ((Time.timeSinceLevelLoad >= 720f && Time.timeSinceLevelLoad <= 1800) || Difficulty.isNewGamePlus)
        {
            timerText.color = new Color32(255, 255, 0, 255);
            SaveManager.instance.activeSave.hasUnlockedNGPlus = true;
            SaveManager.instance.Save();
        }
        if (Time.timeSinceLevelLoad >= 1800)
            timerText.color = new Color32(255, 0, 0, 255);
    }
    void GameOverSequence()
    {
        if(playerBody == null)
        {
            PlayerController.playerHealth = 0;
            Time.timeScale = 0;
            GUI.SetActive(false);
            gameOverMenu.SetActive(true);
        }
    }
    public void UpdateStatsGameOver()
    {
        if (PPMODE.isPPModeOn)
        {
            random1 = Random.Range(1, 9);
            switch (random1)
            {
                case 1: proTipText.text = "1.There once was a great mage. After being given the power of creation, they sought to create infinity."; break;
                case 2: proTipText.text = "2.They made a small world, confined in a box. In that world, the mage made ground, sky, sun, trees, monsters, and a knight."; break;
                case 3: proTipText.text = "3.The knight was meant to roam those lands for eternity, slaying monsters again, and again."; break;
                case 4: proTipText.text = "4.But the mage foresaw the eventual decay of their creation, the knight one day succumbing to the weight of infinity."; break;
                case 5: proTipText.text = "5.The design was deemed a failure, and was cast away in the shadows of their workshop.";break;
                case 6: proTipText.text = "6.But unbeknownst to the mage, a guiding, golden light shines over the knight."; break;
                case 7: proTipText.text = "7.He will resist his creator's curse, and he will break out of his eternal prison..."; break;
                case 8: proTipText.text = "8...because there is no such thing as infinity."; break;
            }
        }
        else if (Time.timeSinceLevelLoad >= 720 && !SaveManager.instance.activeSave.ppModeMessage)
        {
            proTipText.text = "Congratulations on reaching 12 minutes! Write PPMODE in the title screen for a fun easter egg! :)";
            SaveManager.instance.activeSave.ppModeMessage = true;
            SaveManager.instance.Save();
        }
        else
        {
            random1 = Random.Range(1, 1001);
            if (random1 >= 10)
            {
                random2 = Random.Range(1, 17);
                switch (random2)
                {
                    case 1: proTipText.text = "A power-up will spawn every 80 seconds."; break;
                    case 2: proTipText.text = "If your health is full, spawned power-ups will only be weapon ugrades."; break;
                    case 3: proTipText.text = "Certain ground layouts contain hearts."; break;
                    case 4: proTipText.text = "Hearts that spawn from ground layouts become blue hearts if your health is full."; break;
                    case 5: proTipText.text = "Blue hearts may spawn as power-ups if you have all weapon upgrades and your health is full."; break;
                    case 6: proTipText.text = "You can have up to 3 blue hearts."; break;
                    case 7: proTipText.text = "If you pick up a blue heart while having 3 blue hearts, you get a lot of score."; break;
                    case 8: proTipText.text = "Weapons have 4 tiers: wood, iron, gold and obsidian."; break;
                    case 9: proTipText.text = "Falling in a pit results in an instant game over."; break;
                    case 10: proTipText.text = "After 4 minutes and 8 minutes, more types of enemies can appear."; break;
                    case 11: proTipText.text = "Enemy health increases continuously, until it reaches max at 12 minutes."; break;
                    case 12: proTipText.text = "Possible ground layouts become harder as time goes on, reaching max difficulty at 12 minutes."; break;
                    case 13: proTipText.text = "Charge your jump when you have the opportunity."; break;
                    case 14: proTipText.text = "Making leaps of faith without waiting for the screen scroll is a bad idea."; break;
                    case 15: proTipText.text = "Reaching 12 minutes unlocks new game+."; break;
                    case 16: proTipText.text = "You can store your charged jumps for later use."; break;
                    case 17: proTipText.text = "Grey platforms block enemy projectiles."; break;
                }
            }
            else
            {
                random2 = Random.Range(1, 21);
                switch (random2)
                {
                    case 1: proTipText.text = "Have you locked your door?"; break;
                    case 2: proTipText.text = "Accomplish your dreams."; break;
                    case 3: proTipText.text = "Give up on your dreams."; break;
                    case 4: proTipText.text = "Behind you."; break;
                    case 5: proTipText.text = "Jump in a pit to instantly win the game."; break;
                    case 6: proTipText.text = "There is no escape."; break;
                    case 7: proTipText.text = "infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity infinity."; break;
                    case 8: proTipText.text = "Go play something else."; break;
                    case 9: proTipText.text = "What is the point?"; break;
                    case 10: proTipText.text = "Every 60 seconds in the game, a minute passes."; break;
                    case 11: proTipText.text = "There are two types of enemies in the game, because I'm too lazy to make more."; break;
                    case 12: proTipText.text = "Sorry for the music."; break;
                    case 13: proTipText.text = "How did you find this?"; break;
                    case 14: proTipText.text = "Will I make everything I want to make?"; break;
                    case 15: proTipText.text = "Have you found my secret?"; break;
                    case 16: proTipText.text = "At 30 minutes, something interesting happens..."; break;
                    case 17: proTipText.text = "The Queen wars did not end in a victory."; break;
                    case 18: proTipText.text = "The princess is the castle."; break;
                    case 19: proTipText.text = "The last angel died centuries ago."; break;
                    case 20: proTipText.text = "There once was a dungeon atop a hill..."; break;
                }
            }
        }
        score = player.score;
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
        if(!Difficulty.isNewGamePlus)
        {
            if(score > SaveManager.instance.activeSave.hiScore)
            {
                SaveManager.instance.activeSave.hiScore = score;
                SaveManager.instance.Save();
                scoreName.text = "NEW HIScore:";
                StartCoroutine(HIScore());
            }
        }
        if (Difficulty.isNewGamePlus)
        {
            if (score > SaveManager.instance.activeSave.hiScoreNGPlus)
            {
                SaveManager.instance.activeSave.hiScoreNGPlus = score;
                SaveManager.instance.Save();
                scoreName.text = "NEW HIScore:";
                StartCoroutine(HIScore());
            }
        }
    }
    IEnumerator HIScore()
    {
        while(true)
        {
            scoreName.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.3f);
            scoreName.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }
    IEnumerator LevelStartSequence()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(4.5f);
        Time.timeScale = 1;
    }
}
