using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UserInterfaceManager : MonoBehaviour
{
    public AudioClip checksound;

    public AudioClip entersound;

    public AudioClip rewardsound;

    [SerializeField]
    SoundManager soundManager;

    [SerializeField]
    public Slider EXPSlider;

    [SerializeField]
    public Slider HPSlider;

    [SerializeField]
    public GameObject LevelUpUIParent;

    Animator UIAnim;

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
        UIAnim = LevelUpUIParent.GetComponent<Animator>();
        LevelUpUIParent.SetActive(false);
    }

    public void SliderValueChange(Slider slider, float value)
    {
        slider.value = value;
    }

    public void select1stPanel()
    {
        soundManager.MakeSound(checksound,0.5f);
        selectedPanelnum = 0;
        Debug.Log("押したよ");
    }

    public void select2ndPanel()
    {
        soundManager.MakeSound(checksound, 0.5f);
        selectedPanelnum = 1;
        Debug.Log("押したよ");
    }

    public void select3rdPanel()
    {
        soundManager.MakeSound(checksound, 0.5f);
        selectedPanelnum = 2;
        Debug.Log("押したよ");
    }

    public void PointerEnter()
    {

    }

    public void SelectEnterPanel()
    {
        soundManager.MakeSound(entersound,0.3f);
        onClick = true;
    }

    public void RewardUISet(int[] infotoPanel, int[] levelArray)
    {
        soundManager.MakeSound(rewardsound,0.8f);
        UIAnim.SetTrigger("RewardUI");
       // Time.timeScale = 0;
        //Debug.Log(string.Join(",", infotoPanel));
        for (int i = 0; i < infotoPanel.Length; i++)
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
