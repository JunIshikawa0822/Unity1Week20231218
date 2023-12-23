using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public Slider EXPSlider;

    [SerializeField]
    public Slider HPSlider;

    [SerializeField]
    public GameObject[] PanelsArray = new GameObject[3];

    float EXPsliderMaxValuePersent = 100;
    float HPsliderMaxValuePersent = 100;

    public void SliderMaxInit()
    {
        EXPSlider.maxValue = EXPsliderMaxValuePersent;
        HPSlider.maxValue = HPsliderMaxValuePersent;
    }

    public void SliderValueChange(Slider slider, float value)
    {
        slider.value = value;
    } 
}
