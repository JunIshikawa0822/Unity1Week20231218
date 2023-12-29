using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [System.NonSerialized]
    public bool isPenetrateActive = false;
    //float fireInterval;
    WaitForSeconds penetrateIntervalWait;

    public List<Enemy> deadEnemiesList = new List<Enemy>();

    public void PenetrateIntervalInit(float interval)
    {
        penetrateIntervalWait = new WaitForSeconds(interval);
    }

    void GiveDamage(Bullet bullet, Enemy enemy)
    {
        //弾からダメージ値を取得
        int damage = bullet.BulletDamage();

        //敵にダメージを与える
        enemy.GetDamage(damage);
    }

    //ダメージ処理
    public void bulletDamegeProcess(List<Collider> ColOpList, List<Enemy> AEIList, List<GameObject> enemyObjList, Bullet bullet)
    {
        Debug.Log("ダメージ");
        //int addEXP = 0;

        for(int i = 0; i < ColOpList.Count; i++)
        {
            int enListIndex = enemyObjList.IndexOf(ColOpList[i].gameObject);

            //Debug.Log("Index" + enListIndex);

            if(enListIndex < 0)
            {
                continue;
            }

            Enemy enemy = AEIList[enListIndex];

            GiveDamage(bullet, enemy);

            //addEXP += (int)enemy.EnemyEXP();

            //死んだら
            if (enemy.isDead())
            {
                //Debug.Log("加算前：" + totalPlayerEXP);

                //経験値加算
                deadEnemiesList.Add(enemy);
                //addEXP += (int)enemy.EnemyEXP();

                //Debug.Log("加算後：" + totalPlayerEXP);

                //敵の破壊
                EnemyRemove(AEIList, enemyObjList, enListIndex);
            }
        }
        //return addEXP;
    }

    void EnemyRemove(List<Enemy> AEIList, List<GameObject> enemyObjList, int number)
    {
        Destroy(AEIList[number].EnemyGameObject());
        AEIList.RemoveAt(number);
        enemyObjList.RemoveAt(number);
    }

    public IEnumerator PenetrateIntervalTimer()
    {
        isPenetrateActive = true;

        yield return penetrateIntervalWait;

        isPenetrateActive = false;
    }
}
