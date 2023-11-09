using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour , IInteractable
{
    [SerializeField]
    private Material greenMaterial;

    [SerializeField]
    private Material redMaterial;
    
    [SerializeField]
    private MeshRenderer meshRenderer;

    private bool isGreen;
    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private float timer;
    private bool isActive;

    private void Start()
    {
        gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.SetInteractableAtGridPosition(gridPosition, this);

        SetColorRed();
    }

    private void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    private void SetColorGreen()
    {
        isGreen = false;
        meshRenderer.material = greenMaterial;
    }
    
    private void SetColorRed()
    {
        isGreen = true;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = .5f;

        if (isGreen)
        {
            SetColorGreen();
        }
        else
        {
            SetColorRed();
        }
    }
}
