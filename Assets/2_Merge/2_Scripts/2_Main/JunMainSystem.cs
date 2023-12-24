using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEditor.PlayerSettings;

public class JunMainSystem : MonoBehaviour
{
    [SerializeField]
    PlayerInputManager PIManager;

    [SerializeField]
    PlayerMoveManager PMManager;

    //[SerializeField]
    //DebugManager DBManager;

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

    [SerializeField]
    EnemyManager ENManager;

    [SerializeField]
    LevelManager LVManager;

    [SerializeField]
    PlayerHPManager HPManager;

    Camera PlayerCamera;

    LayerMask wallLayer = 1 << 6;

    LayerMask bulletHitLayer = 1 << 6 | 1 << 7;

    LayerMask playerLayer = 1 << 8;

    LayerMask enemyLayer = 1 << 7;

    List<Bullet> ABIList;
    List<GameObject> AEOList;

    List<Enemy> AEIList;

    int gamePhase = 0;

    GameObject Player;
    GameObject Fannel;
    GameObject ShotOrigin;

    private void Awake()
    {
        UIManager.LevelUpUIInit();
        UIManager.SliderMaxInit();
        PIManager.LineRendererInit();
        PEXPManager.EXPdebugTextInit();
        LVManager.LevelInit();

        DMManager.PenetrateIntervalInit(0.03f);
        HPManager.InvincibleIntervalInit(0.5f);
        HPManager.PlayerHPInit(100);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.main;

        ABIList = SIManager.AllBulletInfoList;
        AEIList = ENManager.AllEnemyInfoList;
        AEOList = ENManager.AllEnemyObjectList;

        PIManager.InputInterval(LVManager.fireIntervalLevelArray[LVManager.LevelofIndex(2)]);

        Player = PMManager.Player;
        Fannel = PMManager.Fannel;
        ShotOrigin = PMManager.shotOriginObject;

        ENManager.EnemyInit(
            Player,
            ENManager.enemySimultaniousNum,
            ENManager.enemySpawnRadius,
            ENManager.spawnMaxTime,
            ENManager.enemySpawnInterval
            );

        //LVManager.nowBullet = LVManager.bullet1;

        gamePhase = 0;

        PEXPManager.AccumulationEXP(2);

        LVManager.RewardInit();
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

                PEXPManager.EXPdebugText();

                //弾の処理
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
                            BulletProcess(ABIList, i, bulletHitLayer, "Wall", LVManager.penetrateLevelArray[LVManager.LevelofIndex(4)]);
                        }
                    }
                }

                //敵の処理
                if (AEIList.Count > 0)
                {
                    for (int i = 0; i < AEIList.Count; i++)
                    {
                        EnemyProcess(AEIList, i, Player);

                        if (AEIList[i].EnemyGameObject().transform.position.x < Player.transform.position.x)
                        {
                            AEIList[i].EnemyGameObject().transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
                        }
                        else
                        {
                            AEIList[i].EnemyGameObject().transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
                        }
                    }
                }

                HPManager.PlayerHPCheck(Player, 3, enemyLayer, AEOList, AEIList);

                if (HPManager.playerHP < 1)
                {
                    gamePhase = 3;
                    return;
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    Vector3 mouseVec = PIManager.MouseVector(ShotOrigin, PlayerCamera, PIManager.zAdjust);
                    Vector3 restVec = PIManager.RestrictVector(Player, mouseVec, 150);

                    FannelProcess(true, restVec);
                    LineDrawProcess(Fannel, restVec, wallLayer, 7);

                    BaseObjShotProcess(
                        restVec,
                        ShotOrigin,
                        PMManager.predictObject,
                        PMManager.baseBlocksArray[0],
                        wallLayer,
                        playerLayer,
                        9
                        );
                }
                else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Vector3 mouseVec = PIManager.MouseVector(ShotOrigin, PlayerCamera, PIManager.zAdjust);
                    Vector3 restVec = PIManager.RestrictVector(Player, mouseVec, 180);

                    FannelProcess(false, restVec);

                    PMManager.predictObject.SetActive(false);

                    if (!Physics.Raycast(ShotOrigin.transform.position, restVec, out RaycastHit hitInfo, 16, wallLayer))
                    {
                        //できない
                        LineDrawProcess(ShotOrigin, restVec, wallLayer, 0);
                        return;
                    }
                    else
                    {
                        //できる
                        LineDrawProcess(ShotOrigin, restVec, wallLayer, 16);

                        if (Input.GetMouseButtonDown(0))
                        {
                            PMManager.MoveAndRot(Player, hitInfo);
                        }
                    }

                    
                    //プレイヤーの移動
                    //if (Input.GetMouseButtonDown(0))
                    //{
                    //    PMManager.NormalMove(
                    //    Player,
                    //    Player.transform.position,
                    //    restVec,
                    //    20,
                    //    wallLayer,
                    //    QueryTriggerInteraction.Collide
                    //    );
                    //}
                }
                else
                {
                    Vector3 mouseVec = PIManager.MouseVector(ShotOrigin, PlayerCamera, PIManager.zAdjust);
                    Vector3 restVec = PIManager.RestrictVector(Player, mouseVec, 150);

                    FannelProcess(true, restVec);
                    LineDrawProcess(Fannel, restVec, wallLayer, 3);

                    PMManager.predictObject.SetActive(false);
                    if (Input.GetMouseButton(0))
                    {
                        if (PIManager.fireTimerIsActive)
                        {
                            return;
                        }
                        //発射方向を決める

                        SIManager.BulletShotSimultaniously(
                            restVec,
                            LVManager.simulNumLevelArray[LVManager.LevelofIndex(0)],
                            SIManager.bulletTypeObjArray,
                            LVManager.bulletDamageLevelArray[LVManager.LevelofIndex(3)],
                            Fannel.transform.position,
                            LVManager.destroyDistLevelArray[LVManager.LevelofIndex(1)],
                            LVManager.bulletAngleLevelArray[LVManager.LevelofIndex(5)],
                            LVManager.penetrateLevelArray[LVManager.LevelofIndex(4)]
                            );

                        PIManager.StartCoroutine("FireTimer");
                    }   
                }

                break;

            case 2:
                
                

                if (UIManager.selectedPanelnum > -1 && UIManager.onClick)
                {
                    LVManager.RewardSelectAndlevelUp(UIManager.selectedPanelnum);

                    LevelUpUIProcess(false);

                    PIManager.InputInterval(LVManager.fireIntervalLevelArray[LVManager.LevelofIndex(2)]);

                    gamePhase = 1;
                    Debug.Log(LVManager.rewardsLevelsArray[4]);
                }

                break;

            case 3:
                //死んだ時の処理を書く
                Debug.Log("ゲームオーバー");
                break;
        }
    }

    //弾が一回で行う処理　壁に衝突するか　敵に衝突するか　+移動
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
                    if (DMManager.isPenetrateActive)
                    {
                        ABIList[number].BulletGetMove();
                        return;
                    }

                    PEXPManager.totalPlayerEXP += DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet);

                    //レベルアップするかどうか
                    LevelUpCheckProcess(beforeLevel);

                    //判定を行ったのでColOpListの中身を削除
                    colOponentList.Clear();

                    DMManager.StartCoroutine("PenetrateIntervalTimer");
                }
            }
        }

        ABIList[number].BulletGetMove();
        //Debug.Log("うごいた");
    }

    void BaseObjShotProcess(Vector3 direction, GameObject originObj, GameObject predictObj, GameObject baseObj, LayerMask rayHitLayer, LayerMask collideLayer, float dist)
    {
        //Vector3 mouseVec = PIManager.MouseVector(originObj, PlayerCamera, PIManager.zAdjust);

        //オブジェクトを飛ばす場所を決定
        Vector3 pos = PMManager.BaseObjPos(
            originObj.transform.position,
            direction,
            dist,
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
        //Debug.Log("現在のレベル" + afterLevel);

        if (afterLevel > beforeLevel)
        {
            UIManager.SliderValueChange(UIManager.EXPSlider, 0);
            
            UIManager.selectedPanelnum = 0;
            UIManager.onClick = false;

            LVManager.RewardInit();

            LevelUpUIProcess(true);
            UIManager.RewardUISet(LVManager.infotoPanel, LVManager.rewardsLevelsArray);

            gamePhase = 2;
        }
    }

    void EnemyProcess(List<Enemy> AEIList, int number, GameObject target)
    {
        AEIList[number].EnemyNavMove(target.transform, AEIList[number].enemyObject.transform.position);   
    }

    void LineDrawProcess(GameObject originObj, Vector3 direction, LayerMask rayHitLayer, float dist)
    {
        Vector3 pos = PMManager.BaseObjPos(
            originObj.transform.position,
            direction,
            dist,
            rayHitLayer);

        PIManager.LineDraw(originObj, pos);
    }

    void LevelUpUIProcess(bool isGameStop)
    {
        AgentStopProcess(isGameStop);
        UIManager.LevelUpUIParent.SetActive(isGameStop);
    }

    void AgentStopProcess(bool isStop)
    {
        if (AEIList.Count > 0)
        {
            for (int i = 0; i < AEIList.Count; i++)
            {
                AEIList[i].agent.isStopped = isStop;
            }
        }
    }

    void FannelProcess(bool isFannel, Vector3 fannelVec)
    {
        if (isFannel)
        {
            Fannel.SetActive(true);
            Fannel.transform.position = PMManager.BaseObjPos(ShotOrigin.transform.position, fannelVec, 2, wallLayer);
        }
        else
        {
            Fannel.SetActive(false);
        }
    }
}
