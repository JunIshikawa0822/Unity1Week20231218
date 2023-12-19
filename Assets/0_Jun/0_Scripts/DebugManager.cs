using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    public GameObject MouseObject;

    public void mousePosDebug(GameObject mouseObject, PlayerInputManager playerInputManager, Camera camera, float zValue)
    {
        //mouseObject.transform.position = new Vector3(
        //    playerInputManager.MousePoint(camera, Input.mousePosition).x,
        //    playerInputManager.MousePoint(camera, Input.mousePosition).y,
        //    zValue);

        mouseObject.transform.position = new Vector3(
           playerInputManager.MousePoint(camera, Input.mousePosition).x,
           zValue,
           playerInputManager.MousePoint(camera, Input.mousePosition).z
           );
    }
}
