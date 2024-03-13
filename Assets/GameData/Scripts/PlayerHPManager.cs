using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHPManager : MonoBehaviour
{
    [System.NonSerialized]
    public int playerHP;

    bool isInvincibleActive = false;

    WaitForSeconds invincibleTimeWait;

    public void PlayerHPInit(int HP)
    {
        playerHP = HP;
    }

    public void InvincibleIntervalInit(float interval)
    {
        invincibleTimeWait = new WaitForSeconds(interval);
    }

    void PlayerDamage(Enemy enemy)
    {
        //弾からダメージ値を取得
        int damage = (int)enemy.GiveDamage();
        
        //敵にダメージを与える
        playerHP -= damage;
    }

    public void PlayerHPCheck(GameObject playerObj, float hitRadius, LayerMask enemyLayer, Animator anim)
    {
       // Debug.Log(enemyObjList.Count);
        if (isInvincibleActive)
        {
            return;
        }
        
        Collider[] damagingEnemiesArray = Physics.OverlapSphere(playerObj.transform.position, hitRadius, enemyLayer);

        if(damagingEnemiesArray.Length < 1)
        {
            
            return;
        }
        
        for (int i = 0; i < damagingEnemiesArray.Length; i++)
        {
            
            //int enListIndex = enemyObjList.IndexOf(damagingEnemiesArray[i].gameObject);
            
            //if (enListIndex < 0)
            //{
                
            //    //Debug.Log(enemyObjList.Count);
            //    continue;
            //}
            
            Enemy enemy = damagingEnemiesArray[i].GetComponent<Enemy>();
            //Debug.Log(type);
            PlayerDamage(enemy);
            
            if (playerHP < 1)
            {
                break;
            }
        }

        anim.SetTrigger("PlayerDamage");
        StartCoroutine("InvincibleTimer");
    }

    public void PlayerHPCheckMissile(GameObject playerObj, float hitRadius, LayerMask enemyLayer, List<GameObject> enemyObjList, List<Enemy> MIList, Animator anim)
    {
        // Debug.Log(enemyObjList.Count);
        if (isInvincibleActive)
        {
            return;
        }

        Collider[] damagingEnemiesArray = Physics.OverlapSphere(playerObj.transform.position, hitRadius, enemyLayer);

        if (damagingEnemiesArray.Length < 1)
        {

            return;
        }

        for (int i = 0; i < damagingEnemiesArray.Length; i++)
        {

            int enListIndex = enemyObjList.IndexOf(damagingEnemiesArray[i].gameObject);

            if (enListIndex < 0)
            {

                //Debug.Log(enemyObjList.Count);
                continue;
            }

            Enemy enemy = MIList[enListIndex];
            //Debug.Log(type);
            PlayerDamage(enemy);

            if (playerHP < 1)
            {
                break;
            }
        }
        anim.SetTrigger("PlayerDamage");
        StartCoroutine("InvincibleTimer");
    }

    IEnumerator InvincibleTimer()
    {
        isInvincibleActive = true;

        yield return invincibleTimeWait;

        isInvincibleActive = false;
    }


}
