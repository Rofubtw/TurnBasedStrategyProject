using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event Action OnAnyChangeActionPointsChanged;
    public static event Action<Unit> OnAnyUnitSpawned;
    public static event Action<Unit> OnAnyUnitDead;

    [field: SerializeField]
    public bool IsEnemy { get; private set; }

    public BaseAction[] BaseActionArray { get; private set; }
    public GridPosition GridPosition { get; private set; }
    public HealthSystem HealthSystem { get; private set; }
    public int ActionPoints { get; private set; } = ACTION_POINTS_MAX;

    private void Awake()
    {
        
        if (TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            HealthSystem = healthSystem;
        }
        BaseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        GridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.AddUnitAtGridPosition(GridPosition, this);

        TurnSystem.instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        HealthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        
        if(newGridPosition != GridPosition)
        {
            GridPosition oldGridPosition = GridPosition;
            GridPosition = newGridPosition;

            LevelGrid.instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in BaseActionArray) 
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
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
        if((IsEnemy && !TurnSystem.instance.IsPlayerTurn) || (!IsEnemy && TurnSystem.instance.IsPlayerTurn))
        {
            ActionPoints = ACTION_POINTS_MAX;

            OnAnyChangeActionPointsChanged?.Invoke();
        }
    }

    public void Damage(int damageAmount)
    {
        HealthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDead()
    {
        LevelGrid.instance.RemoveUnitAtGridPosition(GridPosition, this);
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public float GetHealthNormalized()
    {
        return HealthSystem.GetHealthNormalized();
    }
}
