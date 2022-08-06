using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Node_Type
{
        Walk, //可以走
        Stop, //不能走，障碍或边界
}

public class ConsumptionCompare:IComparer<AStarNode>
{
    public int Compare(AStarNode x, AStarNode y)
    {
        if (x.f > y.f)
            return 1;
        if (x.f == y.f)
            return 1;
        else
            return -1;
    }
}

public class AStarNode
{
    //此格子的坐标x
    public int x;
    //此格子的坐标y
    public int y;
    //寻路消耗
    public float f;
    //距离起点距离
    public float g;
    //距离终点距离
    public float h;
    //格子类型
    public E_Node_Type nodeType;
    //存储父节点
    public AStarNode fatherNode;

    public AStarNode( int x,int y,E_Node_Type nodeType)
    {
        this.x = x;
        this.y = y;
        this.nodeType = nodeType;
    }

}
