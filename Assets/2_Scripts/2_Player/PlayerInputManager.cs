using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField]
    public float zAdjust = 10;

    LineRenderer lineRenderer;

    

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

    public Vector3 RestrictVector(GameObject playerObject, Vector3 baseVec, float restrictAngle)
    {
        Vector3 restVec;
        Vector3 fromVec = Vector3.ProjectOnPlane(playerObject.transform.up, -playerObject.transform.forward);
        Vector3 toVec = Vector3.ProjectOnPlane(baseVec, -playerObject.transform.forward);

        float Angle = Vector3.SignedAngle(fromVec, toVec, -playerObject.transform.forward);
        //Debug.Log(Angle);

        if(Mathf.Abs(Angle) > restrictAngle)
        {
            //Debug.Log(Mathf.Sign(Angle));
            restVec = Quaternion.Euler(0, Mathf.Sign(Angle) * restrictAngle, 0) * playerObject.transform.up;
        }
        else
        {
            restVec = baseVec;
        }

        return restVec.normalized;
    }

    public void LineRendererInit()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void LineDraw(GameObject originObj, Vector3 endPos)
    {
        Vector3[] positions = new Vector3[] {originObj.transform.position, endPos};

        lineRenderer.SetPositions(positions);
    }
}


