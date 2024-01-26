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
    Collider enemyCollider;
    Vector3 enemyPos;

    //追記
    public bool isMissile;//ミサイルかどうか
    public LineRenderer orbitRenderer;//ミサイルの軌跡描画用
    Vector3 missileArrive;//ミサイルの目的地
    private bool isFinish;//ミサイルの挙動が停止したか
    private bool isShot;//ミサイルが発射したか
    public Animator damageAnim;//エネミーのダメージ演出

    public EnemyS(int enHP, float enSpeed, float enDamage, float enEXP, GameObject enemyObj, Vector3 misAr, bool IsMissile)
    {
        
        enemyHP = enHP;
        enemyDamage = enDamage;
        enemyEXP = enEXP;
        enemyObject = enemyObj;
        enemyPos = enemyObject.transform.position;
        agent = enemyObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyCollider = enemyObject.GetComponent<SphereCollider>();

        //追記：初期化
        isMissile = IsMissile;
        missileArrive = misAr;
        damageAnim = enemyObject.GetComponentInChildren<Animator>();
        agent.updateRotation = false;
        isFinish = false;
        isShot = false;
        DOTween.SetTweensCapacity(1500, 50);
        agent.speed  = enSpeed;
    }

    public void GetDamage(int givenDamage)
    {
        enemyHP -= givenDamage;
        damageAnim.SetTrigger("Damage");//追記：ダメージ演出
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

    //追記：ミサイルの挙動
    public void MissileMove(GameObject gameObject)
    {
        //現在の位置を取得
        Vector3 currenteEnemyPos = gameObject.transform.position;
        orbitRenderer = enemyObject.GetComponent<LineRenderer>();
        orbitRenderer.startWidth = 0.1f; 
        orbitRenderer.endWidth = 0.1f;
        
        //軌跡の描画
        isShot = true;
        OrbitDisplay(currenteEnemyPos);
        gameObject.transform.DOMove(missileArrive, 6f).SetEase(Ease.Linear)
            .OnComplete(() => 
            {
                gameObject.transform.DOKill();
                isFinish = true;
            });
   
    }
    //追記：ミサイルの挙動が停止したかどうか
    public bool IsMissileFinish()
    {
        return isFinish;
    }
    public bool IsShot()
    {
        return isShot;
    }
    //追記：軌跡の描画
    private void OrbitDisplay(Vector3 currentEnemyPos)
    {
        var positions = new Vector3[]{
            currentEnemyPos,               
            missileArrive,               
        };
        orbitRenderer.SetPositions(positions);
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
