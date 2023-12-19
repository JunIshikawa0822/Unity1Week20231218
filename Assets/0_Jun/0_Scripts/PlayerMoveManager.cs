using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    //移動できるかどうか
    public void NormalMove(GameObject playerObject, Vector3 originPos, Vector3 directionVec, float maxDistance, int layerMask, QueryTriggerInteraction triggerDetectMode)
    {
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

    void MoveAndRot(GameObject playerObject, RaycastHit hitInfo)
    {
        playerObject.transform.position = hitInfo.point;
        playerObject.transform.rotation = Quaternion.FromToRotation(transform.up, hitInfo.normal);
    }
}
