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
    GameObject Player;

    Camera PlayerCamera;

    LayerMask wallLayerMask = 1 << 6;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //マウスの位置デバッグ
        //debugManager.mousePosDebug(debugManager.MouseObject, playerInputManager, PlayerCamera, 10);

        //左クリックを押したら移動
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            
            playerMoveManager.NormalMove(
                Player,
                Player.transform.position,
                playerInputManager.MouseVector(Player, PlayerCamera, Input.mousePosition, 10),
                100,
                wallLayerMask,
                QueryTriggerInteraction.Collide
                );
        }
    }
}
