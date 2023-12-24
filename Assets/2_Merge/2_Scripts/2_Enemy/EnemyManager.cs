using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    private NavMeshAgent agent;
    

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject enemySwarmPrefab;

    Dictionary<string, float> Enemy1 = new Dictionary<string, float>() { { "eTypeNum", 0 }, {"eHP", 3}, { "eSpeed", 0.2f }, { "eDamage", 1 }, {"eEXP", 1}};
    Dictionary<string, float> Enemy2 = new Dictionary<string, float>() { { "eTypeNum", 1 }, {"eHP", 3}, { "eSpeed", 0.2f }, { "eDamage", 1 }, {"eEXP", 1}};

    [System.NonSerialized]
    public List<Enemy> AllEnemyInfoList = new List<Enemy>();

    [System.NonSerialized]
    public List<GameObject> AllEnemyObjectList = new List<GameObject>();

    float spawnRadius = 30;

    int enemySimultaniousNum = 2;

    private void EnemyGenerate(Vector3 enemyPos, float enHP, float enSpeed, float enDamage, float enEXP, float enType)
    {
        if (enType == 0)
        {
            GameObject enemyObj = Instantiate(enemyPrefab,enemyPos,Quaternion.identity);
            Enemy enemy = new Enemy((int)enHP, enSpeed, enDamage,enEXP,enemyObj);
            AllEnemyInfoList.Add(enemy);
            AllEnemyObjectList.Add(enemy.EnemyGameObject());
        }
        else if(enType == 1)
        {
            GameObject enemyObj = Instantiate(enemySwarmPrefab,enemyPos,Quaternion.identity);
            Enemy enemy = new Enemy((int)enHP, enSpeed, enDamage,enEXP,enemyObj);
            AllEnemyInfoList.Add(enemy);
            AllEnemyObjectList.Add(enemy.EnemyGameObject());
        }     
    }
    private void spawnNormal(Transform player,int enemyNum, float spawnRadius, Dictionary<string, float> enemyDic)
    {
        
        Vector3 center = player.transform.position;
        for(int i = 0; i < enemyNum ; i ++)
        {
            float angle = 360/enemyNum * i;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius;

            Vector3 enemyPos = center + new Vector3(x, 0, z);

            EnemyGenerate(enemyPos, enemyDic["eHP"], enemyDic["eSpeed"], enemyDic["eDamage"], enemyDic["eEXP"], enemyDic["eTypeNum"]);
        }
    }
    private void spawnSwarm(Transform player,int enemyNum, float spawnRadius, Dictionary<string, float> enemyDic)
    {
        //SpawnRadius = 30.0f;
        Vector3 center = player.transform.position;
        int leaderPos = Random.Range(0,enemyNum - 1);
        for(int i = 0; i < enemyNum ; i ++)
        {
            
            float angle = 360/enemyNum * leaderPos;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius;

            Vector3 enemyPos = center + new Vector3(x, 0, z);

            EnemyGenerate(enemyPos, enemyDic["eHP"], enemyDic["eSpeed"], enemyDic["eDamage"], enemyDic["eEXP"], enemyDic["eTypeNum"]);
        }
    }
    public IEnumerator SpawnCoroutine(Transform player,int enemyNum, float spawnRadius)
    {
        float timer = 0f;
        while(timer < 120f)
        {
            if(timer%4 == 0) spawnNormal(player,enemyNum, spawnRadius, Enemy1);
            if(timer%8 == 0) spawnSwarm(player,enemyNum, spawnRadius, Enemy2);

            yield return new WaitForSeconds(8f);

            timer += 8f; 
            enemyNum++;
        }   
    }

    public void EnemyInit(GameObject player)
    {
        Vector3 enemyPos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, Camera.main.nearClipPlane));
        enemyPos.z = enemyPos.y;
        enemyPos.y = 0;
        StartCoroutine(SpawnCoroutine(player.transform, enemySimultaniousNum, spawnRadius));
    }
}
