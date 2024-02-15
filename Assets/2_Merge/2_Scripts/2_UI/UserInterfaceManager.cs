using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UserInterfaceManager : MonoBehaviour
{
    public Image panel1;
    public Image panel2;
    public Image panel3;

    Outline selectedReward1;
    Outline selectedReward2;
    Outline selectedReward3;

    public Button Restart;

    public AudioClip checksound;

    public AudioClip entersound;

    public AudioClip rewardsound;

    [SerializeField]
    SoundManager soundManager;

    [SerializeField]
    public Slider EXPSlider;

    [SerializeField]
    public Slider HPSlider;

    [SerializeField] public GameObject Slider;

    [SerializeField]
    public GameObject LevelUpUIParent;

    [SerializeField]
    public GameObject PauseParent;

    [SerializeField]
    public GameObject ResultParent;

    Animator UIAnim;

    float EXPsliderMaxValuePersent = 1000;
    float HPsliderMaxValuePersent = 100;

    [System.NonSerialized]
    public int selectedPanelnum = 0;

    [System.NonSerialized]
    public bool onClick = false;

    [System.NonSerialized]
    public bool onClickRestart = false;

    [SerializeField]
    TextMeshProUGUI[] panelTexts = new TextMeshProUGUI[3];

    [SerializeField]
    public TextMeshProUGUI levelText;

    string[] optionsExplains = new string[] { "弾数＋１", "射程範囲延長", "射撃間隔短縮", "攻撃力強化", "貫通力強化", "Wide"};

    public void SliderMaxInit()
    {
        EXPSlider.maxValue = EXPsliderMaxValuePersent;
        HPSlider.maxValue = HPsliderMaxValuePersent;
        Slider.SetActive(false);
    }

    public void LevelUpUIInit()
    {
        ResultParent.SetActive(false);
        UIAnim = LevelUpUIParent.GetComponent<Animator>();
        LevelUpUIParent.SetActive(false);
        PauseParent.SetActive(false);
        selectedReward1 = panel1.GetComponent<Outline>();
        selectedReward2 = panel2.GetComponent<Outline>();
        selectedReward3 = panel3.GetComponent<Outline>();
        selectedReward1.enabled = false;
        selectedReward2.enabled = false;
        selectedReward3.enabled = false;
        HPSlider.enabled = false;

    }


    public void OnClickButtonRestart()
    {
        // Time.timeScale = 1;
        onClickRestart = true;
        Debug.Log("hogehoge");
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
        selectedReward1.enabled = true;
        selectedReward2.enabled = false;
        selectedReward3.enabled = false;
    }

    public void select2ndPanel()
    {
        soundManager.MakeSound(checksound, 0.5f);
        selectedPanelnum = 1;
        Debug.Log("押したよ");
        selectedReward1.enabled = false;
        selectedReward2.enabled = true;
        selectedReward3.enabled = false;
    }

    public void select3rdPanel()
    {
        soundManager.MakeSound(checksound, 0.5f);
        selectedPanelnum = 2;
        Debug.Log("押したよ");
        selectedReward1.enabled = false;
        selectedReward2.enabled = false;
        selectedReward3.enabled = true;
    }

    public void PointerEnter()
    {

    }

    public void SelectEnterPanel()
    {
        soundManager.MakeSound(entersound,0.7f);
        onClick = true;
    }

    public void ResultUISet()
    {
        ResultParent.SetActive(true);
    }

    public void RewardUISet(int[] infotoPanel, int[] levelArray)
    {
        selectedReward1.enabled = false;
        selectedReward2.enabled = false;
        selectedReward3.enabled = false;
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
