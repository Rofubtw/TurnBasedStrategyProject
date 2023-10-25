using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event Action OnAnyChangeActionPointsChanged;

    public MoveAction MoveAction { get; private set; }
    public SpinAction SpinAction { get; private set; }
    public BaseAction[] BaseActionArray { get; private set; }
    public GridPosition gridPosition { get; private set; }
    public int ActionPoints { get; private set; } = ACTION_POINTS_MAX;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            MoveAction = moveAction;
        }
        if (TryGetComponent<SpinAction>(out SpinAction spinAction))
        {
            SpinAction = spinAction;
        }
        BaseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        
        if(newGridPosition != gridPosition)
        {
            LevelGrid.instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (!CanSpendActionPointsToTakeAction(baseAction)) return false;

        SpendActionPoints(baseAction.GetActionPontsCost());
        return true;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return ActionPoints >= baseAction.GetActionPontsCost();
    }

    private void SpendActionPoints(int amount)
    {
        ActionPoints -= amount;

        OnAnyChangeActionPointsChanged?.Invoke();
    }

    private void TurnSystem_OnTurnChanged()
    {
        ActionPoints = ACTION_POINTS_MAX;
        
        OnAnyChangeActionPointsChanged?.Invoke();
    }
}
