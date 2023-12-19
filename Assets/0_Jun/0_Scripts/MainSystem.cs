using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    [SerializeField]
    PlayerInputManager playerInputManager;

    [SerializeField]
    PlayerMoveManager playerMoveManager;

    [SerializeField]
    DebugManager debugManager;

    [SerializeField]
    ShotInfoManager shotInfoManager;

    [SerializeField]
    GameObject Player;

    Camera PlayerCamera;

    LayerMask wallLayerMask = 1 << 6;

    List<Bullet> ABList;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.main;
        ABList = shotInfoManager.AllBulletInfoList;
    }

    // Update is called once per frame
    void Update()
    {
        //マウスの位置デバッグ
        //debugManager.mousePosDebug(debugManager.MouseObject, playerInputManager, PlayerCamera, 10);
        if(ABList.Count > 0)
        {
            shotInfoManager.BulletRemove(ABList, wallLayerMask);
            shotInfoManager.AllBulletMove(ABList);
        }

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            //移動
            if (Input.GetMouseButtonDown(0))
            {
                playerMoveManager.NormalMove(
                Player,
                Player.transform.position,
                playerInputManager.MouseVector(Player, PlayerCamera, 10),
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
                Vector3 mouseVec = playerInputManager.MouseVector(Player, PlayerCamera, 10);

                shotInfoManager.BulletInfoInstantiate(shotInfoManager.bullet1, Player.transform.position, mouseVec);
            }
        }
    }
}
