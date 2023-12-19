using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    float bulletSpeed;
    Vector3 bulletMoveDir;
    float bulletDamage;

    public Bullet(float speed, Vector3 moveDir, float damage)
    {
        bulletSpeed = speed;
        bulletMoveDir = moveDir;
        bulletDamage = damage;
    }

    public Vector3 bulletMove()
    {
        Vector3 moveValue = bulletMoveDir * bulletSpeed;
        return moveValue;
    }
}
