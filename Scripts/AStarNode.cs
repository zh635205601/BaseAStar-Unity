using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Node_Type
{
        Walk, //������
        Stop, //�����ߣ��ϰ���߽�
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
    //�˸��ӵ�����x
    public int x;
    //�˸��ӵ�����y
    public int y;
    //Ѱ·����
    public float f;
    //����������
    public float g;
    //�����յ����
    public float h;
    //��������
    public E_Node_Type nodeType;
    //�洢���ڵ�
    public AStarNode fatherNode;

    public AStarNode( int x,int y,E_Node_Type nodeType)
    {
        this.x = x;
        this.y = y;
        this.nodeType = nodeType;
    }

}
