using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class ShotInfoManager : MonoBehaviour
{
    [System.NonSerialized]
    public Dictionary<string, float> bullet1 = new Dictionary<string, float>(){ {"BTypeNum",1},{"speed", 0.2f},{"damage", 1}};
    [System.NonSerialized]
    public Dictionary<string, float> bullet2 = new Dictionary<string, float>(){ {"BTypeNum",2},{"speed", 7},{"damage", 3}};

    [SerializeField]
    public GameObject[] bulletTypeObjArray = new GameObject[3];

    //全ての弾
    [System.NonSerialized]
    public List<Bullet> allBulletInfoList = new List<Bullet>();

    [Range(1, 7), SerializeField]
    public int simultaniousNum = 1;

    [Range(0, 100), SerializeField]
    public float destroyDistance = 50;

    [Range(5, 30), SerializeField]
    public int bulletAngle = 10;

    [SerializeField]
    public bool isPenetrate = false;

    // 一つの弾を生成する
    //・弾のオブジェクト配列・弾情報Dic・生成場所・進むベクトル・消える距離
    void BulletInfoInstantiate(GameObject[] bTObjArray, Dictionary<string, float> bTypeDic, Vector3 instantPos, Vector3 moveDir, float destroyDist)
    {
        GameObject bulletObj = Instantiate(bTObjArray[(int)bTypeDic["BTypeNum"]], instantPos, Quaternion.identity);
        Bullet bullet = new Bullet(bTypeDic["speed"], bTypeDic["damage"], moveDir, bulletObj, destroyDist);
        allBulletInfoList.Add(bullet);
    }

    //同時に弾を発射する
    public void BulletShotSimultaniously(Vector3 mouseVec, int simulNum, GameObject[] bTObjArray, Dictionary<string, float> bTypeDic, Vector3 instantPos, float destroyDist, float bAngle, float zValue)
    {
        float theta;
        Debug.Log(mouseVec);
        for (int i = 0; i < simulNum; i++)
        {
            //奇数なら
            if (simulNum % 2 == 1)
            {
                theta = Mathf.Pow(-1, i) * ((i + 1) / 2) * bAngle;
            }
            //偶数なら
            else
            {
                theta = Mathf.Pow(-1, i) * ((i + 1) / 2) * bAngle + bAngle / 2;
            }

            Vector3 vec = Quaternion.Euler(0, theta, 0) * mouseVec;
            //Vector3 vec = new Vector3(
            //    mouseVec.x * Mathf.Cos(theta) - mouseVec.z * Mathf.Sin(theta),
            //    zValue,
            //    mouseVec.x * Mathf.Sin(theta) + mouseVec.z * Mathf.Cos(theta)).normalized;
            //Debug.Log(theta);
            BulletInfoInstantiate(bTObjArray, bTypeDic, instantPos, vec, destroyDist);
        }
    }
}
