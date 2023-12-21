using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public void GiveDamage(Bullet bullet,Enemy enemy)
    {
        //弾からダメージ値を取得
        float damage = bullet.BulletDamage();

        //敵にダメージを与える
        enemy.GetDamage(damage);
    }

    //ダメージ処理
    public void bulletDamegeProcess(List<Collider> ColOpList)
    {
        //for (int i = 0; i < ColOpList.Count; i++)
        //{
        Debug.Log("ダメージ");
        //}
    }
}
