using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public Text credits;
    private void Start()
    {
        credits.text = "By Nathan Whitehead and Phoenix Makins.";
    }
    public void OnPlay() // loading main scene
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
