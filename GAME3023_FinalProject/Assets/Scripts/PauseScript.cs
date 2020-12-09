using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject self;
    public bool isShowing = false;
    public GameObject player;

    private void Start()
    {
        self.SetActive(false);
    }
    public void OnResume()
    {
        self.SetActive(false);
        isShowing = false;
    }

    public void SetShow(bool active)
    {
        isShowing = active;
    }

    public void OnMainMenu()
    {
        PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerY", player.transform.position.y);
        PlayerPrefs.Save();
        SceneManager.LoadScene("StartScene");
    }
}
