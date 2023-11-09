using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform bulletProjectilePrefab;
    
    [SerializeField]
    private Transform shootPointTransform;

    [SerializeField]
    private Transform rifleTransform;

    [SerializeField]
    private Transform swordTransform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
        }
    }

    private void Start()
    {
        EquipRifle();
    }

    private void SwordAction_OnSwordActionCompleted()
    {
        EquipRifle();
    }

    private void SwordAction_OnSwordActionStarted()
    {
        EquipSword();
        animator.SetTrigger("SwordSlash");
    }

    private void MoveAction_OnStartMoving()
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving()
    {
        animator.SetBool("IsWalking", false);
    }

    private void ShootAction_OnShoot(Unit targetUnit, Unit unit)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform =
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);

        if(bulletProjectileTransform.TryGetComponent<BulletProjectile>(out BulletProjectile bulletProjectile))
        {
            Vector3 targetUnitShootAtPosition = targetUnit.GetWorldPosition();

            targetUnitShootAtPosition.y = shootPointTransform.position.y;

            bulletProjectile.Setup(targetUnitShootAtPosition);
        }
    }

    private void EquipSword()
    {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }

    private void EquipRifle()
    {
        rifleTransform.gameObject.SetActive(true);
        swordTransform.gameObject.SetActive(false);
    }
}
