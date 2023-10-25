using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField]
    private Transform actionButtonPrefab;

    [SerializeField]
    private Transform actionButtonContainerTransform;

    [SerializeField]
    private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyChangeActionPointsChanged += Unit_OnAnyChangeActionPointsChanged;

        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

   

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform) 
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.instance.SelectedUnit;

        foreach (BaseAction baseAction in selectedUnit.BaseActionArray)
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            if(actionButtonTransform.TryGetComponent<ActionButtonUI>(out ActionButtonUI actionButtonUI))
            {
                actionButtonUI.SetBaseAction(baseAction);

                actionButtonUIList.Add(actionButtonUI);
            }
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged()
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged()
    {
        UpdateSelectedVisual();

    }

    private void UnitActionSystem_OnActionStarted()
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChanged()
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyChangeActionPointsChanged()
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.instance.SelectedUnit;
        int actionPoints = selectedUnit.ActionPoints;
        actionPointsText.text = "Action Points : " + actionPoints;
    }
}
