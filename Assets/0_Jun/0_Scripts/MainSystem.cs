using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    [SerializeField]
    PlayerInputManager PIManager;

    [SerializeField]
    PlayerMoveManager PMManager;

    [SerializeField]
    DebugManager DBManager;

    [SerializeField]
    ShotInfoManager SIManager;

    [SerializeField]
    CollideManager CLManager;

    Camera PlayerCamera;

    LayerMask wallLayerMask = 1 << 6;

    LayerMask bulletHitLayer = 1 << 6 | 1 << 7;

    List<Bullet> ABIList;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.main;
        ABIList = SIManager.allBulletInfoList;
    }

    // Update is called once per frame
    void Update()
    {
        //マウスの位置デバッグ
        //DBManager.mousePosDebug(debugManager.MouseObject, playerInputManager, PlayerCamera, 10);
        if(ABIList.Count > 0)
        {
            for(int i = 0; i < ABIList.Count; i++)
            {
                //一定距離飛んでいる
                if (ABIList[i].isDestroyByDis())
                {
                    CLManager.BulletRemove(ABIList, i);
                    continue;
                }
                //一定距離飛んでいない
                else
                {
                    //そのまま飛ばす
                    BulletProcess(ABIList, i, bulletHitLayer, CLManager.CollideEnemyList, "Enemy");
                }       
            }
        }

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            //プレイヤーの移動
            if (Input.GetMouseButtonDown(0))
            {
                PMManager.NormalMove(
                PMManager.Player,
                PMManager.Player.transform.position,
                PIManager.MouseVector(PMManager.Player, PlayerCamera, 10),
                100,
                wallLayerMask,
                QueryTriggerInteraction.Collide
                );
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseVec = PIManager.MouseVector(PMManager.Player, PlayerCamera, PIManager.zAdjust);

                //SIManager.BulletInfoInstantiate(
                //    SIManager.bulletTypeObjArray,
                //    SIManager.bullet1,
                //    Player.transform.position,
                //    mouseVec,
                //    SIManager.destroyDistance
                //    );

                SIManager.BulletShotSimultaniously(
                    mouseVec,
                    SIManager.simultaniousNum,
                    SIManager.bulletTypeObjArray,
                    SIManager.bullet1,
                    PMManager.shotOriginObject.transform.position,
                    SIManager.destroyDistance,
                    SIManager.bulletAngle,
                    PIManager.zAdjust
                    );
            }
        }
    }

    void BulletProcess(List<Bullet> ABIList, int number, LayerMask bHitLayer, List<Collider> ColOponentList, string tagName)
    {
        bool isCollideWall = CLManager.BulletCollide(ABIList[number], bHitLayer, ColOponentList, tagName);

        //壁にぶつかっている
        if (isCollideWall)
        {
            //弾を破壊
            CLManager.BulletRemove(ABIList, number);
            return;
        }
        //壁にまだぶつかっていない
        else
        {
            //敵にぶつかっている
            if (ColOponentList.Count > 0)
            {
                //ダメージ処理
            }
        }

        ABIList[number].BulletGetMove();
    }
}
