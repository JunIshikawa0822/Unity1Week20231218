using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    private EnemyAdmin enemyAdmin;

    public void EnemyManagerInit(EnemyAdmin _enemyAdmin)
    {
        enemyAdmin = _enemyAdmin;
    }

    void RemoveEnemy(Enemy enemy)
    {
        Destroy(enemy);
    }
}
