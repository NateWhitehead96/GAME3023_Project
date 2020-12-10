using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public Text credits;
    public Animator screen;
    private void Start()
    {
        credits.text = "By Nathan Whitehead and Phoenix Makins.";
        StartCoroutine(IdleScreen());
    }

    IEnumerator IdleScreen()
    {
        yield return new WaitForSeconds(1.5f);
        screen.SetInteger("ScreenTransition", 1);
    }
    public void OnPlay() // loading main scene
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        screen.SetInteger("ScreenTransition", 2);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
