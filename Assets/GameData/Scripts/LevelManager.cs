using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{

    [System.NonSerialized]
    //#0
    public int[] simulNumLevelArray = new int[] { 1, 2, 3, 4, 5, 6, 7 };
    //public int simulNumLevel;

    [System.NonSerialized]
    //#1
    public float[] destroyDistLevelArray = new float[] { 15, 20, 25, 30, 35, 40, 45 };
    //public int destroyDistLevel;

    [System.NonSerialized]
    //#2
    public float[] fireIntervalLevelArray = new float[] { 1.5f, 1.3f, 1.1f, 0.9f, 0.7f, 0.5f, 0.3f};
    //public int fireInterLevel;

    [System.NonSerialized]
    //#3
    public int[] bulletDamageLevelArray = new int[] { 1, 2, 3, 4, 5, 6, 7};
    //public int bulletDamageLevel;

    [System.NonSerialized]
    //#4
    public bool[] penetrateLevelArray = new bool[] { false, true };
    //public int penLevel;

    [System.NonSerialized]
    //#5
    public int[] bulletAngleLevelArray = new int[] { 5, 7, 10, 12, 15, 17, 20 };
    //public int bAngleLevel;

    [System.NonSerialized]
    public int[] rewardsLevelsArray = new int[6];

    int[] eachMaxLevelArray = new int[] { 7, 7, 7, 7, 2, 7};

    [System.NonSerialized]
    //パネルに表示するオプションの数字を保持
    public int[] infotoPanel = new int[3];

    List<int> rewardsList = new List<int>();


    public void RewardSelectAndlevelUp(int selectedPanelNum)
    {
        //Debug.Log(string.Join(",", infotoPanel));

        int rewardsIndex = infotoPanel[selectedPanelNum];
        //Debug.Log(rewardsIndex);
        //Debug.Log(rewardsLevelsArray[rewardsIndex]);
        
        if(rewardsLevelsArray[rewardsIndex] >= eachMaxLevelArray[rewardsIndex])
        {
            rewardsLevelsArray[rewardsIndex] = eachMaxLevelArray[rewardsIndex];
        }
        else
        {
            rewardsLevelsArray[rewardsIndex] += 1;
        }
    }

    public void RewardInit()
    {
        //1 ~ 6をリストに並べる
        for(int i = 0; i < rewardsLevelsArray.Length; i++)
        {
            rewardsList.Add(i);
        }

        //Debug.Log(rewardsList.Count);

        //0~5のオプションのうちパネルに表示するものを3つ選ぶ
        for (int i = 0; i < infotoPanel.Length; i++)
        {
            int rand = UnityEngine.Random.Range(0, rewardsList.Count - 1); 
            infotoPanel[i] = rewardsList[rand];
            rewardsList.RemoveAt(rand);
        }

        //Debug.Log(string.Join(",", infotoPanel));

        rewardsList.Clear();
    }

    public void LevelInit()
    {
        for(int i = 0; i < rewardsLevelsArray.Length; i++)
        {
            rewardsLevelsArray[i] = 1;
        }
    }

    public int LevelofIndex(int index)
    {
        return rewardsLevelsArray[index] - 1;
    }
}
