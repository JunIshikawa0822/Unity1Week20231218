using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField]
    DamageManager DMManager;

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
                    BulletProcess(ABIList, i, bulletHitLayer, "Wall", SIManager.isPenetrate);
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
            }
        }
    }

    //弾が一回で行う処理　壁に衝突するか　敵に衝突するか　＋移動
    void BulletProcess(List<Bullet> ABIList, int number, LayerMask bHitLayer, string tagName, bool isPen)
    {
        Collider[] cols = CLManager.whatBulletCollide(ABIList[number], bHitLayer);

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
                DMManager.bulletDamegeProcess(colOponentList);

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
                    DMManager.bulletDamegeProcess(colOponentList);

                    //弾を破壊
                    CLManager.BulletRemove(ABIList, number);
                    return;
                }
                //貫通なら
                else
                {
                    DMManager.bulletDamegeProcess(colOponentList);
                }
                   
                //判定を行ったのでColOpListの中身を削除
                colOponentList.Clear();
            }
        }
        
        ABIList[number].BulletGetMove();
    }
}
