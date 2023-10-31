using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    public List<Unit> UnitList { get; private set; }
    public List<Unit> FriendlyUnitList { get; private set; }
    public List<Unit> EnemyUnitList { get; private set; }

    private void Awake()
    {
        instance = this;

        UnitList = new List<Unit>();
        FriendlyUnitList = new List<Unit>();
        EnemyUnitList = new List<Unit>();
    }
    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(Unit senderUnit)
    {
        UnitList.Add(senderUnit);

        (senderUnit.IsEnemy ? EnemyUnitList : FriendlyUnitList).Add(senderUnit);
    }

    private void Unit_OnAnyUnitDead(Unit senderUnit)
    {
        UnitList.Remove(senderUnit);

        (senderUnit.IsEnemy ? EnemyUnitList : FriendlyUnitList).Remove(senderUnit);
    }
}
