using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleAttributes : MonoBehaviour
{
    public string name;
    public string level;
    public int maxHealth;
    public int currentHealth;
    public int maxExp;
    public int currentExp;

    public void SetAttributes(string _name, string _level, int _maxhp, int _currenthp, int _maxexp, int _currentexp)
    {
        name = _name;
        level = _level;
        maxHealth = _maxhp;
        currentHealth = _currenthp;
        maxExp = _maxexp;
        currentExp = _currentexp;
    }

    public void SetHP(int maxHP)
    {
        currentHealth = maxHP;
    }

}
