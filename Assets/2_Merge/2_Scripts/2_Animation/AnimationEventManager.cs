using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationEventManager : MonoBehaviour
{
    public GameObject HPSlider;
    public void TimeStop()
    {
        Time.timeScale = 0;
    }

    public void UIDisplayStart()
    {
        HPSlider.SetActive(true);
        Invoke("UIDisplayEnd", 3.0f);
    }

    public void UIDisplayEnd()
    {
        HPSlider.SetActive(false);
    }
    //void EndAnimation()
    //{
    //    JunMainSystem.isMoveStartAnimEnd = true;
    //}
}
