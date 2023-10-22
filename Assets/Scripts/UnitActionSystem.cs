using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem instance;

    public event Action OnSelectedUnitChanged;

    [field: SerializeField] 
    public Unit SelectedUnit { get; private set; }

    [SerializeField] 
    private LayerMask unitsLayerMask;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            if(SelectedUnit != null)
            {
                GridPosition mouseGridPosition = LevelGrid.instance.GetGridPosition(MouseWorld.GetPosition());

                if (SelectedUnit.MoveAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    SelectedUnit.MoveAction.Move(mouseGridPosition);
                }
            }
        }
    }
    
    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask)) 
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }
    private void SetSelectedUnit(Unit unit)
    {
        SelectedUnit = unit;
        OnSelectedUnitChanged?.Invoke();
    }
}
