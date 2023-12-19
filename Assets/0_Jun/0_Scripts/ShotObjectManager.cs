using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShotObjectManager : MonoBehaviour
{
    // Start is called before the first frame update

    //複数種類の弾を保持する配列
    [SerializeField]
    GameObject[] BulletTypeArray = new GameObject[2];

    [System.NonSerialized]
    List<GameObject> AllBulletList = new List<GameObject>();


}
