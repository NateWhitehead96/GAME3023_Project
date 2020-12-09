using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{

    public GameObject self;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("BossDefeat"))
        {
            self.SetActive(false);
        }
    }

}
