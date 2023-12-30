using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManagerS : MonoBehaviour
{
    [SerializeField]
    public GameObject shotOriginObject;

    [SerializeField]
    public GameObject Player;

    [SerializeField]
    public GameObject Fannel;

    [SerializeField]
    public GameObject[] baseBlocksArray = new GameObject[3];

    [SerializeField]
    public GameObject predictObject;

    //移動できるかどうか
    public void NormalMove(GameObject playerObject, Vector3 originPos, Vector3 directionVec, float maxDistance, int layerMask, QueryTriggerInteraction triggerDetectMode)
    {
        //Debug.DrawRay(originPos, directionVec, Color.red, 3);
        if (!Physics.Raycast(originPos, directionVec, out RaycastHit hitInfo, maxDistance, layerMask, triggerDetectMode))
        {
            //できない
            return;   
        }
        else
        {
            //できる
            MoveAndRot(playerObject, hitInfo);
        }    
    }

    public void MoveAndRot(GameObject playerObject, RaycastHit hitInfo)
    {
        playerObject.transform.position = hitInfo.point + hitInfo.normal;
        playerObject.transform.rotation = Quaternion.LookRotation(-Vector3.up, hitInfo.normal);
    }

    public Vector3 BaseObjPos(Vector3 originPos, Vector3 directionVec, float maxDistance, int layerMask)
    {
        //当たらない
        if (!Physics.Raycast(originPos, directionVec, out RaycastHit hitInfo, maxDistance, layerMask))
        {
            //マウスの先
            //Debug.Log(mouseVec);
            return originPos + new Vector3(directionVec.x * maxDistance, directionVec.y, directionVec.z * maxDistance);
        }
        else
        {
            //できる
            return hitInfo.point;
        }
    }

    public void SetObjPos(Vector3 setPos, GameObject obj)
    {
        obj.transform.position = setPos;
    }


    //特定のオブジェクトと接しているか
    public bool isPlayerStandOnBaseObj(GameObject box, LayerMask layerMask)
    {
        Collider[] cols = Physics.OverlapBox(
            box.transform.position,
            box.transform.localScale/2,
            Quaternion.identity,
            layerMask
            );

        bool isStand = false;

        if (cols.Length > 0)
        {
            isStand = true;
        }
        else
        {
            isStand = false;
        }

        return isStand;
    }
}
