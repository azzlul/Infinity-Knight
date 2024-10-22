using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public bool isNGPlus;
    public GameObject levelTransition;
    public GameObject music1, music2;
    public GameObject logo, loadText;
    private void Update()
    {
        if (isNGPlus && !SaveManager.instance.activeSave.hasUnlockedNGPlus)
            GetComponent<Button>().interactable = false;
        else
            GetComponent<Button>().interactable = true;
    }
    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    public void ReturnToTitleScreen()
    {
        StartCoroutine(ReturnToTitleScreenCoroutine());
    }
    IEnumerator ReturnToTitleScreenCoroutine()
    {
        levelTransition.GetComponent<Animator>().SetTrigger("startLevel");
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadSceneAsync("TitleScreen");
    }
    IEnumerator StartGameCoroutine()
    {
        levelTransition.GetComponent<Animator>().SetTrigger("startLevel");
        yield return new WaitForSeconds(2f);
        logo.gameObject.SetActive(true);
        loadText.gameObject.SetActive(true);
        SceneManager.LoadSceneAsync("Game");
    }
}
