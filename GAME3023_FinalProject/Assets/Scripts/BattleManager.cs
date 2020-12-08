using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// Note for Phoenix, so far all button's do the same shit. We need to be able to dynamically set abilities or at least make some availible if leveled up
public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WIN,
    LOSE
}
public class BattleManager : MonoBehaviour
{
    public BattleState state;

    public Animator playerAnimator;
    public Animator enemyAnimator;

    public GameObject player;
    public GameObject enemy;

    BattleAttributes playerAttributes;
    BattleAttributes enemyAttributes;

    public BattleUI playerUI;
    public BattleUI enemyUI;

    // UI variables
    public Text dialogueText;
    public Button ability1;
    public Button ability2;
    public Button ability3;
    public Button ability4;

    public bool burn = false;
    public bool counter = false;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        // LOAD ALL DATA
        player.GetComponent<BattleAttributes>().SetAttributes("Player", "1", 20, 14, 100, 25); // use this setter first before assigning
        playerAttributes = player.GetComponent<BattleAttributes>();
        enemyAttributes = enemy.GetComponent<BattleAttributes>();

        dialogueText.text = "A wild " + enemyAttributes.name + " approaches!";

        // add ui elements
        playerUI.SetUI(playerAttributes);
        enemyUI.SetUI(enemyAttributes);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an ability:";
        if(playerAttributes.level == "1")
        {
            // then only allow 1 button to be visible and so on
            //ability3.gameObject.SetActive(false);
            //ability4.gameObject.SetActive(false);
        }
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
        int fleeChance = Random.Range(0, 10);
        if(fleeChance > 5)
        {
            dialogueText.text = "You fled the battle!";
            // save player hp here
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
        enemyAttributes.currentHealth -= 5; // the enemy now takes damage
        enemyUI.SetHP(enemyAttributes.currentHealth);

        yield return new WaitForSeconds(2f);

        if (enemyAttributes.currentHealth <= 0)
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
        yield return new WaitForSeconds(2f);

        burn = true;
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    IEnumerator Counter()
    {
        yield return new WaitForSeconds(2f);

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
                enemyAttributes.currentHealth -= 5; // the enemy now takes damage
                enemyUI.SetHP(enemyAttributes.currentHealth);
            }
        }
        else
        {
            counter = false;
            dialogueText.text = "You failed to block the attack!";
            yield return new WaitForSeconds(2f);
        }
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WIN)
        {
            dialogueText.text = "You defeated the enemy! You gained 25 experience points!";
            playerAttributes.currentExp += 25;
            playerUI.SetExp(playerAttributes.currentExp);
            // SAVE ALL DATA HERE
        }
        else if (state == BattleState.LOSE)
        {
            dialogueText.text = "You have blacked out!";
        }
        // make sure to save player stuff
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameScene");
    }

    void EnemyTurn()
    {
        StartCoroutine(EnemyAttack());
    }

    IEnumerator EnemyAttack()
    {
        // do health changes and check if player is alive or dead

        if (counter == false)
        {
            dialogueText.text = enemyAttributes.name + " uses an ability.";
            yield return new WaitForSeconds(2f);
            dialogueText.text = "You got hit!";
            yield return new WaitForSeconds(2f);
            playerAttributes.currentHealth -= 5; // the player now takes damage
            playerUI.SetHP(playerAttributes.currentHealth);
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
                dialogueText.text = "The " + enemyAttributes.name + " got damaged by burn!";
                enemyAttributes.currentHealth -= 5; // the enemy now takes damage
                enemyUI.SetHP(enemyAttributes.currentHealth);
                if (enemyAttributes.currentHealth <= 0)
                {
                    state = BattleState.WIN;
                    StartCoroutine(EndBattle());
                }
            }
            counter = false;
            yield return new WaitForSeconds(2f);
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
}
