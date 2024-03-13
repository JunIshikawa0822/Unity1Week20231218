using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet:MonoBehaviour
{
    private float bulletSpeed;
    private Vector3 bulletMoveDir;
    private int bulletDamage;
    private GameObject bulletObject;

    private float moveDistance;
    private float destroyDistance;

    private bool isPenetrate;
    private LayerMask bulletHitLayer;
    private string wall;

    private DamageManager damageManager;
    //private PlayerEXPManager playerEXPManager;

    public Bullet(float speed, int damage, Vector3 moveDir, GameObject bulletGameObj, float destroyDist, bool isPen, LayerMask bHLayer)
    {
        bulletSpeed = speed;
        bulletMoveDir = moveDir;
        bulletDamage = damage;
        bulletObject = bulletGameObj;
        moveDistance = 0;
        destroyDistance = destroyDist;
        isPenetrate = isPen;
        bulletHitLayer = bHLayer;   
    }

    public void BulletInit(DamageManager _damageManager)
    {
        damageManager = _damageManager;
        //playerEXPManager = _playerEXPManager;
    }
    
    //動かし続ける
    public void BulletGetMove()
    {
        Vector3 moveValue = bulletMoveDir * bulletSpeed;
        bulletObject.transform.position += moveValue;
        moveDistance += moveValue.magnitude;
    }

    //GameObjectの情報を返す
    public GameObject BulletGameObject()
    {
        return bulletObject;
    }

    public int BulletDamage()
    {
        return bulletDamage;
    }

    public bool isBulletPenetrate()
    {
        return isPenetrate;
    }

    //一定距離飛んだら合図
    public bool isDestroyByDis()
    {
        if(moveDistance < destroyDistance)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void Update()
    {
        if (isDestroyByDis())
        {
            Break();
        }

        Collider[] colArray = Physics.OverlapSphere(transform.position, transform.lossyScale.x / 2, bulletHitLayer);

        for (int i = 0; i < colArray.Length; i++)
        {
            if (colArray.Length < 0)
            {
                continue;
            }

            if (colArray[i].gameObject.tag == wall)
            {
                Break();
                break;
            }

            if(colArray.GetType() == typeof(Enemy))
            {
                Enemy enemy = colArray[i].GetComponent<Enemy>();
                //ダメージを与える
                damageManager.GiveDamage(this, enemy);

                //if (damageManager.isEnemyDead(enemy))
                //{
                //    //経験値加算
                //    playerEXPManager.GetEXP(enemy);

                //    //敵の破壊
                //    EnemyRemove(enemy);
                //}

                if (isPenetrate)
                {
                    continue;
                }
                else
                {
                    damageManager.StartCoroutine("PenetrateIntervalTimer");
                    Break();
                }
            }

            BulletGetMove();
        }

        //void EnemyRemove(Enemy enemy)
        //{
        //    //要修正
        //    Destroy(enemy);
        //}

        void Break()
        {
            damageManager.BulletRemove(this);
        }
    }
}
