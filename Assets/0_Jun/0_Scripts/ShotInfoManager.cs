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
    GameObject[] bulletTypeObjArray = new GameObject[3];

    //全ての弾
    [System.NonSerialized]
    public List<Bullet> AllBulletInfoList = new List<Bullet>();

    [System.NonSerialized]
    public int simultaniousNum;

    // 一つの弾を生成する
    public void BulletInfoInstantiate(Dictionary<string, float> bTypeDic, Vector3 instantPos, Vector3 moveDir)
    {
        GameObject bulletObj = Instantiate(bulletTypeObjArray[(int)bTypeDic["BTypeNum"]], instantPos, Quaternion.identity);
        Bullet bullet = new Bullet(bTypeDic["speed"], bTypeDic["damage"], moveDir, bulletObj);
        AllBulletInfoList.Add(bullet);
    }

    public void AllBulletMove(List<Bullet> AllBIList)
    {
        for(int i = 0; i < AllBIList.Count; i++)
        {
            AllBIList[i].BulletGetMove();
        }
    }

    //衝突判定
    bool isBulletCollide(Bullet bullet, LayerMask bulletHitLayer)
    {
        Collider[] cols = Physics.OverlapSphere(
            bullet.BulletGameObject().transform.position,
            bullet.BulletGameObject().transform.lossyScale.x / 2,
            bulletHitLayer);
        //Debug.Log(cols.Length);

        if(cols.Length > 0)
        {
            Debug.Log("きえます");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BulletRemove(List<Bullet> AllBIList, LayerMask bulletHitLayer)
    {
        for(int i = 0; i < AllBIList.Count; i++)
        {
            if (!isBulletCollide(AllBIList[i], bulletHitLayer))
            {
                continue;
            }
            else
            {
                //Debug.Log("消えるよ");
                Destroy(AllBIList[i].BulletGameObject());
                AllBIList.RemoveAt(i);
            }
        }
    }
}
