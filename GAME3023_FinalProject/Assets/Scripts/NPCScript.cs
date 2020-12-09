using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCScript : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            text.fontSize = 30;
            text.gameObject.SetActive(true);
            text.text = "Hello player! To the left of me here is wild grass to help you level up. Stay away from the cave until you reach a good level! You gain new abilities every level!";
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        text.gameObject.SetActive(false);
        text.fontSize = 0;
    }
}
