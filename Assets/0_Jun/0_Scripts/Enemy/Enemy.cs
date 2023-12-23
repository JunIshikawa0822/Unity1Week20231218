using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    float enemyHP;
    float enemySpeed;
    float enemyDamage;
    float enemyEXP;

    GameObject enemyObject;

    int enemyMoveType;
    //Animation enemyAnim;

    public Enemy(float enHp, float enSpeed, float enDamage, float enEXP, int enMoveType, GameObject enObject)
    {
        enemyHP = enHp;
        enemySpeed = enSpeed;
        enemyDamage = enDamage;
        enemyEXP = enEXP;

        enemyObject = enObject;
        enemyMoveType = enMoveType;

        //enemyAnim = enAnim;
    }

    public GameObject EnemyGameObject()
    {
        return enemyObject;
    }

    public float EnemyEXP()
    {
        return enemyEXP;
    }

    //ダメージをHPに与える
    public void GetDamage(float givenDamage)
    {
        enemyHP -= givenDamage;
        //Debug.Log(enemyHP);
    }

    public bool isDead()
    {
        if (enemyHP < 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
