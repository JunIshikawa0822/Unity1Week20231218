using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyInfoManager : MonoBehaviour
{
    [System.NonSerialized]
    public List<Enemy> AllEnemyInfoList = new List<Enemy>();

    [System.NonSerialized]
    public List<GameObject> AllEnemyObjectList = new List<GameObject>();

    public void EnemyInfoInstantiate(GameObject enemyObject, GameObject instaPosObj)
    {
        GameObject enemyObj = Instantiate(enemyObject, instaPosObj.transform.position, Quaternion.identity);
        Enemy enemy = new Enemy(3, 0, 0, 1, 0, enemyObj);

        AllEnemyInfoList.Add(enemy);
        AllEnemyObjectList.Add(enemy.EnemyGameObject());
    }
}
