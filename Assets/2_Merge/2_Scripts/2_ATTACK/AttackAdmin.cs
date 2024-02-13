using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAdmin : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public ShotInfoManage SIManager;

    [SerializeField]
    public LevelManager LVManager;

    [System.NonSerialized]
    public bool fireTimerIsActive = false;
    //float fireInterval;
    WaitForSeconds fireIntervalWait;

    //intervalWaitの更新　レベルアップの時に再度更新する
    public void AttackIntervalInit()
    {
        fireIntervalWait = new WaitForSeconds(LVManager.fireIntervalLevelArray[LVManager.LevelofIndex(2)]);
    }

    public void Attack(Vector3 attackVec, GameObject attackInstaPos)
    {
        SIManager.BulletShotSimultaniously(
            attackVec,
            LVManager.simulNumLevelArray[LVManager.LevelofIndex(0)],
            SIManager.bulletTypeObjArray,
            LVManager.bulletDamageLevelArray[LVManager.LevelofIndex(3)],
            attackInstaPos.transform.position,
            LVManager.destroyDistLevelArray[LVManager.LevelofIndex(1)],
            LVManager.bulletAngleLevelArray[LVManager.LevelofIndex(5)],
            LVManager.penetrateLevelArray[LVManager.LevelofIndex(4)]
            );
    }

    public IEnumerator AttackIntervalTimer()
    {
        fireTimerIsActive = true;

        yield return fireIntervalWait;

        fireTimerIsActive = false;
    }

}
