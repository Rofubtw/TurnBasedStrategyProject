using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public MoveAction MoveAction { get; private set; }

    public GridPosition gridPosition { get; private set; }

    private void Awake()
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            this.MoveAction = moveAction;
        }
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
