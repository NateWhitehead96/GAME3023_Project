using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void OnPlay() // loading main scene
    {
        SceneManager.LoadScene(1);
    }
}
