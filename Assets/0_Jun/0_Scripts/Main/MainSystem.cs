using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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

    [SerializeField]
    DamageManager DMManager;

    [SerializeField]
    EnemyInfoManager EIManager;

    [SerializeField]
    PlayerEXPManager PEXPManager;

    Camera PlayerCamera;

    LayerMask wallLayer = 1 << 6;

    LayerMask bulletHitLayer = 1 << 6 | 1 << 7;

    LayerMask playerLayer = 1 << 8;

    List<Bullet> ABIList;
    List<Enemy> AEIList;
    List<GameObject> AEOList;

    int gamePhase = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.main;

        ABIList = SIManager.AllBulletInfoList;
        AEIList = EIManager.AllEnemyInfoList;
        AEOList = EIManager.AllEnemyObjectList;

        EIManager.EnemyInfoInstantiate(DBManager.EnemyObj, DBManager.InstaPosObj);

        PIManager.InputIntervalInit(SIManager.fireInterval);

        gamePhase = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gamePhase)
        {
            //移動フェーズ
            case 1:

                //PMManager.NormalMove(
                //        PMManager.Player,
                //        PMManager.Player.transform.position,
                //        PIManager.MouseVector(PMManager.Player, PlayerCamera, PIManager.zAdjust),
                //        100,
                //        wallLayer,
                //        QueryTriggerInteraction.Collide
                //        );

                //gamePhase = 0;

                break;

            case 2:

                break;


            case 0:

                if(ABIList.Count > 0)
                {
                    for (int i = 0; i < ABIList.Count; i++)
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
                            BulletProcess(ABIList, i, bulletHitLayer, "Wall", SIManager.isPenetrate, PEXPManager.totalPlayerEXP);
                        }
                    }
                }

                //マウスの位置デバッグ
                //DBManager.mousePosDebug(debugManager.MouseObject, playerInputManager, PlayerCamera, 10);
                if (Input.GetKey(KeyCode.Space))
                {
                    BaseObjShotProcess(
                        PMManager.shotOriginObject,
                        PMManager.predictObject,
                        PMManager.baseBlocksArray[0],
                        wallLayer,
                        playerLayer
                        );
                    
                }
                else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    PMManager.predictObject.SetActive(false);
                    //プレイヤーの移動
                    if (Input.GetMouseButtonDown(0))
                    {
                        PMManager.NormalMove(
                        PMManager.Player,
                        PMManager.Player.transform.position,
                        PIManager.MouseVector(PMManager.Player, PlayerCamera, PIManager.zAdjust),
                        100,
                        wallLayer,
                        QueryTriggerInteraction.Collide
                        );
                    }
                }
                else
                {
                    PMManager.predictObject.SetActive(false);
                    if (Input.GetMouseButton(0))
                    {
                        if (PIManager.fireTimerIsActive)
                        {
                            return;
                        }
                        //発射方向を決める
                        Vector3 mouseVec = PIManager.MouseVector(PMManager.shotOriginObject, PlayerCamera, PIManager.zAdjust);

                        //SIManager.BulletInfoInstantiate(
                        //    SIManager.bulletTypeObjArray,
                        //    SIManager.bullet1,
                        //    PMManager.shotOriginObject.transform.position,
                        //    mouseVec,
                        //    SIManager.destroyDistance,
                        //    SIManager.isPenetrate
                        //    );

                        SIManager.BulletShotSimultaniously(
                            mouseVec,
                            SIManager.simultaniousNum,
                            SIManager.bulletTypeObjArray,
                            SIManager.bullet1,
                            PMManager.shotOriginObject.transform.position,
                            SIManager.destroyDistance,
                            SIManager.bulletAngle,
                            SIManager.isPenetrate
                            );

                        PIManager.StartCoroutine("FireTimer");

                    }
                }

                break;
        }
    }

    //弾が一回で行う処理　壁に衝突するか　敵に衝突するか　＋移動
    void BulletProcess(List<Bullet> ABIList, int number, LayerMask bHitLayer, string tagName, bool isPen, int totalPlayerEXP)
    {
        Bullet bullet = ABIList[number];
        Collider[] cols = CLManager.whatBulletCollide(bullet, bHitLayer);

        int totalEXP = totalPlayerEXP;

        //何かにぶつかっている
        if (cols.Length > 0)
        {
            List<Collider> colOponentList = CLManager.FindWhatYouWant(cols, tagName);

            //colsが初っ端から壁だった
            if(colOponentList.Count < 1)
            {
                //弾を破壊
                CLManager.BulletRemove(ABIList, number);

                //判定を行ったのでColOpListの中身を削除
                colOponentList.Clear();

                return;
            }

            //衝突したリストに壁があった
            if(colOponentList.Count < cols.Length)
            {
                //敵にダメージ判定
                DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet, totalEXP);

                //弾を破壊
                CLManager.BulletRemove(ABIList, number);

                //判定を行ったのでColOpListの中身を削除
                colOponentList.Clear();
                
                return;
            }
            //なかった
            else
            {
                //Debug.Log(colOponentList[0].name);

                //貫通でない
                if (!isPen)
                {
                    DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet, totalEXP);

                    //弾を破壊
                    CLManager.BulletRemove(ABIList, number);
                    return;
                }
                //貫通なら
                else
                {
                    DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet, totalEXP);
                }
                   
                //判定を行ったのでColOpListの中身を削除
                colOponentList.Clear();
            }
        }

        //レベルアップしたか
        if (PEXPManager.isPlayerLevelUp(PEXPManager.totalPlayerEXP, PEXPManager.playerLevel))
        {
            gamePhase = 2;
        }

        ABIList[number].BulletGetMove();
    }

    void BaseObjShotProcess(GameObject originObj, GameObject predictObj, GameObject baseObj, LayerMask rayHitLayer, LayerMask collideLayer)
    {
        Vector3 mouseVec = PIManager.MouseVector(originObj, PlayerCamera, PIManager.zAdjust);

        //オブジェクトを飛ばす場所を決定
        Vector3 pos = PMManager.BaseObjPos(
            mouseVec,
            originObj.transform.position,
            mouseVec,
            7,
            rayHitLayer);

        predictObj.SetActive(true);

        //予測オブジェクトをその位置へ
        PMManager.SetObjPos(pos, predictObj);

        if (Input.GetMouseButtonDown(0))
        {
            //オブジェクトの位置を決定
            predictObj.SetActive(false);

            if (!PMManager.isPlayerStandOnBaseObj(baseObj, collideLayer))
            {
                PMManager.SetObjPos(pos, baseObj);
            }
        }
    }
}
