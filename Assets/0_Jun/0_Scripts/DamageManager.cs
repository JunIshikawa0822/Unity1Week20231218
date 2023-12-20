using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    //敵のリストをチェック
    void CheckEnemy(List<Collider> ColOponentList, bool isPen)
    {
        if (isPen)
        {
            //ColOponentListの全員にダメージを与える
        }
        else
        {
            //ColOponentListの先頭にダメージを与える
        }
    }
}
