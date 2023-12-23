using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGenerator : MonoBehaviour
{
    private NavMeshAgent agent;
    

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject enemySwarmPrefab;
    

    [System.NonSerialized]
    public List<Enemy> allEnemyInfoList = new List<Enemy>();

    float SpawnRadius;
    private void EnemyGenerate(Vector3 enemyPos,int enHP, float enSpeed, float enDamage, float enEXP,int enId)
    {
        if(enId == 0)
        {
            GameObject enemyObj = Instantiate(enemyPrefab,enemyPos,Quaternion.identity);
            Enemy enemy = new Enemy(enHP, enSpeed, enDamage,enEXP,enemyObj);
            allEnemyInfoList.Add(enemy);
        }
        else if(enId == 1)
        {
            GameObject enemyObj = Instantiate(enemySwarmPrefab,enemyPos,Quaternion.identity);
            Enemy enemy = new Enemy(enHP, enSpeed, enDamage,enEXP,enemyObj);
            allEnemyInfoList.Add(enemy);
        } 
        
        
        
    }
    private void spawnNormal(Transform player,int enemyNum)
    {
        SpawnRadius = 15.0f;
        Vector3 center = player.transform.position;
        for(int i = 0; i < enemyNum ; i ++)
        {
            float angle = 360/enemyNum * i;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * SpawnRadius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * SpawnRadius;

            Vector3 enemyPos = center + new Vector3(x, 0, z);

            EnemyGenerate(enemyPos,10, 1.0f, 1.0f, 1.0f,0);
        }
    }
    private void spawnSwarm(Transform player,int enemyNum)
    {
        SpawnRadius = 15.0f;
        Vector3 center = player.transform.position;
        int leaderPos = Random.Range(0,enemyNum - 1);
        for(int i = 0; i < enemyNum ; i ++)
        {
            
            float angle = 360/enemyNum * leaderPos;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * SpawnRadius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * SpawnRadius;

            Vector3 enemyPos = center + new Vector3(x, 0, z);

            EnemyGenerate(enemyPos,5, 1.0f, 1.0f, 1.0f,1);
        }
    }
    public IEnumerator SpawnCoroutine(Transform player,int enemyNum)
    {
        float timer = 0f;
        while(timer < 120f)
        {
            if(timer%4 == 0) spawnNormal(player,enemyNum);
            if(timer%8 == 0) spawnSwarm(player,enemyNum);

            yield return new WaitForSeconds(8f);

            timer += 8f; 
            enemyNum++;
            
        }
        
        
    }
}
