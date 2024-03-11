using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    float bulletSpeed;
    Vector3 bulletMoveDir;
    int bulletDamage;
    GameObject bulletObject;

    float moveDistance;
    float destroyDistance;

    bool isPenetrate;

    public Bullet(float speed, int damage, Vector3 moveDir, GameObject bulletGameObj, float destroyDist, bool isPen)
    {
        bulletSpeed = speed;
        bulletMoveDir = moveDir;
        bulletDamage = damage;
        bulletObject = bulletGameObj;
        moveDistance = 0;
        destroyDistance = destroyDist;
        isPenetrate = isPen;
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
}
