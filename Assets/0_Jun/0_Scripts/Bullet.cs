using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    float bulletSpeed;
    Vector3 bulletMoveDir;
    float bulletDamage;
    GameObject bulletObject;

    public Bullet(float speed, float damage, Vector3 moveDir, GameObject bulletGameObj)
    {
        bulletSpeed = speed;
        bulletMoveDir = moveDir;
        bulletDamage = damage;
        bulletObject = bulletGameObj;
    }

    public void BulletGetMove()
    {
        Vector3 moveValue = bulletMoveDir * bulletSpeed;
        bulletObject.transform.position += moveValue;
    }
}
