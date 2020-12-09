using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Text name;
    public Text level;
    public Slider healthSlider;
    public Slider expSlider;

    public void SetUI(BattleAttributes unit)
    {
        name.text = unit.name;
        level.text = "Level: " + unit.level;
        healthSlider.maxValue = unit.maxHealth;
        healthSlider.value = unit.currentHealth;
        expSlider.maxValue = unit.maxExp;
        expSlider.value = unit.currentExp;
    }

    public void SetHP(int hp)
    {
        healthSlider.value = hp;
    }

    public void SetExp(int exp)
    {
        expSlider.value = exp;
    }
}
