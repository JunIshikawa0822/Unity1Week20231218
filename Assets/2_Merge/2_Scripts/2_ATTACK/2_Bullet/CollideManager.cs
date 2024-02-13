using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideManage : MonoBehaviour
{
    

    //ぶつかったオブジェクトを返す
    public Collider[] whatBulletCollide(Bullet bullet, LayerMask bulletHitLayer)
    {
        //すべてのオブジェクトを取得
        Collider[] colArray = Physics.OverlapSphere(
            bullet.BulletGameObject().transform.position,
            bullet.BulletGameObject().transform.lossyScale.x / 2,
            bulletHitLayer);

        return colArray;
    }

    //tagNameがみつかるまで探索
    public List<Collider> FindWhatYouWant(Collider[] cols, string tagName)
    {
        List<Collider> colsList = new List<Collider>();

        foreach(Collider objectCol in cols)
        {
            //壁があったらおしまい
            if (objectCol.gameObject.tag == tagName)
            {
                break;
            }
            //壁がない
            else
            {
                colsList.Add(objectCol);
            }
        }

        return colsList;
    }
}
