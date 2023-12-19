using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShotInfoManager : MonoBehaviour
{
    Dictionary<string, int> bullet1 = new Dictionary<string, int>(){{"speed", 5},{"damage", 1}};
    Dictionary<string, int> bullet2 = new Dictionary<string, int>(){{"speed", 7},{"damage", 3}};

    [SerializeField]
    GameObject[] bulletTypeObjArray = new GameObject[3];

    //全ての弾
    [System.NonSerialized]
    List<Bullet> AllBulletInfoList = new List<Bullet>();

    public int simultaniousNum;

    // 一つの弾を生成する
    void BulletInfoInstantiate(Dictionary<string, int> bulletType, GameObject bulletTypeObj, Vector3 instantPos, Vector3 moveDir)
    {
        GameObject bulletObj = Instantiate(bulletTypeObj, instantPos, Quaternion.identity);
        Bullet bullet = new Bullet(bulletType["speed"], bulletType["damage"], moveDir, bulletObj);
        AllBulletInfoList.Add(bullet);
    }

    public void AllBulletMove(List<Bullet> AllBIList)
    {
        for(int i = 0; i < AllBIList.Count; i++)
        {
            AllBIList[i].BulletGetMove();
        }
    }
}
