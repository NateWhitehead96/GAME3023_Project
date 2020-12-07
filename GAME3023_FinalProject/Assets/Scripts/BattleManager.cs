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
            ability3.gameObject.SetActive(false);
            ability4.gameObject.SetActive(false);
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
        dialogueText.text = "You slapped the enemy! OUCH!";
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerSlap());
        }
        else
            return;
    }

    public void ButtonFour() // ability 4 unlocked at level 3
    {
        dialogueText.text = "You slapped the enemy! OUCH!";
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerSlap());
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
        dialogueText.text = enemyAttributes.name + " uses an ability.";
        // do health changes and check if player is alive or dead
        if (playerAttributes.currentHealth <= 0)
        {
            // player dies
            state = BattleState.LOSE;
            StartCoroutine(EndBattle());
        }
        else
        {
            yield return new WaitForSeconds(1f);
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
}
