using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyManagerS : MonoBehaviour
{
    //追記：エネミー一覧
    [SerializeField]
    private GameObject enemyType0;
    [SerializeField]
    private GameObject enemyType1;
    [SerializeField]
    private GameObject enemyType2;
    [SerializeField]
    private GameObject enemyType3;
    [SerializeField]
    private GameObject enemyType4;
    [SerializeField]
    private GameObject enemyType5;
    //追記：エネミーのステータス、eSpeedはそれぞれが持つNavmeshのspeedに対応（EnemyS参照）
    Dictionary<string, float> Enemy0 = new Dictionary<string, float>() { { "eTypeNum", 0 }, {"eHP", 3}, { "eSpeed", 0.0f }, { "eDamage", 1 }, {"eEXP", 0}};
    Dictionary<string, float> Enemy1 = new Dictionary<string, float>() { { "eTypeNum", 1 }, {"eHP", 2}, { "eSpeed", 1.5f }, { "eDamage", 1 }, {"eEXP", 0}};
    Dictionary<string, float> Enemy2 = new Dictionary<string, float>() { { "eTypeNum", 2 }, {"eHP", 1}, { "eSpeed", 0.7f }, { "eDamage", 1 }, {"eEXP", 0}};
    Dictionary<string, float> Enemy3 = new Dictionary<string, float>() { { "eTypeNum", 3 }, {"eHP", 2}, { "eSpeed", 1.0f }, { "eDamage", 1 }, {"eEXP", 0}};
    Dictionary<string, float> Enemy4 = new Dictionary<string, float>() { { "eTypeNum", 4 }, {"eHP", 2}, { "eSpeed", 0.5f }, { "eDamage", 1 }, {"eEXP", 0}};
    Dictionary<string, float> Enemy5 = new Dictionary<string, float>() { { "eTypeNum", 5 }, {"eHP", 2}, { "eSpeed", 0.8f }, { "eDamage", 1 }, {"eEXP", 0}};


    [System.NonSerialized]
    public List<EnemyS> AllEnemyInfoList = new List<EnemyS>();

    [System.NonSerialized]
    public List<GameObject> AllEnemyObjectList = new List<GameObject>();

    [SerializeField]
    public GameObject CenterObject;

    [System.NonSerialized]
    public float enemySpawnRadius = 30;//追記：エネミーがスポーンするプレイヤーを中心とした外周の半径
    [SerializeField]
    public float spawnMissileRadius = 70;//追記：ミサイルがスポーンするプレイヤーを中心とした外周の半径

    //追記：敵のスポーン数(初期状態)
    private int missileNumbers = 3;
    private int Type1Num = 3;
    private int Type2Num = 5;
    private int Type3Num = 1;
    private int Type4Num = 1;
    private int Type5Num = 1;

    //追記：ミサイル含む全てのエネミーのコルーチン待機時間
    [SerializeField]
    public int enemySpawnInterval = 15;

    //追記：ミサイルの発射時インターバル
    [SerializeField]
    public float missileInterval = 20f;

    //追記：シーン右上のタイマー用
    [SerializeField]
    private TextMeshProUGUI timerText;
    private float seconds = 0f;
    private float oldseconds = 0f;
    private int minute = 0;

    //追記：プレイ時間
    private int playTime = 4;

    //追記：フェーズごとにミサイルがスポーンされる地点（プレイヤーとの距離）
    int firstOrbitGap = 10;
    int secondOrbitGap = 30;
    int thirdOrbitGap = 5;
    int fourthOrbitGap = 0;

    //追記：シーン右上のタイマー
    public void TimerInit()
    {
        
            seconds += Time.deltaTime;
            if(seconds >= 60f){
                minute ++;
                seconds -= 60;
            }
            if((int)seconds != (int)oldseconds){
                timerText.text = minute.ToString("00") + ":" + ((int) seconds).ToString("00");
            }
            oldseconds = seconds;
        
    }
    
    //追記：敵（ミサイル以外）の生成
    private void EnemyGenerate(Vector3 enemyPos, float enHP, float enSpeed, float enDamage, float enEXP, float enType)
    {
        
        GameObject enemyObj = null;
        float randomScale;
        switch(enType)
        {
            case 1:
                enemyObj = Instantiate(enemyType1,enemyPos,Quaternion.identity);
                randomScale = Random.Range(0.4f,0.5f);
                enemyObj.transform.localScale = new Vector3(randomScale,0,randomScale); 
                break;
            case 2:
                enemyObj = Instantiate(enemyType2,enemyPos,Quaternion.identity);
                randomScale = Random.Range(0.4f,0.5f);
                enemyObj.transform.localScale = new Vector3(randomScale,0,randomScale);
                break;
            case 3:
                enemyObj = Instantiate(enemyType3,enemyPos,Quaternion.identity);
                break;
            case 4:
                enemyObj = Instantiate(enemyType4,enemyPos,Quaternion.identity);
                break;
            case 5:
                enemyObj = Instantiate(enemyType5,enemyPos,Quaternion.identity);
                break;
        }
        EnemyS enemy = new EnemyS((int)enHP, enSpeed, enDamage,enEXP,enemyObj,Vector3.zero,false);
        AllEnemyInfoList.Add(enemy);
        AllEnemyObjectList.Add(enemy.EnemyGameObject());
    }

    //追記：単体生成（場所はプレイヤーを中心とした円周上ランダム）
    private void spawnNormal(Transform player,int enemyNum, float spawnRadius, Dictionary<string, float> enemyDic)
    {
        
        Vector3 center = player.transform.position;
        for(int i = 0; i < enemyNum ; i ++)
        {
            int angle = Random.Range(1,360);
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius;

            Vector3 enemyPos = center + new Vector3(x, 0, z);

            EnemyGenerate(enemyPos, enemyDic["eHP"], enemyDic["eSpeed"], enemyDic["eDamage"], enemyDic["eEXP"], enemyDic["eTypeNum"]);
        }
    }

    //追記：群れ生成（最初のエネミーの生成位置だけランダムで決めて、それ以降は同じ地点にスポーン）
    private void spawnSwarm(Transform player,int enemyNum, float spawnRadius, Dictionary<string, float> enemyDic)
    {
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

    //追記：ミサイルを生成
    private void MissileGenerate(Vector3 enemyPos,float enHP, float enSpeed, float enDamage, float enEXP, Vector3 playerPos, float enemyRot, float orbitGap)
    {
        
        
        // missileArrive ベクトルを計算
        Vector3 missileArriveNormal = playerPos - (enemyPos - playerPos);

        // ランダムな角度を生成（ミサイルの軌道に微妙なズレを起こしたい）
        float randomAngle = Random.Range(1f, orbitGap);

        // ランダムな角度をラジアンに変換
        float angleInRadians = randomAngle * Mathf.Deg2Rad;

        // ベクトルを指定された角度で回転させる
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
        Vector3 missileArrive = rotation * missileArriveNormal;

        GameObject enemyObj = Instantiate(enemyType0,enemyPos,Quaternion.Euler(90, 0, enemyRot)* rotation);

        EnemyS enemy = new EnemyS((int)enHP, enSpeed, enDamage,enEXP,enemyObj,missileArrive,true);
        AllEnemyInfoList.Add(enemy);
        AllEnemyObjectList.Add(enemy.EnemyGameObject());
    }

    //追記：ミサイルを投下（スポーン）
    private IEnumerator spawnMissile(Transform player, int missileNum, float spawnRadius, Dictionary<string, float> enemyDic, float missileInterval, float orbitGap)
    {
        Vector3 center = player.transform.position;

        for (int i = 0; i < missileNum - 1; i++)
        {
            int angle = Random.Range(1,360);
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius;
            Vector3 enemyPos = center + new Vector3(x, 0, z);
            MissileGenerate(enemyPos, enemyDic["eHP"], enemyDic["eSpeed"], enemyDic["eDamage"], enemyDic["eEXP"], center, angle, orbitGap);
            for(int j = 0; j < missileInterval; j++)
            {
                yield return null;
            }
        }
    }


    //追記：4フェーズの敵生成コルーチン（minuteの値が変わるとフェーズが切り替わる）
    public IEnumerator PhaseCoroutine(Transform player, float spawnRadius, float spawnInterval,int missileNum)
    {
        //float timer = 0f;
        
        while(minute < playTime)//
        {
            if(minute == 0)
            {
                //３０秒経過したらミサイルを放つ
                if(seconds > 29) StartCoroutine(spawnMissile(player,missileNum, spawnMissileRadius, Enemy0,missileInterval, firstOrbitGap));
                spawnNormal(player,Type1Num, spawnRadius, Enemy1);
                yield return new WaitForSeconds(spawnInterval);
            }
            else if(minute == 1)
            {
                //３０秒経過したらミサイルを放つ
                if(seconds > 29) StartCoroutine(spawnMissile(player,missileNum + 1, spawnMissileRadius, Enemy0,missileInterval, secondOrbitGap));
                spawnNormal(player,Type1Num, spawnRadius, Enemy1);
                spawnSwarm(player,Type2Num, spawnRadius, Enemy2);

                yield return new WaitForSeconds(spawnInterval);
            }
            else if(minute == 2)
            {
                //３０秒経過したらミサイルを放つ
                if(seconds > 29) StartCoroutine(spawnMissile(player,missileNum + 2, spawnMissileRadius, Enemy0,missileInterval, thirdOrbitGap));
                spawnNormal(player,Type1Num - 2, spawnRadius, Enemy1);
                spawnSwarm(player,Type2Num, spawnRadius, Enemy2);
                spawnNormal(player,Type3Num, spawnRadius, Enemy3);
                spawnNormal(player,Type4Num, spawnRadius, Enemy4);

                yield return new WaitForSeconds(spawnInterval);
            }
            else if(minute == 3)
            {
                //３０秒経過したらミサイルを放つ
                if(seconds > 29) StartCoroutine(spawnMissile(player,missileNum + 2, spawnMissileRadius, Enemy0,missileInterval, fourthOrbitGap));
                spawnNormal(player,Type1Num - 1, spawnRadius, Enemy1);
                spawnSwarm(player,Type2Num, spawnRadius, Enemy2);
                spawnNormal(player,Type3Num, spawnRadius, Enemy3);
                spawnNormal(player,Type4Num, spawnRadius, Enemy4);
                spawnNormal(player,Type5Num, spawnRadius, Enemy5);

                yield return new WaitForSeconds(spawnInterval);
            }
            else yield break;
        }   
    }

    //enemySimultaniousNumは削除
    public void EnemyInit(GameObject player, float enSpawnRadius, float spawnInterval)
    {
        Vector3 enemyPos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, Camera.main.nearClipPlane));
        enemyPos.z = enemyPos.y;
        enemyPos.y = 0;
        StartCoroutine(PhaseCoroutine(player.transform, enSpawnRadius, spawnInterval,missileNumbers));
    }
}
