using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMgr
{
    private static AStarMgr instance; 
    public static AStarMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AStarMgr();
            }
            return instance;
        }
    }
    private int mapW;
    private int mapH;
    public AStarNode[,] nodes;
    private List<AStarNode> openList = new List<AStarNode>();
    private List<AStarNode> closeList = new List<AStarNode>();
    private List<AStarNode> pathNodes = new List<AStarNode>();

    /// <summary>
    /// ��ʼ����ͼ�ڵ�����
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    public void InitMapNodes(int w,int h)
    {
        mapW = w;
        mapH = h;
        nodes = new AStarNode[w, h];
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            { 
                nodes[i, j] = new AStarNode(i, j,Random.Range(0,100) <30 ? E_Node_Type.Stop : E_Node_Type.Walk);
            }
        } 
    }

    public List<AStarNode> FindPath(int start_x ,int start_y,int end_x,int end_y)
    {    
        if (start_x < 0 || start_x > mapW ||
            start_y < 0 || start_y > mapH ||
            end_x < 0 || end_x > mapW ||
            end_y < 0 || end_y > mapH)
        {
            Debug.Log("��ʼ��������ڵ�ͼ��");
            return null;
        }
        AStarNode start = nodes[start_x, start_y];
        AStarNode end = nodes[end_x, end_y];

        if (start.nodeType == E_Node_Type.Stop || end.nodeType == E_Node_Type.Stop)
        {
            Debug.Log("��ʼ����������赲");
            return null;
        }


        //�������
        closeList.Clear();
        openList.Clear();

        //��ʼ����ʼ��
        start.fatherNode = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);

        
        while (true)
        {
            Debug.Log("Ѱ·ing");
            //Ѱ�������Χ�ĵ�
            FindAroundNodes(start.x, start.y, end.x, end.y);

            //��������б�Ϊ�գ�Ҳ�������еĿ��еĵ㶼���ӵ��ر��б�֮������û���ҵ����е�·��������·�ˡ�
            if(openList.Count == 0)
            {
                Debug.Log("�����б�Ϊ�գ��ر�Ѱ·");
                return null;
            }
            //�Կ����б�����Զ�������
            openList.Sort(new ConsumptionCompare());
            //��ʱ�����б���������f��С�ĵ�,��Ϊ��һ�ֵ����
            start = openList[0];
            //�ѵ�ǰ����ӵ��ر��б������Ҫ�ӿ����б���ɾ����
            closeList.Add(start);
            openList.RemoveAt(0);
            //�ҵ���
            if (start == end)
            {
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end); 
                //������������ݣ���·������ѹ��List
                while (end.fatherNode != null)
                {
                    path.Add(end.fatherNode);
                    end = end.fatherNode;
                }
                //��List��ת���õ�����·��
                path.Reverse();
                Debug.Log("��ת·�������   " + path.Count);
                return path;
            }
        } 

    }

    //Ѱ��Ŀ�����Χ��8����
    private void FindAroundNodes(int x,int y,int end_x,int end_y)
    {
        //  |x-1,y-1 | x-1,y |x-1,y+1 |
        //  |x,y-1   |  x,y  |x,y+1   |
        //  |x+1,y-1 |x+1,y  |x+1,y+1 | 

        for (int i = x-1; i <= x+1; i++)
        {
            for (int j = y-1; j <= y+1; j++)
            {
                //������Լ�
                if (i == x && j == y) continue;
                //�߽���
                if ((i < 0 || i >= mapW )||(j < 0 || j >= mapH) ) continue;
                //�ڵ㲻���ϰ������ҿ����б���û��
                AStarNode node = nodes[i, j];
                if(node.nodeType == E_Node_Type.Stop ||closeList.Contains(node)) continue;

                if(openList.Contains(node))
                {
                    //�����б�����������㡣��������������ֱ��������ˣ����������Ǹ�f��С��Ȼ���������father�ڵ���� 
                    //�����µ�g
                    float cur_g = ((Mathf.Abs(i - x) + Mathf.Abs(j - y)) == 1 ? 1.0f : 1.4f) + nodes[x,y].g;
                    //����h
                    float cur_h = Mathf.Abs(i - end_x) + Mathf.Abs(j - end_y); 
                    float cur_f = cur_g + cur_h;
                    if(node.f > cur_f)
                    {
                        node.g = cur_g;
                        node.h = cur_h;
                        node.f = cur_f;
                        node.fatherNode = nodes[x, y];
                    }
                }
                else
                {
                    //�洢���ڵ�
                    node.fatherNode = nodes[x, y];
                    //����g
                    float cur_g = ((Mathf.Abs(i - x) + Mathf.Abs(j - y)) == 1 ? 1.0f : 1.4f);
                    if (node.fatherNode != null)
                    {
                        cur_g += node.fatherNode.g;
                    }
                    node.g = cur_g;
                    //����h
                    node.h = Mathf.Abs(i - end_x) + Mathf.Abs(j - end_y);

                    node.f = node.g + node.h;
                    openList.Add(nodes[i, j]);
                }
                
            }
        }
    }
     
}
