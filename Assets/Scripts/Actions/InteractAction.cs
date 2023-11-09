using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    public int MaxInteractDistance { get; private set; } = 1;

    public override string GetActionName()
    {
        return "Interact";
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

        for (int x = -MaxInteractDistance; x <= MaxInteractDistance; x++)
        {
            for (int z = -MaxInteractDistance; z <= MaxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.instance.IsValidGridPosition(testGridPosition)) continue;

                IInteractable interactable = LevelGrid.instance.GetInteractableAtGridPosition(testGridPosition);
                if (interactable == null) continue; // No interactable on this GridPosition

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractable interactable = LevelGrid.instance.GetInteractableAtGridPosition(gridPosition);

        interactable.Interact(OnInteractComplete);

        ActionStart(onActionComplete);
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }
}
