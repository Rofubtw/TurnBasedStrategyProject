using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event Action<Unit,Unit> OnShoot;

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
    
    public Unit TargetUnit { get; private set; }
    public int MaxShootDistance { get; private set; } = 7;

    private State state;
    private float stateTimer;
    private bool canShootBullet;

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                RotateTowardsEnemy();
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer<= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float cooloffStateTime = 0.5f;
                stateTimer = cooloffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(TargetUnit, Unit);


        TargetUnit.Damage(40);
    }

    private void RotateTowardsEnemy()
    {
        Vector3 aimDir = (TargetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;

        float rotationSpeed = 10f;
        Unit.transform.forward = Vector3.Lerp(Unit.transform.forward, aimDir, rotationSpeed * Time.deltaTime);
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = Unit.GridPosition;
        for (int x = -MaxShootDistance; x <= MaxShootDistance; x++)
        {
            for (int z = -MaxShootDistance; z <= MaxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;

                if (!LevelGrid.instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > MaxShootDistance) continue;

                if (!LevelGrid.instance.HasAnyGridOnGridPosition(testGridPosition)) continue; // Grid Position is empty no unit

                Unit targetUnit = LevelGrid.instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy == Unit.IsEnemy) continue; // both units in same team


                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        TargetUnit = LevelGrid.instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }
}
