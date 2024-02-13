using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackObjectAdmin : MonoBehaviour
{
    [SerializeField]
    public CollideManage CLManager;

    [SerializeField]
    public DamageManager DMManager;

    //弾を消す
    public void BulletRemove(List<Bullet> ABIList, int number)
    {
        Destroy(ABIList[number].BulletGameObject());
        ABIList.RemoveAt(number);
    }

    public void BulletProcess(List<Bullet> ABIList, List<Enemy> AEIList, List<GameObject> AEOList, int number, LayerMask bHitLayer, string tagName, bool isPen)
    {
        Bullet bullet = ABIList[number];
        Collider[] cols = CLManager.whatBulletCollide(bullet, bHitLayer);


        //Debug.Log("加算前レベル" + beforeLevel);

        //Debug.Log("加算前" + PEXPManager.totalPlayerEXP);

        //何かにぶつかっている
        if (cols.Length > 0)
        {
            List<Collider> colOponentList = CLManager.FindWhatYouWant(cols, tagName);

            //colsが初っ端から壁だった
            if (colOponentList.Count < 1)
            {
                //弾を破壊
                BulletRemove(ABIList, number);

                //判定を行ったのでColOpListの中身を削除
                colOponentList.Clear();

                return;
            }

            //衝突したリストに壁があった
            if (colOponentList.Count < cols.Length)
            {
                //敵にダメージ判定
                DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet);
                //GetEXP(DMManager.deadEnemiesList);

                //レベルアップするかどうか
                //LevelUpCheckProcess(beforeLevel);

                //弾を破壊
                BulletRemove(ABIList, number);

                //判定を行ったのでColOpListの中身を削除
                colOponentList.Clear();
                return;
            }
            //なかった
            else
            {
                //Debug.Log(colOponentList[0].name);

                //貫通でない
                if (!isPen)
                {
                    DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet);
                    //GetEXP(DMManager.deadEnemiesList);
                    //レベルアップするかどうか
                    //LevelUpCheckProcess(beforeLevel);

                    //弾を破壊
                    BulletRemove(ABIList, number);

                    //判定を行ったのでColOpListの中身を削除
                    colOponentList.Clear();
                    return;
                }
                //貫通なら
                else
                {
                    if (DMManager.isPenetrateActive)
                    {
                        ABIList[number].BulletGetMove();
                        return;
                    }

                    DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet);
                    //GetEXP(DMManager.deadEnemiesList);

                    //レベルアップするかどうか
                    //LevelUpCheckProcess(beforeLevel);

                    //判定を行ったのでColOpListの中身を削除
                    colOponentList.Clear();

                    DMManager.StartCoroutine("PenetrateIntervalTimer");
                }
            }
        }

        ABIList[number].BulletGetMove();
        //Debug.Log("うごいた");
    }
}
