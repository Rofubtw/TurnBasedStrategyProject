using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{

    public static event Action OnAnySwordHit;

    public event Action OnSwordActionStarted;
    public event Action OnSwordActionCompleted;

    public int MaxSwordDistance { get; private set; } = 1;

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingSwordAfterHit,
    }

    private State state;
    private float stateTimer;
    private Unit targetUnit;

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                RotateTowardsEnemy();
                break;
            case State.SwingSwordAfterHit:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingSwordAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                targetUnit.Damage(100);
                OnAnySwordHit?.Invoke();
                break;
            case State.SwingSwordAfterHit:
                OnSwordActionCompleted?.Invoke();
                ActionComplete(); 
                break;
        }
    }

    private void RotateTowardsEnemy()
    {
        Vector3 aimDir = (targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;

        float rotationSpeed = 10f;
        Unit.transform.forward = Vector3.Lerp(Unit.transform.forward, aimDir, rotationSpeed * Time.deltaTime);
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = Unit.GridPosition;

        for (int x = -MaxSwordDistance; x <= MaxSwordDistance; x++)
        {
            for (int z = -MaxSwordDistance; z <= MaxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.instance.IsValidGridPosition(testGridPosition)) continue;

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
        targetUnit = LevelGrid.instance.GetUnitAtGridPosition(gridPosition);
        
        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke();

        ActionStart(onActionComplete);
    }
}
