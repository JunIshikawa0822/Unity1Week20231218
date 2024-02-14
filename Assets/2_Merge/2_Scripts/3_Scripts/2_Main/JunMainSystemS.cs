using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEditor.PlayerSettings;

public class JunMainSystemS : MonoBehaviour
{
    [SerializeField]
    PlayerInputManagerS PIManager;

    [SerializeField]
    PlayerMoveManager PMManager;

    //[SerializeField]
    //DebugManager DBManager;

    //[SerializeField]
    //ShotInfoManage SIManager;

    //[SerializeField]
    //CollideManagerS CLManager;

    //[SerializeField]
    //DamageManagerS DMManager;

    [SerializeField]
    EnemyInfoManager EIManager;

    [SerializeField]
    PlayerEXPManagerS PEXPManager;

    [SerializeField]
    UserInterfaceManager UIManager;

    [SerializeField]
    EnemyManagerS ENManager;

    //[SerializeField]
    //LevelManager LVManager;

    [SerializeField]
    PlayerHPManager HPManager;

    [SerializeField]
    SoundManager SManager;

    [SerializeField]
    PlayerAnimationManager PAManager;

    [SerializeField]
    AttackAdmin attackAdmin;

    [SerializeField]
    AttackObjectAdmin attackObjectAdmin;

    Camera PlayerCamera;
    Animator PlayerAnimator;

    LayerMask wallLayer = 1 << 6;

    LayerMask bulletHitLayer = 1 << 6 | 1 << 7;

    LayerMask playerLayer = 1 << 8;

    LayerMask enemyLayer = 1 << 7;

    LayerMask missileLayer = 1 << 9;

    List<Bullet> ABIList;

    List<GameObject> AEOList;
    List<EnemyS> AEIList;
    List<GameObject> MOList;
    List<EnemyS> MIList;


    int gamePhase = 0;

    GameObject Player;
    GameObject Fannel;
    GameObject ShotOrigin;
    GameObject ShotOrigin2;
    //S_Edit
    public GameObject player_s;
    private Animator Anim;

    public float warpdelay;

    //bool inputgetmouse;

    //bool IsShot;

    private void Awake()
    {
        UIManager.LevelUpUIInit();
        UIManager.SliderMaxInit();
        PIManager.LineRendererInit();
        PEXPManager.EXPdebugTextInit();
        //LVManager.LevelInit();
        attackAdmin.LVManager.LevelInit();

        attackObjectAdmin.DMManager.PenetrateIntervalInit(0.03f);
        HPManager.InvincibleIntervalInit(0.5f);
        HPManager.PlayerHPInit(100);
    }

    // Start is called before the first frame update
    void Start()
    {
        //S_Edit
        //IsShot = true;
        SManager.AudioInit();
        PlayerAnimator = PAManager.AnimationObject.GetComponent<Animator>();

        PlayerCamera = Camera.main;

        ABIList = attackAdmin.SIManager.AllBulletInfoList;
        AEIList = ENManager.AllEnemyInfoList;
        AEOList = ENManager.AllEnemyObjectList;

        MIList = ENManager.MissileInfoList;
        MOList = ENManager.MissileObjectList;

        attackAdmin.AttackIntervalInit();

        Player = PMManager.Player;
        Fannel = PMManager.Fannel;
        ShotOrigin = PMManager.shotOriginObject;
        ShotOrigin2 = PMManager.shotOriginObject2;

        ENManager.EnemyInit(
            Player,
            //ENManager.enemySimultaniousNum,
            ENManager.enemySpawnRadius,
            ENManager.spawnMaxTime,
            ENManager.enemySpawnInterval
            );

        //LVManager.nowBullet = LVManager.bullet1;

        gamePhase = 0;

        PEXPManager.AccumulationEXP(2);

   
    }

    // Update is called once per frame
    void Update()
    {
        ENManager.TimerInit();
        switch (gamePhase)
        {
            //移動フェーズ
            case 0:
                gamePhase = 1;
                break;

            case 1:

                UIManager.TextSet(UIManager.levelText, "Lv : ", PEXPManager.EXPtoLevel());


                //弾の処理
                if (ABIList.Count > 0)
                {
                    for (int i = 0; i < ABIList.Count; i++)
                    {
                        //一定距離飛んでいる
                        if (ABIList[i].isDestroyByDis())
                        {
                            attackObjectAdmin.BulletRemove(ABIList, i);
                            continue;
                        }
                        //一定距離飛んでいない
                        else
                        {
                            int beforeLevel = PEXPManager.EXPtoLevel();
                            //そのまま飛ばす
                            attackObjectAdmin.BulletProcess(ABIList, AEIList, AEOList, i, bulletHitLayer, "Wall", attackAdmin.LVManager.penetrateLevelArray[attackAdmin.LVManager.LevelofIndex(4)]);

                            GetEXP(attackObjectAdmin.DMManager.deadEnemiesList);
                            LevelUpCheckProcess(beforeLevel);
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
                //ミサイルの処理
                
                    if (MIList.Count > 0)
                    {
                        for (int i = 0; i < MIList.Count; i++)
                        {
                            MissileProcess(MIList, i);

                            // if (AEIList[i].EnemyGameObject().transform.position.x < Player.transform.position.x)
                            // {
                            //     AEIList[i].EnemyGameObject().transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
                            // }
                            // else
                            // {
                            //     AEIList[i].EnemyGameObject().transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
                            // }
                        }
                    }
                //Debug.Log(MOList.Count);
                HPManager.PlayerHPCheck(Player, 2, enemyLayer, AEOList, AEIList);
                HPManager.PlayerHPCheckMissile(Player, 2, missileLayer, MOList, MIList);
                UIManager.SliderValueChange(UIManager.HPSlider, HPManager.playerHP);

                if (HPManager.playerHP < 1)
                {
                    gamePhase = 3;
                    return;
                }

                if (Input.GetKey(KeyCode.Space))
                {

                    
                    PlayerAnimator.SetBool("WarpStart", true);
                    Vector3 mouseVec = PIManager.MouseVector(ShotOrigin, PlayerCamera, PIManager.zAdjust);
                    Vector3 restVec = PIManager.RestrictVector(Player, mouseVec, 150);

                    FannelProcess(false, restVec, ShotOrigin, 2);
                    PMManager.predictObject.SetActive(false);

                    if (!Physics.Raycast(ShotOrigin.transform.position, restVec, out RaycastHit hitInfo, 16, wallLayer))
                    {
                        //できない
                        LineDrawProcess(Fannel, restVec, wallLayer, 0);
                        return;
                    }
                    else
                    {
                        //できる
                        LineDrawProcess(Fannel, restVec, wallLayer, 16);

                        if (Input.GetMouseButtonDown(0))
                        {
                            //Invokeここに書けるよ
                            //PMManager.MoveAndRot(Player, hitInfo);
                            StartCoroutine(Move(Player, hitInfo, warpdelay));

                        }
                    }
                
                }else if (Input.GetKeyUp(KeyCode.Space))
                {
                    PlayerAnimator.SetBool("WarpStart", false);
                }
                else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    //Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                    Vector3 mouseVec = PIManager.MouseVector(ShotOrigin, PlayerCamera, PIManager.zAdjust);
                    Vector3 restVec = PIManager.RestrictVector(Player, mouseVec, 180);

                    FannelProcess(false, restVec, ShotOrigin, 2);

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
                else
                {
                    Vector3 mouseVec1 = PIManager.MouseVector(ShotOrigin, PlayerCamera, PIManager.zAdjust);
                    Vector3 restVec1 = PIManager.RestrictVector(Player, mouseVec1, 150);

                    FannelProcess(false, restVec1, ShotOrigin2, 2);
                    LineDrawProcess(Fannel, restVec1, wallLayer, 3);

                    PMManager.predictObject.SetActive(false);
                    

                    if (Input.GetMouseButtonUp(0))
                    {
                        //inputgetmouse = false;
                        //if (Input.GetKey(KeyCode.Space)) return;
                        PlayerAnimator.SetBool("Attack", false);
                    }

                    else if (Input.GetMouseButton(0))
                    {
                        //inputgetmouse = true;
                        //if (Input.GetKey(KeyCode.Space)) return;
                        PlayerAnimator.SetBool("Attack", true);
                        Vector3 mouseVec2 = PIManager.MouseVector(ShotOrigin, PlayerCamera, PIManager.zAdjust);
                        Vector3 restVec2 = PIManager.RestrictVector(Player, mouseVec2, 150);

                        FannelProcess(true, restVec2, ShotOrigin2, 4);
                        LineDrawProcess(Fannel, restVec2, wallLayer, 3);

                        if (attackAdmin.fireTimerIsActive)
                        {
                            return;
                        }
                        //発射方向を決める

                        attackAdmin.Attack(restVec2, Fannel);

                        attackAdmin.StartCoroutine("AttackIntervalTimer");
                    }
                }

                break;

            case 2:


                
                if (UIManager.selectedPanelnum > -1 && UIManager.onClick)
                {
                    Time.timeScale = 1;
                    attackAdmin.LVManager.RewardSelectAndlevelUp(UIManager.selectedPanelnum);

                    AgentStopProcess(false);
                    LevelUpUIProcess(false);

                    attackAdmin.AttackIntervalInit();

                    gamePhase = 1;
                    //Debug.Log(LVManager.rewardsLevelsArray[4]);
                }

                break;

            case 3:

                break;
        }
    }

    ////弾が一回で行う処理　壁に衝突するか　敵に衝突するか　+移動
    //void BulletProcess(List<Bullet> ABIList, int number, LayerMask bHitLayer, string tagName, bool isPen)
    //{
    //    Bullet bullet = ABIList[number];
    //    Collider[] cols = CLManager.whatBulletCollide(bullet, bHitLayer);

    //    int beforeLevel = PEXPManager.EXPtoLevel();
    //    //Debug.Log("加算前レベル" + beforeLevel);

    //    //Debug.Log("加算前" + PEXPManager.totalPlayerEXP);

    //    //何かにぶつかっている
    //    if (cols.Length > 0)
    //    {
    //        List<Collider> colOponentList = CLManager.FindWhatYouWant(cols, tagName);

    //        //colsが初っ端から壁だった
    //        if(colOponentList.Count < 1)
    //        {
    //            //弾を破壊
    //            CLManager.BulletRemove(ABIList, number);

    //            //判定を行ったのでColOpListの中身を削除
    //            colOponentList.Clear();

    //            return;
    //        }

    //        //衝突したリストに壁があった
    //        if(colOponentList.Count < cols.Length)
    //        {
    //            //敵にダメージ判定
    //            PEXPManager.totalPlayerEXP += DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet);

    //            //レベルアップするかどうか
    //            LevelUpCheckProcess(beforeLevel);

    //            //弾を破壊
    //            CLManager.BulletRemove(ABIList, number);

    //            //判定を行ったのでColOpListの中身を削除
    //            colOponentList.Clear();
    //            return;
    //        }
    //        //なかった
    //        else
    //        {
    //            //Debug.Log(colOponentList[0].name);

    //            //貫通でない
    //            if (!isPen)
    //            {
    //                PEXPManager.totalPlayerEXP += DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet);
    //                //レベルアップするかどうか
    //                LevelUpCheckProcess(beforeLevel);

    //                //弾を破壊
    //                CLManager.BulletRemove(ABIList, number);

    //                //判定を行ったのでColOpListの中身を削除
    //                colOponentList.Clear();
    //                return;
    //            }
    //            //貫通なら
    //            else
    //            {
    //                PEXPManager.totalPlayerEXP += DMManager.bulletDamegeProcess(colOponentList, AEIList, AEOList, bullet);

    //                //レベルアップするかどうか
    //                LevelUpCheckProcess(beforeLevel);

    //                //判定を行ったのでColOpListの中身を削除
    //                colOponentList.Clear();
    //            }
    //        }
    //    }

    //    ABIList[number].BulletGetMove();
    //    //Debug.Log("うごいた");
    //}

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

            attackAdmin.LVManager.RewardInit();

            AgentStopProcess(true);
            LevelUpUIProcess(true);
            
            UIManager.RewardUISet(attackAdmin.LVManager.infotoPanel, attackAdmin.LVManager.rewardsLevelsArray);
            
            gamePhase = 2;
        }
    }

    void EnemyProcess(List<EnemyS> AEIList, int number, GameObject target)
    {
        AEIList[number].EnemyNavMove(target.transform, AEIList[number].enemyObject.transform.position);

    }

    void MissileProcess(List<EnemyS> MIList, int number)
    {
        bool isFinish = MIList[number].IsMissileFinish();
        if(isFinish)
        {
            Destroy(MIList[number].EnemyGameObject());
            //MIList.RemoveAt(number);
            //MOList.RemoveAt(number);
        }
        else
        {
            MIList[number].MissileMove(MIList[number].enemyObject);
        }
            
        

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
        // if (MIList.Count > 0)
        // {
        //     for (int i = 0; i < MIList.Count; i++)
        //     {
        //         MIList[i].
        //     }
        // }
    }

    void FannelProcess(bool isFannel, Vector3 fannelVec, GameObject originObj, float fannelDist)
    {
        if (isFannel)
        {
            Fannel.SetActive(true);
            Fannel.transform.position = PMManager.BaseObjPos(originObj.transform.position, fannelVec, fannelDist, wallLayer);
        }
        else
        {
            Fannel.transform.position = PMManager.BaseObjPos(originObj.transform.position, fannelVec, fannelDist, wallLayer);
            Fannel.SetActive(false);
        }
    }

    //S_Edit
    IEnumerator AnimDelay(GameObject Player,RaycastHit hitInfo,float delay)
    {
        yield return new WaitForSeconds(delay);
        PMManager.MoveAndRot(Player, hitInfo);
        
    }
    void GetEXP(List<EnemyS> deadEnemiesList)
    {
        if (deadEnemiesList.Count < 1)
        {
            return;
        }

        for (int i = 0; i < deadEnemiesList.Count; i++)
        {
            int exp = (int)deadEnemiesList[i].EnemyEXP();
            PEXPManager.totalPlayerEXP += exp;
        }

        deadEnemiesList.Clear();
    }
    IEnumerator Move(GameObject playerObj, RaycastHit hitInfo, float time)
    {
        PlayerAnimator.SetTrigger("WarpEnd");
        yield return new WaitForSeconds(time);

        PMManager.MoveAndRot(playerObj, hitInfo);


    }
}
