using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAdmin : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GunShotManager GSManager;

    [SerializeField]
    public LevelManager LVManager;

    [SerializeField]
    public DamageManager DMManager;

    //[SerializeField]
    //public SoundManager SManager;

    [System.NonSerialized]
    public bool fireTimerIsActive = false;
    //float fireInterval;
    WaitForSeconds fireIntervalWait;

    LayerMask bulletHitLayer = 1 << 6 | 1 << 7;

    //intervalWaitの更新　レベルアップの時に再度更新する

    public void AttackAdminInit()
    {
        GSManager.GunShotManagerInit(this);
        DMManager.DamageManagerInit(this);
        //SManager.SoundManagerInit(this);
    }

    public void AttackIntervalInit()
    {
        fireIntervalWait = new WaitForSeconds(LVManager.fireIntervalLevelArray[LVManager.LevelofIndex(2)]);
    }

    public void Attack(Vector3 attackVec, GameObject attackInstaPos)
    {
        GSManager.GunShot(
            attackVec,
            LVManager.simulNumLevelArray[LVManager.LevelofIndex(0)],
            GSManager.bulletTypeObjArray,
            LVManager.bulletDamageLevelArray[LVManager.LevelofIndex(3)],
            attackInstaPos.transform.position,
            LVManager.destroyDistLevelArray[LVManager.LevelofIndex(1)],
            LVManager.bulletAngleLevelArray[LVManager.LevelofIndex(5)],
            LVManager.penetrateLevelArray[LVManager.LevelofIndex(4)],
            bulletHitLayer
            );
    }

    public IEnumerator AttackIntervalTimer()
    {
        fireTimerIsActive = true;

        yield return fireIntervalWait;

        fireTimerIsActive = false;
    }

}
