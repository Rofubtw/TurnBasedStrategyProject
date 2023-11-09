using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeAction : BaseAction
{

    [SerializeField]
    private Transform grenadeProjectilePrefab;

    public int MaxThrowDistance { get; private set; } = 7;

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = Unit.GridPosition;

        for (int x = -MaxThrowDistance; x <= MaxThrowDistance; x++)
        {
            for (int z = -MaxThrowDistance; z <= MaxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > MaxThrowDistance) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, Unit.GetWorldPosition(), Quaternion.identity);

        if(grenadeProjectileTransform.TryGetComponent<GrenadeProjectile>(out GrenadeProjectile grenadeProjectile))
        {
            grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);
        }

        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
}
