using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManagerS : MonoBehaviour
{
    public AudioClip damagesound;
    public AudioClip destroysound;
    [SerializeField]
    SoundManager SManager;

    [System.NonSerialized]
    public bool isPenetrateActive = false;
    //float fireInterval;
    WaitForSeconds penetrateIntervalWait;

    public void PenetrateIntervalInit(float interval)
    {
        penetrateIntervalWait = new WaitForSeconds(interval);
    }

    public List<EnemyS> deadEnemiesList = new List<EnemyS>();

    void GiveDamage(int damageValue, EnemyS enemy)
    {

        //敵にダメージを与える
        enemy.GetDamage(damageValue);

        SManager.MakeSound(damagesound, 0.2f);
    }

    //ダメージ処理
    public void DamegeProcess(List<Collider> ColOpList, List<EnemyS> AEIList, List<GameObject> enemyObjList, int damageValue)
    {
        //int addEXP = 0;

        for (int i = 0; i < ColOpList.Count; i++)
        {
            int enListIndex = enemyObjList.IndexOf(ColOpList[i].gameObject);

            //Debug.Log("Index" + enListIndex);

            if (enListIndex < 0)
            {
                continue;
            }

            EnemyS enemy = AEIList[enListIndex];

            GiveDamage(damageValue, enemy);

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

    void EnemyRemove(List<EnemyS> AEIList, List<GameObject> enemyObjList, int number)
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
