using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEditor.PackageManager;

public class PlayerEXPManagerS : MonoBehaviour
{
    [System.NonSerialized]
    public int totalPlayerEXP = 0;

    //[System.NonSerialized]
    //public int playerLevel = 0;

    [Range(3, 10), System.NonSerialized]
    int demandEXPprime = 3;

    [Range(1, 2), System.NonSerialized]
    float EXPRatio = 1.5f;

    [SerializeField]
    TextMeshProUGUI totalEXP;

    [SerializeField]
    TextMeshProUGUI demandEXP;

    [SerializeField]
    TextMeshProUGUI Level;

    [SerializeField]
    TextMeshProUGUI EXPtoNextLevel;

    [SerializeField]
    TextMeshProUGUI AccumeEXP;

    //経験値からレベルを計算
    public int EXPtoLevel()
    {
        //レベル1から2に必要な経験値（初項） = primeEXP
        //公比 = ratio
        int level;

        float calculate = ((totalPlayerEXP * EXPRatio - totalPlayerEXP) / demandEXPprime) + 1;

        level = (int)Mathf.Log(calculate, EXPRatio);

        return level + 1;
    }

    //レベルから次のレベルアップに必要な経験値を計算
    public int demandEXPtoNextLevel(int nowLevel)
    {
        int demandEXP;

        demandEXP = Mathf.FloorToInt(demandEXPprime * Mathf.Pow(EXPRatio, nowLevel - 1));

        return demandEXP;
    }

    //指定したレベルまでの累積経験値計算
    public int AccumulationEXP(int level)
    {
        int aEXP;

        if(level == 1)
        {
            aEXP = 0;
        }
        else
        {
            aEXP = Mathf.FloorToInt(demandEXPprime * ((Mathf.Pow(EXPRatio, level - 1) - 1) / (EXPRatio - 1)));
        }

        return aEXP;
    }

    //現在の経験値から現在のレベルまでの累積経験値を引く
    public float BarPersent(int nowLevel)
    {
        int EXPtoNextLevel = totalPlayerEXP - (int)AccumulationEXP(nowLevel);

        //Debug.Log("nowEXP" + EXPtoNextLevel + ", dem：" + demandEXPtoNextLevel(nowLevel));

        float persent = Mathf.FloorToInt(((float)EXPtoNextLevel / (float)demandEXPtoNextLevel(nowLevel)) * 1000);

        //Debug.Log(persent);

        return persent;
    }

    public void EXPdebugText()
    {
        int level = EXPtoLevel();
        int toNextLevel = totalPlayerEXP - (int)AccumulationEXP(level);

        totalEXP.text = "TotalEXP: " + totalPlayerEXP.ToString();
        Level.text = "Level: " + level.ToString();
        demandEXP.text = "You need " + demandEXPtoNextLevel(level).ToString() + " EXP to levelUp";
        EXPtoNextLevel.text = "You have " + toNextLevel.ToString() + "EXP";
        AccumeEXP.text = "you got " + (int)AccumulationEXP(level) + " for " + level + " level"; 
    }

    public void EXPdebugTextInit()
    {
        totalEXP.text = "0";
        Level.text = "0";
        demandEXP.text = "0";
        EXPtoNextLevel.text = "0";
        AccumeEXP.text = "0";
    }
}
