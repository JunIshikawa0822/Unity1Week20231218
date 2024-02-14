using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManagerS : MonoBehaviour
{
    //[Range(1, 7), SerializeField]
    //public int simultaniousNum = 1;

    //[Range(0, 100), SerializeField]
    //public float destroyDistance = 50;

    //[Range(5, 30), SerializeField]
    //public int bulletAngle = 10;

    //[Range(0.1f, 5), SerializeField]
    //public float fireInterval = 0.4f;

    //[SerializeField]
    //public bool isPenetrate = false;

    //[System.NonSerialized]
    //public Dictionary<string, float> nowBullet;

    //[System.NonSerialized]
    //public Dictionary<string, float> bullet1 = new Dictionary<string, float>() { { "BTypeNum", 1 }, { "speed", 0.1f }, { "damage", 1 } };

    //[System.NonSerialized]
    //public Dictionary<string, float> bullet2 = new Dictionary<string, float>() { { "BTypeNum", 2 }, { "speed", 7 }, { "damage", 3 } };

    // Start is called before the first frame update

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
        int rewardsIndex = infotoPanel[selectedPanelNum];
        
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
