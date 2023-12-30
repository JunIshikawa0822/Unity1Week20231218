using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class ShotInfoManagerS : MonoBehaviour
{
    //[System.NonSerialized]

    [SerializeField]
    public GameObject[] bulletTypeObjArray = new GameObject[3];

    //全ての弾
    [System.NonSerialized]
    public List<Bullet> AllBulletInfoList = new List<Bullet>();


    // 一つの弾を生成する
    //・弾のオブジェクト配列・弾情報Dic・生成場所・進むベクトル・消える距離
    public void BulletInfoInstantiate(GameObject[] bTObjArray, int damage, Vector3 instantPos, Vector3 moveDir, float destroyDist, bool isbPen)
    {
        GameObject bulletObj = Instantiate(bTObjArray[0], instantPos, Quaternion.identity);
        Bullet bullet = new Bullet(0.2f, damage, moveDir, bulletObj, destroyDist, isbPen);
        AllBulletInfoList.Add(bullet);
    }

    //同時に弾を発射する
    public void BulletShotSimultaniously(Vector3 mouseVec, int simulNum, GameObject[] bTObjArray, int damage, Vector3 instantPos, float destroyDist, float bAngle, bool isbPen)
    {
        float theta;
        //Debug.Log(mouseVec);
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
            BulletInfoInstantiate(bTObjArray, damage, instantPos, vec, destroyDist, isbPen);
        }
    }
}
