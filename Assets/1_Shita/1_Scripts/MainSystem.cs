using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    [SerializeField]
    PlayerManager plM;
    [SerializeField]
    EnemyManager enG;

    List<Enemy> EnList;
    public Transform playerPos;
    Vector3 enemyPos;

    // Start is called before the first frame update
    void Start()
    {
        
        //enemyPos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, Camera.main.nearClipPlane));
        //enemyPos.z = enemyPos.y;
        //enemyPos.y = 0;
        //StartCoroutine(enG.SpawnCoroutine(playerPos,5, 30));
        //EnList = enG.AllEnemyInfoList;  
    }

    // Update is called once per frame
    void Update()
    {
        plM.movePlayer();
        for(int i = 0; i < EnList.Count; i++)
        {
            EnList[i].EnemyNavMove(playerPos, EnList[i].enemyObject.transform.position);
            //if(EnList[i].IsEnemyDestroy(playerPos,EnList[i].enemyObject.transform.position)) 
            //{
            //    if(Input.GetKeyUp("space"))
            //    {
            //        if(EnList[i].GetDamage(5))
            //        {
            //            Destroy(EnList[i].enemyObject);
            //            EnList.RemoveAt(i);
            //        }
            //    }
            //}
        }
    }
}
