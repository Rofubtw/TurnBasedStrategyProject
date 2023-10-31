using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem instance;

    public event Action OnTurnChanged;

    public int TurnNumber { get; private set; } = 1;
    public bool IsPlayerTurn { get; private set; } = true;

    private void Awake()
    {
        instance = this;
    }
    
    public void NextTurn()
    {
        TurnNumber++;
        IsPlayerTurn = !IsPlayerTurn;

        OnTurnChanged?.Invoke();
    }
}
