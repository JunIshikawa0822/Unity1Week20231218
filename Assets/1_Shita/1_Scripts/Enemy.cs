using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy 
{
    int enemyHP;
    float enemySpeed;
    float enemyDamage;
    float enemyEXP;

    int enemyMoveType;
    Animation enemyAnim;

    public Enemy(int enHP, float enSpeed, float enDamage, float enEXP, int enMoveType, Animation enAnim)
    {
        enemyHP = enHP;
        enemySpeed  = enSpeed;
        enemyDamage = enDamage;
        enemyEXP = enEXP;
        enemyMoveType = enMoveType;

        enemyAnim = enAnim;
    }

    public bool GetDamage(int givenDamage)
    {
        enemyHP -= givenDamage;
        if(enemyHP < 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
