using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    private void DestructibleCrate_OnAnyDestroyed(DestructibleCrate destructibleCrate)
    {
        Pathfinding.instance.SetIsWalkableGridPosition(destructibleCrate.GridPosition, true);
    }
}
