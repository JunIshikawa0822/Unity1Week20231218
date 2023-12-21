using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    float enemyHP;
    float enemySpeed;
    float enemyDamage;
    float enemyEXP;

    int enemyMoveType;
    Animation enemyAnim;

    public Enemy(float enHp, float enSpeed, float enDamage, float enEXP, int enMoveType, Animation enAnim)
    {
        enemyHP = enHp;
        enemySpeed = enSpeed;
        enemyDamage = enDamage;
        enemyEXP = enEXP;
        enemyMoveType = enMoveType;

        enemyAnim = enAnim;
    }

    //ダメージ判定＆死んだかどうか
    public void GetDamage(float givenDamage)
    {
        enemyHP -= givenDamage;
    }
}
