using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event Action OnStartMoving;
    public event Action OnStopMoving;

    [SerializeField]
    private int maxMoveDistance;

    private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }
    private void Update()
    {        
        ActionMove();
    }
    public void ActionMove()
    {
        if (!isActive) return;

        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float stoppingDistance = 0.1f;
        if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        else
        {
            OnStopMoving?.Invoke();

            ActionComplete();
        }

        float rotationSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        OnStartMoving?.Invoke();

        targetPosition = LevelGrid.instance.GetWorldPosition(gridPosition);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = Unit.GridPosition;
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;

                if (!LevelGrid.instance.IsValidGridPosition(testGridPosition)) continue;

                if (unitGridPosition == testGridPosition) continue; // Sane Grid Position where the unit is already at

                if (LevelGrid.instance.HasAnyGridOnGridPosition(testGridPosition)) continue; // Grid Position already occupied with another Unit

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    

}
