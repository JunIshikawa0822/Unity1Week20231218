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

    [SerializeField]
    UserInterfaceManager UIManager;

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

        PIManager.InputInterval(SIManager.fireInterval);

        UIManager.SliderMaxInit();

        //Debug.Log(PEXPManager.EXPtoLevel());

        gamePhase = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gamePhase)
        {
            //移動フェーズ
            case 0:
                gamePhase = 1;
                break;

            case 1:
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

                if (ABIList.Count > 0)
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
                            BulletProcess(ABIList, i, bulletHitLayer, "Wall", SIManager.isPenetrate);
                        }
                    }
                }

                break;

            case 2:
                
                break;
        }
    }

    //弾が一回で行う処理　壁に衝突するか　敵に衝突するか　＋移動
    void BulletProcess(List<Bullet> ABIList, int number, LayerMask bHitLayer, string tagName, bool isPen)
    {
        Bullet bullet = ABIList[number];
        Collider[] cols = CLManager.whatBulletCollide(bullet, bHitLayer);

        int beforeLevel = PEXPManager.EXPtoLevel();
        //Debug.Log("加算前レベル" + beforeLevel);

        //Debug.Log("加算前" + PEXPManager.totalPlayerEXP);

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
                PEXPManager.totalPlayerEXP += DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet);

                //レベルアップするかどうか
                LevelUpCheckProcess(beforeLevel);

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
                    PEXPManager.totalPlayerEXP += DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet);

                    //レベルアップするかどうか
                    LevelUpCheckProcess(beforeLevel);

                    //弾を破壊
                    CLManager.BulletRemove(ABIList, number);

                    //判定を行ったのでColOpListの中身を削除
                    colOponentList.Clear();
                    return;
                }
                //貫通なら
                else
                {
                    PEXPManager.totalPlayerEXP += DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet);

                    //レベルアップするかどうか
                    LevelUpCheckProcess(beforeLevel);

                    //判定を行ったのでColOpListの中身を削除
                    colOponentList.Clear();
                }
            }
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

    void LevelUpCheckProcess(int beforeLevel)
    {
        //print(PEXPManager.totalPlayerEXP);
        UIManager.SliderValueChange(UIManager.EXPSlider, PEXPManager.BarPersent(beforeLevel));

        int afterLevel = PEXPManager.EXPtoLevel();
        //Debug.Log("加算後レベル" + afterLevel);

        if (afterLevel > beforeLevel)
        {
            gamePhase = 2;
            Debug.Log("フェーズ移行");
        }
    }
}
