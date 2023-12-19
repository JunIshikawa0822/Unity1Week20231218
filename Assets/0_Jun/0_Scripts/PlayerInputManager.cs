using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    //マウスカーソルを置いた場所のWorld座標を返す（クリックしているか否かはこのメソッドでは扱わない）
    public Vector3 MousePoint(Camera camera, Vector3 mousePos)
    {
        //マウスの現在地をmousePointと呼称
        Vector3 mousePoint = camera.ScreenToWorldPoint(mousePos);
        return mousePoint;
    }

    //マウスで指定したい方向を取得
    public Vector3 MouseVector(GameObject playerObject, Camera camera, Vector3 mousePos2d, float zValue)
    {
        Vector3 playerPos = playerObject.transform.position;
        //Vector3 mousePos = new Vector3(MousePoint(camera, mousePos2d).x, MousePoint(camera, mousePos2d).y, zValue);
        Vector3 mousePos = new Vector3(MousePoint(camera, mousePos2d).x, zValue, MousePoint(camera, mousePos2d).z);

        //プレイヤーからマウスへ向かうベクトルを標準化
        Vector3 mouseVector = (mousePos - playerPos).normalized;

        return mouseVector;
    }
}


