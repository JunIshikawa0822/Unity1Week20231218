using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField]
    public Slider EXPSlider;

    //[SerializeField]
    //public Slider HPSlider;

    [SerializeField]
    public GameObject LevelUpUIParent;

    float EXPsliderMaxValuePersent = 100;
    float HPsliderMaxValuePersent = 100;

    [System.NonSerialized]
    public int selectedPanelnum = 0;

    [System.NonSerialized]
    public bool onClick = false;

    [SerializeField]
    GameObject[] panels = new GameObject[3];

    public void SliderMaxInit()
    {
        EXPSlider.maxValue = EXPsliderMaxValuePersent;
        //HPSlider.maxValue = HPsliderMaxValuePersent;
    }

    public void LevelUpUIInit()
    {
        LevelUpUIParent.SetActive(false);
    }

    public void SliderValueChange(Slider slider, float value)
    {
        slider.value = value;
    }

    public void selectRightPanel()
    {
        selectedPanelnum = 0;
        Debug.Log("押したよ");
    }

    public void selectMiddlePanel()
    {
        selectedPanelnum = 1;
        Debug.Log("押したよ");
    }

    public void selectLeftPanel()
    {
        selectedPanelnum = 2;
        Debug.Log("押したよ");
        Debug.Log(selectedPanelnum);
    }

    public void PointerEnter()
    {

    }

    public void SelectEnterPanel()
    {
        onClick = true;
    }


}
