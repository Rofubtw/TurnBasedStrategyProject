using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public GridPosition GridPosition { get; private set; }
    public int GCost { get; private set; }
    public int HCost { get; private set; }
    public int FCost { get; private set; }
    public PathNode CameFromPathNode { get; private set; }
    public bool IsWalkable { get; private set; } = true;

    public PathNode(GridPosition gridPosition)
    {
        this.GridPosition = gridPosition;
    }

    public override string ToString()
    {
        
        return GridPosition.ToString();
    }

    public void SetGCost(int cost)
    {
        this.GCost = cost;
    }

    public void SetHCost(int cost)
    {
        this.HCost = cost;
    }

    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }

    public void SetCameFromPathNode(PathNode pathNode)
    {
        CameFromPathNode = pathNode;
    }

    public void ResetCameFromPathNode()
    {
        CameFromPathNode = null;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        IsWalkable = isWalkable;
    }

}
