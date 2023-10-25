using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.CanvasScaler;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem instance;

    public event Action OnSelectedUnitChanged;
    public event Action OnSelectedActionChanged;
    public event Action<bool> OnBusyChanged;
    public event Action OnActionStarted;

    [field: SerializeField] 
    public Unit SelectedUnit { get; private set; }
    public BaseAction SelectedAction { get; private set; }

    [SerializeField] 
    private LayerMask unitsLayerMask;

    private bool isBusy;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(SelectedUnit);
    }

    private void Update()
    {
        if (isBusy) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryHandleUnitSelection()) return;

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.instance.GetGridPosition(MouseWorld.GetPosition());
            if (!SelectedAction.IsValidActionGridPosition(mouseGridPosition)) return;

            if (!SelectedUnit.TrySpendActionPointsToTakeAction(SelectedAction)) return;

            SetBusy();
            SelectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke();
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(isBusy);
    }
    
    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == SelectedUnit) return false;

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    public void SetSelectedUnit(Unit unit)
    {
        SelectedUnit = unit;
        SetSelectedAction(unit.MoveAction);
        OnSelectedUnitChanged?.Invoke();
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        SelectedAction = baseAction;
        OnSelectedActionChanged?.Invoke();
    }
}
