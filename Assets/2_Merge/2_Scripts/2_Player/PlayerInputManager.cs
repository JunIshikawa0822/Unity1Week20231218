using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField]
    public float zAdjust = 10;

    [System.NonSerialized]
    public bool fireTimerIsActive = false;
    //float fireInterval;
    WaitForSeconds fireIntervalWait;

    //intervalWaitの更新　レベルアップの時に再度更新する
    public void InputInterval(float interval)
    {
        fireIntervalWait = new WaitForSeconds(interval);
    }

    //マウスカーソルを置いた場所のWorld座標を返す（クリックしているか否かはこのメソッドでは扱わない）
    public Vector3 MousePoint(Camera camera)
    {
        //マウスの現在地をmousePointと呼称
        Vector3 mousePoint = camera.ScreenToWorldPoint(Input.mousePosition);
        return mousePoint;
    }

    //マウスで指定したい方向を取得
    public Vector3 MouseVector(GameObject playerObject, Camera camera, float zValue)
    {
        Vector3 playerPos = playerObject.transform.position;
        //Vector3 mousePos = new Vector3(MousePoint(camera, mousePos2d).x, MousePoint(camera, mousePos2d).y, zValue);
        Vector3 mousePos = new Vector3(MousePoint(camera).x, zValue, MousePoint(camera).z);

        //プレイヤーからマウスへ向かうベクトルを標準化
        Vector3 mouseVector = (mousePos - playerPos).normalized;

        return mouseVector;
    }

    public IEnumerator FireTimer()
    {
        fireTimerIsActive = true;

        yield return fireIntervalWait;

        fireTimerIsActive = false;
    }
}


