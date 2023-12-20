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

    //壁にぶつかって消えるかどうかを判定
    public bool BulletCollide(Bullet bullet, LayerMask bulletHitLayer, List<Collider> ColOponentList, string tagName)
    {
        //すべてのオブジェクトを取得
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
            return isObjectCollide(ColOponentList, colArray, tagName);
        }
    }

    //タグを比較してリストに追加
    bool isObjectCollide(List<Collider> ColOponentList, Collider[] cols, string tagName)
    {
        bool isWall = false;

        foreach(Collider objectCol in cols)
        {
            //壁にぶつかった瞬間break
            if (objectCol.gameObject.tag != tagName)
            {
                isWall = true;
                break;
            }
            //敵にぶつかっている間はそのまま
            else
            {
                isWall = false;
                ColOponentList.Add(objectCol);
            }
        }

        return isWall;
    }
}
