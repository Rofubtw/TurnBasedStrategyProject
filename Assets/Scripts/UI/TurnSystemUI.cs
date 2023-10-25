using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    public static TurnSystemUI instance;

    [SerializeField]
    private TextMeshProUGUI turnNumberText;

    [SerializeField]
    private Button endTurnButton;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() => { 
            TurnSystem.instance.NextTurn();
        });

        TurnSystem.instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
    }

    private void TurnSystem_OnTurnChanged()
    {
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = "TURN " + TurnSystem.instance.TurnNumber;
    }
}
