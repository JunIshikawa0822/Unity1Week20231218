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
    GameObject enemyObject;
    UnityEngine.AI.NavMeshAgent agent;
    Collider enemyCollider;
    //Transform EnemyPos;

    //int enemyMoveType;
    //Animation enemyAnim;

    public Enemy(int enHP, float enSpeed, float enDamage, float enEXP,GameObject enemyObj)
    {
        enemyHP = enHP;
        enemySpeed  = enSpeed;
        enemyDamage = enDamage;
        enemyEXP = enEXP;
        enemyObject = enemyObj;
        //EnemyPos = enemyObject.transform.position;
        agent = enemyObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyCollider = enemyObject.GetComponent<SphereCollider>();
        agent.updateRotation = false;
        //enemyMoveType = enMoveType;

       // enemyAnim = enAnim;
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
    public void EnemyMove(Transform playerPos,Vector3 enemyPos)
    {
        enemyPos = Vector3.MoveTowards(
            enemyPos,
            playerPos.transform.position,
            1.0f * Time.deltaTime
        );


    }

    public void EnemyNavMove(Transform playerPos)
    {
        agent.destination = playerPos.transform.position;
    }

    public void OnTriggerExit(Collider wall)
    {
        
        if(wall.CompareTag("Wall"))
        {
            enemyCollider.isTrigger = false;
            Debug.Log("hogehoge");

        }
        
    }


}
