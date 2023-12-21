using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    [SerializeField]
    PlayerManager plM;
    [SerializeField]
    EnemyGenerator enG;

    List<Enemy> EnList;
    public Transform playerPos;
    Vector3 enemyPos;

    // Start is called before the first frame update
    void Start()
    {
        //Vector3 enemyPos = new Vector3(0.0f,0.0f,0.0f);
        enemyPos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, Camera.main.nearClipPlane));
        enemyPos.z = enemyPos.y;
        enemyPos.y = 0;
        enG.spawnNormal(playerPos);
        EnList = enG.allEnemyInfoList;
        
    }

    // Update is called once per frame
    void Update()
    {
        plM.movePlayer();
        for(int i = 0; i < EnList.Count; i++)
        {
            EnList[i].EnemyNavMove(playerPos);

        }
        
        
        //EnList[0].EnemyMove(playerPos,enemyPos);
    }
}
