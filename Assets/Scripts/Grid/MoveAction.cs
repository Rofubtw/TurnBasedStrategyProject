using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField]
    private Animator unitAnimator;

    [SerializeField]
    private int maxMoveDistance;

    private Vector3 targetPosition;
    private Unit unit;

    private void Awake()
    {
        if(TryGetComponent<Unit>(out Unit unit))
        {
            this.unit = unit;
        }
        targetPosition = transform.position;
    }
    private void Update()
    {
        GetMoveAction();
    }
    public void GetMoveAction()
    {
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
        {
            Vector3 moveDir = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            unitAnimator.SetBool("IsWalking", true);

            float rotationSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }
    }
    public void Move(GridPosition gridPosition)
    {
        this.targetPosition = LevelGrid.instance.GetWorldPosition(gridPosition);
    }
    
    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();

        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.gridPosition;
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
}
