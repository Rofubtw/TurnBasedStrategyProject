using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public MoveAction MoveAction { get; private set; }
    public SpinAction SpinAction { get; private set; }
    public BaseAction[] BaseActionArray { get; private set; }

    public GridPosition gridPosition { get; private set; }

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
}
