using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class BossBattleManager : MonoBehaviour
{
    public BattleState state;
    public GameObject player;
    public GameObject boss;

    BattleAttributes playerAttributes;
    BattleAttributes bossAttributes;

    public BattleUI playerUI;
    public BattleUI bossUI;

    // UI variables
    public Text dialogueText;
    public Button ability1;
    public Button ability2;
    public Button ability3;
    public Button ability4;

    public bool burn = false;
    public bool counter = false;

    private string playerLvl;

    // Sound Effects
    public AudioSource[] sounds;
    // Battle Effects
    public ParticleSystem[] abilities;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].Stop();
        }
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        // LOAD ALL DATA
        LoadPlayerData();
        if (PlayerPrefs.HasKey("playerLevel"))
        {
            playerLvl = PlayerPrefs.GetString("playerLevel");
        }
        else
            playerLvl = "1";
        bossAttributes = boss.GetComponent<BattleAttributes>();

        if (playerAttributes.level == "1") // only show available abilities
        {
            // hide all but attack/flee
            ability3.gameObject.SetActive(false);
            ability4.gameObject.SetActive(false);
        }
        if (playerAttributes.level == "2")
        {
            // hide last ability
            ability4.gameObject.SetActive(false);
        }

        dialogueText.text = "A massive " + bossAttributes.name + " approaches! Careful, this is a boss!";

        // add ui elements
        playerUI.SetUI(playerAttributes);
        bossUI.SetUI(bossAttributes);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an ability:";

    }

    public void ButtonOne() // ability 1
    {
        dialogueText.text = "You attempt to flee!";
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerFlee());
        }
        else
            return;
    }

    public void ButtonTwo() // attack 1
    {
        dialogueText.text = "You slapped the enemy! OUCH!";
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerSlap());
        }
        else
            return;
    }

    public void ButtonThree() // ability 3 unlocked at level 2
    {
        dialogueText.text = "You burned the enemy!";
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(Burn());
        }
        else
            return;
    }

    public void ButtonFour() // ability 4 unlocked at level 3
    {
        dialogueText.text = "You prepare to take a hit!";
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(Counter());
        }
        else
            return;
    }

    IEnumerator PlayerFlee()
    {
        // Give the player 50% chance to flee the battle
        int fleeChance = Random.Range(0, 10);
        if (fleeChance > 5)
        {
            dialogueText.text = "You fled the battle!";
            // save player hp here
            SavePlayerData();
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            dialogueText.text = "You failed to flee!";
            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    IEnumerator PlayerSlap()
    {
        bossAttributes.currentHealth -= 5; // the enemy now takes damage
        bossUI.SetHP(bossAttributes.currentHealth);
        sounds[0].Play();
        abilities[0].Play();
        yield return new WaitForSeconds(2f);

        if (bossAttributes.currentHealth <= 0)
        {
            state = BattleState.WIN;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            //StartCoroutine(EnemyTurn());
            EnemyTurn();
        }

    }

    IEnumerator Burn()
    {
        abilities[2].Play();
        yield return new WaitForSeconds(2f);
        sounds[2].Play();
        burn = true;
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    IEnumerator Counter()
    {
        yield return new WaitForSeconds(2f);
        sounds[1].Play();
        abilities[1].Play();
        int counterChance = Random.Range(0, 10);
        if (counterChance > 6)
        {
            counter = true;
            dialogueText.text = "You have blocked the attack!";
            yield return new WaitForSeconds(2f);
            int damageChance = Random.Range(0, 10);
            if (damageChance > 5)
            {
                dialogueText.text = "You returned the attack and dealt damage!";
                yield return new WaitForSeconds(2f);
                bossAttributes.currentHealth -= 15; // the enemy now takes damage
                bossUI.SetHP(bossAttributes.currentHealth);
            }
        }
        else
        {
            counter = false;
            dialogueText.text = "You failed to block the attack!";
            yield return new WaitForSeconds(2f);
        }
        if (bossAttributes.currentHealth <= 0)
        {
            state = BattleState.WIN;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            //StartCoroutine(EnemyTurn());
            EnemyTurn();
        }
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WIN)
        {
            dialogueText.text = "You defeated the enemy! You gained experience points!";
            PlayerPrefs.SetInt("BossDefeat", 1);
            playerAttributes.currentExp += 25 * int.Parse(bossAttributes.level);
            playerUI.SetExp(playerAttributes.currentExp);
            if (playerAttributes.currentExp >= playerAttributes.maxExp)
            {
                sounds[3].Play();
                if (playerAttributes.level == "1")
                {
                    Debug.Log("we are leveling up");
                    playerLvl = "2";
                    playerAttributes.currentHealth = playerAttributes.maxHealth;
                    playerAttributes.currentExp = 0;
                }
                if (playerAttributes.level == "2")
                {
                    playerLvl = "3";
                    playerAttributes.currentHealth = playerAttributes.maxHealth;
                    playerAttributes.currentExp = 0;
                }
                if (playerAttributes.level == "3")
                {
                    playerLvl = "4";
                    playerAttributes.currentHealth = playerAttributes.maxHealth;
                    playerAttributes.currentExp = 0;
                }
                playerAttributes.currentHealth = playerAttributes.maxHealth;
            }


        }
        else if (state == BattleState.LOSE)
        {
            dialogueText.text = "You have blacked out!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("GameScene");
            //playerAttributes.currentHealth = 25; // allows the player to slightly regain hp.
        }
        // SAVE ALL DATA HERE
        SavePlayerData();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameScene");
    }

    void EnemyTurn()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].Stop();
        }
        StartCoroutine(EnemyAttack());
    }

    IEnumerator EnemyAttack()
    {
        // do health changes and check if player is alive or dead
        dialogueText.text = bossAttributes.name + " thinks about their next action.";
        yield return new WaitForSeconds(2f);
        if (counter == false)
        {
            int attackRoll = Random.Range(0, 5);
            if (playerAttributes.currentHealth >= 50 && attackRoll >= 2)
            {
                abilities[3].Play();
                dialogueText.text = bossAttributes.name + " uses dragon breath!";
                playerAttributes.currentHealth -= 15; // the player now takes damage
                playerUI.SetHP(playerAttributes.currentHealth);
                yield return new WaitForSeconds(1f);
            }
            else if(playerAttributes.currentHealth > 0)
            {
                dialogueText.text = bossAttributes.name + " uses slam!";
                playerAttributes.currentHealth -= 5; // the player now takes damage
                playerUI.SetHP(playerAttributes.currentHealth);
                yield return new WaitForSeconds(1f);
            }
            else if (bossAttributes.currentHealth <= 10 && attackRoll >= 1) // try to panic heal
            {
                dialogueText.text = bossAttributes.name + " uses rest!";
                bossAttributes.currentHealth += 15; // the boss heals damage
                bossUI.SetHP(bossAttributes.currentHealth);
                yield return new WaitForSeconds(1f);
            }
            else
                dialogueText.text = "The attack missed!";
        }
        if (playerAttributes.currentHealth <= 0)
        {
            // player dies
            state = BattleState.LOSE;
            StartCoroutine(EndBattle());
        }
        else
        {
            if (burn == true)
            {
                dialogueText.text = "The " + bossAttributes.name + " got damaged by burn!";
                bossAttributes.currentHealth -= 5; // the enemy now takes damage
                bossUI.SetHP(bossAttributes.currentHealth);
                if (bossAttributes.currentHealth <= 0)
                {
                    state = BattleState.WIN;
                    StartCoroutine(EndBattle());
                }
            }
            counter = false;
            yield return new WaitForSeconds(2f);
            abilities[3].Stop();
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    private void SavePlayerData()
    {
        PlayerPrefs.SetString("playerName", "Player");
        PlayerPrefs.SetString("playerLevel", playerLvl);
        PlayerPrefs.SetInt("playerMaxHP", playerAttributes.maxHealth);
        PlayerPrefs.SetInt("playerCurrentHP", playerAttributes.currentHealth);
        PlayerPrefs.SetInt("playerMaxXP", playerAttributes.maxExp);
        PlayerPrefs.SetInt("playerCurrentXP", playerAttributes.currentExp);
        PlayerPrefs.Save();
    }

    private void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("playerLevel"))
        {
            Debug.Log("loading player data");
            player.GetComponent<BattleAttributes>().SetAttributes("Player", PlayerPrefs.GetString("playerLevel")
                , PlayerPrefs.GetInt("playerMaxHP"), PlayerPrefs.GetInt("playerCurrentHP")
                , PlayerPrefs.GetInt("playerMaxXP"), PlayerPrefs.GetInt("playerCurrentXP")); // use this setter first before assigning
            playerAttributes = player.GetComponent<BattleAttributes>();
        }
        else
        {
            player.GetComponent<BattleAttributes>().SetAttributes("Player", "1", 100, 100, 100, 0); // first time battling
            playerAttributes = player.GetComponent<BattleAttributes>();
        }
    }
}

