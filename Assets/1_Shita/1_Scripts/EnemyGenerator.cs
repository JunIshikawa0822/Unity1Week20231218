using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGenerator : MonoBehaviour
{
    private NavMeshAgent agent;
    

    [SerializeField]
    private GameObject enemyPrefab;

    [System.NonSerialized]
    public List<Enemy> allEnemyInfoList = new List<Enemy>();

    float SpawnRadius;
    // Start is called before the first frame update
    private void EnemyGenerate(Vector3 enemyPos,int enHP, float enSpeed, float enDamage, float enEXP)
    {
        
        GameObject enemyObj = Instantiate(enemyPrefab,enemyPos,Quaternion.identity);
        
        
        Enemy enemy = new Enemy(enHP, enSpeed, enDamage,enEXP,enemyObj);
        allEnemyInfoList.Add(enemy);
    }
    public void spawnNormal(Transform player)
    {
        SpawnRadius = 20.0f;
        Vector3 center = player.transform.position;
        for(int i = 0; i < 10 ; i ++)
        {
            float angle = 360/10 * i;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * SpawnRadius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * SpawnRadius;

            Vector3 enemyPos = center + new Vector3(x, 0, z);

            EnemyGenerate(enemyPos,10, 1.0f, 1.0f, 1.0f);
        }
    }
}
