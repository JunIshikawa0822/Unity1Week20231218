using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField]
    public Slider EXPSlider;

    [SerializeField]
    public Slider HPSlider;

    [SerializeField]
    public GameObject LevelUpUIParent;

    float EXPsliderMaxValuePersent = 1000;
    float HPsliderMaxValuePersent = 100;

    [System.NonSerialized]
    public int selectedPanelnum = 0;

    [System.NonSerialized]
    public bool onClick = false;

    [SerializeField]
    TextMeshProUGUI[] panelTexts = new TextMeshProUGUI[3];

    [SerializeField]
    public TextMeshProUGUI levelText;

    string[] optionsExplains = new string[] { "Shots", "Bullet Range", "Interval", "Damage", "Penetrate", "Wide"};

    public void SliderMaxInit()
    {
        EXPSlider.maxValue = EXPsliderMaxValuePersent;
        HPSlider.maxValue = HPsliderMaxValuePersent;
    }

    public void LevelUpUIInit()
    {
        LevelUpUIParent.SetActive(false);
    }

    public void SliderValueChange(Slider slider, float value)
    {
        slider.value = value;
    }

    public void select1stPanel()
    {
        selectedPanelnum = 0;
        Debug.Log("押したよ");
    }

    public void select2ndPanel()
    {
        selectedPanelnum = 1;
        Debug.Log("押したよ");
    }

    public void select3rdPanel()
    {
        selectedPanelnum = 2;
        Debug.Log("押したよ");
    }

    public void PointerEnter()
    {

    }

    public void SelectEnterPanel()
    {
        onClick = true;
    }

    public void RewardUISet(int[] infotoPanel, int[] levelArray)
    {
        //Debug.Log(string.Join(",", infotoPanel));
        for(int i = 0; i < infotoPanel.Length; i++)
        {
            int indexOfRewards = infotoPanel[i];
            int level = levelArray[indexOfRewards];

            panelTexts[i].text = optionsExplains[indexOfRewards] + "\n Lv:" + (level+1);
        }
    }

    public void TextSet(TextMeshProUGUI text, string str, int value)
    {
        text.text = str + value;
    }
}
