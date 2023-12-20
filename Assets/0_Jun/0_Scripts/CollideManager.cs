using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideManager : MonoBehaviour
{
    //弾を消す
    public List<Collider> CollideEnemyList = new List<Collider>();

    public void BulletRemove(List<Bullet> ABIList, int number)
    {
        Destroy(ABIList[number].BulletGameObject());
        ABIList.RemoveAt(number);
    }

    //ぶつかったかを判定すると同時にぶつかった相手（敵）をリストに入れる
    public bool BulletCollide(Bullet bullet, LayerMask bulletHitLayer, List<Collider> ColOponentList, string tagName)
    {
        Collider[] colArray = Physics.OverlapSphere(
            bullet.BulletGameObject().transform.position,
            bullet.BulletGameObject().transform.lossyScale.x / 2,
            bulletHitLayer);

        if (colArray.Length < 1)
        {
            return false;
        }
        else
        {
            //ぶつかっていたら
            isObjectCollide(ColOponentList, colArray, tagName);
            return true;
        }
    }

    //タグを比較してリストに追加
    void isObjectCollide(List<Collider> ColOponentList, Collider[] cols, string tagName)
    {
        foreach(Collider objectCol in cols)
        {
            if (objectCol.gameObject.tag != tagName)
            {
                continue;
            }
            else
            {
                ColOponentList.Add(objectCol);
            }
        }
    }
}
