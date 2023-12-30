using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManagerS : MonoBehaviour
{
    void GiveDamage(Bullet bullet, EnemyS enemy)
    {
        //弾からダメージ値を取得
        int damage = bullet.BulletDamage();

        //敵にダメージを与える
        enemy.GetDamage(damage);
    }

    //ダメージ処理
    public int bulletDamegeProcess(List<Collider> ColOpList, List<EnemyS> AEIList, List<GameObject> enemyObjList, Bullet bullet)
    {
        int addEXP = 0;

        for(int i = 0; i < ColOpList.Count; i++)
        {
            int enListIndex = enemyObjList.IndexOf(ColOpList[i].gameObject);

            //Debug.Log("Index" + enListIndex);

            if(enListIndex < 0)
            {
                continue;
            }

            EnemyS enemy = AEIList[enListIndex];

            GiveDamage(bullet, enemy);

            //addEXP += (int)enemy.EnemyEXP();

            //死んだら
            if (enemy.isDead())
            {
                //Debug.Log("加算前：" + totalPlayerEXP);

                //経験値加算
                addEXP += (int)enemy.EnemyEXP();

                //Debug.Log("加算後：" + totalPlayerEXP);

                //敵の破壊
                EnemyRemove(AEIList, enemyObjList, enListIndex);
            }
        }
        return addEXP;
    }

    void EnemyRemove(List<EnemyS> AEIList, List<GameObject> enemyObjList, int number)
    {
        Destroy(AEIList[number].EnemyGameObject());
        AEIList.RemoveAt(number);
        enemyObjList.RemoveAt(number);
    }
}
