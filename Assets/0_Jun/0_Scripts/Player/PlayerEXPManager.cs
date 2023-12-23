using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEXPManager : MonoBehaviour
{
    [System.NonSerialized]
    public int totalPlayerEXP = 0;

    //[System.NonSerialized]
    //public int playerLevel = 0;

    [Range(3, 10), System.NonSerialized]
    int demandEXPprime = 3;

    [Range(1, 2), System.NonSerialized]
    float EXPRatio = 1.2f;

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
    int demandEXPtoNextLevel(int nowLevel)
    {
        int demandEXP;

        demandEXP = Mathf.FloorToInt(demandEXPprime * Mathf.Pow(EXPRatio, nowLevel - 1));

        return demandEXP;
    }

    //指定したレベルまでの累積経験値計算
    int AccumulationEXP(int level)
    {
        int aEXP;

        if(level == 1)
        {
            aEXP = 0;
        }
        else
        {
            aEXP = Mathf.FloorToInt(demandEXPprime * (Mathf.Pow(EXPRatio, level) / EXPRatio));
        }

        return aEXP;
    }

    //bool isPlayerLevelUp(int nowLevel)
    //{
    //    int aEXP = AccumulationEXP(nowLevel + 1);

    //    if (totalPlayerEXP > aEXP)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //現在の経験値から現在のレベルまでの累積経験値を引く
    public float BarPersent(int nowLevel)
    {
        int EXPtoNextLevel = totalPlayerEXP - (int)AccumulationEXP(nowLevel);

        //Debug.Log("nowEXP" + EXPtoNextLevel + ", dem：" + demandEXPtoNextLevel(nowLevel));

        float persent = Mathf.FloorToInt(((float)EXPtoNextLevel / (float)demandEXPtoNextLevel(nowLevel)) * 100);

        //Debug.Log(persent);

        return persent;
    }
}
