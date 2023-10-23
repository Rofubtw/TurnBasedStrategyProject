using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField]
    private Transform actionButtonPrefab;

    [SerializeField]
    private Transform actionButtonContainerTransform;

    private void Start()
    {
        UnitActionSystem.instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_OnSelectedUnitChanged()
    {
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform) 
        {
            Destroy(buttonTransform.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.instance.SelectedUnit;

        foreach (BaseAction baseAction in selectedUnit.BaseActionArray)
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            if(actionButtonTransform.TryGetComponent<ActionButtonUI>(out ActionButtonUI actionButtonUI))
            {
                actionButtonUI.SetBaseAction(baseAction);
            }
        }
    }
}
