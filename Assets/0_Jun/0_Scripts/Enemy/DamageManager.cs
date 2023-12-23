using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    void GiveDamage(Bullet bullet,Enemy enemy)
    {
        //弾からダメージ値を取得
        float damage = bullet.BulletDamage();

        //敵にダメージを与える
        enemy.GetDamage(damage);
    }

    //ダメージ処理
    public void bulletDamegeProcess(List<Collider> ColOpList, List<Enemy> AEIList, List<GameObject> enemyObjList, Bullet bullet, int totalPlayerEXP)
    {
        for(int i = 0; i < ColOpList.Count; i++)
        {
            int enListIndex = enemyObjList.IndexOf(ColOpList[i].gameObject);

            Enemy enemy = AEIList[enListIndex];

            GiveDamage(bullet, enemy);

            //死んだら
            if (enemy.isDead())
            {         
                //経験値加算
                totalPlayerEXP += (int)enemy.EnemyEXP();

                //敵の破壊
                EnemyRemove(AEIList, enListIndex);
            }
        }   
    }

    void EnemyRemove(List<Enemy> AEIList, int number)
    {
        Destroy(AEIList[number].EnemyGameObject());
        AEIList.RemoveAt(number);
    }
}
