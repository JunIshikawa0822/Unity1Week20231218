using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    int enemyHP;
    float enemySpeed;
    float enemyDamage;
    float enemyEXP;
    public GameObject enemyObject;
    public UnityEngine.AI.NavMeshAgent agent;

    Collider enemyCollider;
    Vector3 enemyPos;


    public Enemy(int enHP, float enSpeed, float enDamage, float enEXP, GameObject enemyObj)
    {
        enemyHP = enHP;
        enemySpeed  = enSpeed;
        enemyDamage = enDamage;
        enemyEXP = enEXP;
        enemyObject = enemyObj;
        enemyPos = enemyObject.transform.position;
        agent = enemyObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyCollider = enemyObject.GetComponent<SphereCollider>();
        agent.updateRotation = false;
        //enemyMoveType = enMoveType;

       // enemyAnim = enAnim;
    }

    public float GiveDamage()
    {
        return enemyDamage;
    }

    public void GetDamage(int givenDamage)
    {
        enemyHP -= givenDamage;
        //Debug.Log(enemyHP);
    }

    public void EnemyMove(Transform playerPos,Vector3 enemyPos)
    {
        enemyPos = Vector3.MoveTowards(
            enemyPos,
            playerPos.transform.position,
            1.0f
        );
    }

    public Vector3 EnemyPos()
    {
        return enemyObject.transform.position;
    }

    public void EnemyNavMove(Transform playerPos,Vector3 enemyPos)
    {
        
        agent.destination = playerPos.transform.position;
   
    }

    public bool IsEnemyDestroy(Transform playerPos, Vector3 enemyPos)
    {
        if (Vector3.Distance(playerPos.transform.position, enemyPos) < agent.stoppingDistance + 1.0f)
        {
            return true;
        }
        else return false;
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

    public float EnemyEXP()
    {
        return enemyEXP;
    }

    public GameObject EnemyGameObject()
    {
        return enemyObject;
    }
}
