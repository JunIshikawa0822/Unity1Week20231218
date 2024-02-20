using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyS : MonoBehaviour
{
    int enemyHP;
    float enemySpeed;
    float enemyDamage;
    float enemyEXP;
    public GameObject enemyObject;
    public UnityEngine.AI.NavMeshAgent agent;
    public LineRenderer orbitRenderer;//ミサイルの軌跡
    Vector3 missileArrive;
    private bool isFinish;
    private bool isShot;
    public Animator damageAnim;

    Collider enemyCollider;
    Vector3 enemyPos;

    public float GiveDamage()
    {
        return enemyDamage;
    }


    public EnemyS(int enHP, float enSpeed, float enDamage, float enEXP, GameObject enemyObj, Vector3 misAr)
    {
        DOTween.SetTweensCapacity(1500, 50);
        enemyHP = enHP;
        
        enemyDamage = enDamage;
        enemyEXP = enEXP;
        enemyObject = enemyObj;
        enemyPos = enemyObject.transform.position;
        agent = enemyObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed  = enSpeed;
        missileArrive = misAr;
        damageAnim = enemyObject.GetComponentInChildren<Animator>();
        enemyCollider = enemyObject.GetComponent<SphereCollider>();
        agent.updateRotation = false;
        isFinish = false;
        isShot = false;

        //enemyMoveType = enMoveType;

       // enemyAnim = enAnim;
    }

    public void GetDamage(int givenDamage)
    {     
        damageAnim.SetTrigger("Damage");
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

    public void MissileMove(GameObject gameObject)
    {
        Vector3 currenteEnemyPos = gameObject.transform.position;
        orbitRenderer = enemyObject.GetComponent<LineRenderer>();
        orbitRenderer.startWidth = 0.1f; 
        orbitRenderer.endWidth = 0.1f;
        
        isShot = true;
        OrbitDisplay(currenteEnemyPos);
        gameObject.transform.DOMove(missileArrive, 6f).SetEase(Ease.Linear)
            .OnComplete(() => 
            {
                gameObject.transform.DOKill();
                isFinish = true;
            });
   
    }
    public bool IsMissileFinish()
    {
        return isFinish;
    }
    public bool IsShot()
    {
        return isShot;
    }
    private void OrbitDisplay(Vector3 currentEnemyPos)
    {
        var positions = new Vector3[]{
            currentEnemyPos,               
            missileArrive,               
        };
        orbitRenderer.SetPositions(positions);
        // orbitRenderer.SetPosition(0, enemyPos);
        // orbitRenderer.SetPosition(1, missileArrive);
    }
    // public bool IsShot()
    // {
    //     return
    // }

    // public bool IsMissileFinish(GameObject gameObject, List<EnemyS> MIList, List<GameObject> missileObjList, int number)
    // {
    //     Destroy(gameObject);
    //     MIList.RemoveAt(number);
    //     missileObjList.RemoveAt(number);
    // }

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
