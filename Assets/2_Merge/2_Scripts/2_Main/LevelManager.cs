using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Range(1, 7), SerializeField]
    public int simultaniousNum = 1;

    [Range(0, 100), SerializeField]
    public float destroyDistance = 50;

    [Range(5, 30), SerializeField]
    public int bulletAngle = 10;

    [Range(0.1f, 5), SerializeField]
    public float fireInterval = 0.4f;

    [SerializeField]
    public bool isPenetrate = false;

    [System.NonSerialized]
    public Dictionary<string, float> nowBullet;

    [System.NonSerialized]
    public Dictionary<string, float> bullet1 = new Dictionary<string, float>() { { "BTypeNum", 1 }, { "speed", 0.2f }, { "damage", 1 } };

    [System.NonSerialized]
    public Dictionary<string, float> bullet2 = new Dictionary<string, float>() { { "BTypeNum", 2 }, { "speed", 7 }, { "damage", 3 } };

    // Start is called before the first frame update
    int[] displayRewardNumArray = new int[3];

    void rewardSelect(int selectedPanelNum)
    {

    }
}
