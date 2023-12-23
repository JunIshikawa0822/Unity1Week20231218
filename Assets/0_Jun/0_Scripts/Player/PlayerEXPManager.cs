using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEXPManager : MonoBehaviour
{
    public int totalPlayerEXP;

    public int playerLevel;

    [Range(3, 10), SerializeField]
    int demandEXPprime;

    [Range(1, 2), SerializeField]
    float EXPRatio;

    //経験値からレベルを計算
    int EXPtoLevel()
    {
        //レベル1から2に必要な経験値（初項） = primeEXP
        //公比 = ratio
        int level;

        float calculate = (totalPlayerEXP * (EXPRatio - 1)) / demandEXPprime;

        level = (int)Mathf.Log(calculate, EXPRatio);

        return level;
    }

    //レベルから次のレベルアップに必要な経験値を計算
    int demandEXPtoNextLevel(int nowLevel)
    {
        int demandEXP;

        demandEXP = (int)(demandEXPprime * Mathf.Pow(EXPRatio, nowLevel - 1));

        return demandEXP;
    }

    //指定したレベルまでの累積経験値計算
    float AccumulationEXP(int level)
    {
        float aEXP = demandEXPprime * (Mathf.Pow(EXPRatio, level) / EXPRatio);

        return aEXP;
    }

    public bool isPlayerLevelUp(int playerEXP, int nowLevel)
    {
        float aEXP = AccumulationEXP(nowLevel + 1);

        if (playerEXP > aEXP)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //現在の経験値から現在のレベルまでの累積経験値を引く
    public float BarPersent(int playerEXP, int nowLevel)
    {
        int EXPtoNextLevel = playerEXP - (int)AccumulationEXP(nowLevel);

        float persent = (EXPtoNextLevel / demandEXPtoNextLevel(nowLevel)) * 100;

        return persent;
    }
}
