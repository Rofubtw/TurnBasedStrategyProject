using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
    }

    private void SwordAction_OnAnySwordHit()
    {
        ScreenShake.instance.Shake(2f);
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded()
    {
        ScreenShake.instance.Shake(5f);
    }

    private void ShootAction_OnAnyShoot(Unit arg1, Unit arg2)
    {
        ScreenShake.instance.Shake();
    }
}
