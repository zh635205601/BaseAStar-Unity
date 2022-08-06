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
    /// 初始化地图节点数组
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
            Debug.Log("开始或结束点在地图外");
            return null;
        }
        AStarNode start = nodes[start_x, start_y];
        AStarNode end = nodes[end_x, end_y];

        if (start.nodeType == E_Node_Type.Stop || end.nodeType == E_Node_Type.Stop)
        {
            Debug.Log("开始或结束点是阻挡");
            return null;
        }


        //清空容器
        closeList.Clear();
        openList.Clear();

        //初始化起始点
        start.fatherNode = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);

        
        while (true)
        {
            Debug.Log("寻路ing");
            //寻找起点周围的点
            FindAroundNodes(start.x, start.y, end.x, end.y);

            //如果开启列表为空，也就是所有的可行的点都被加到关闭列表之后，依旧没有找到可行的路径，则死路了。
            if(openList.Count == 0)
            {
                Debug.Log("开启列表为空，关闭寻路");
                return null;
            }
            //对开启列表进行自定义排序，
            openList.Sort(new ConsumptionCompare());
            //此时开启列表的首项，就是f最小的点,定为下一轮的起点
            start = openList[0];
            //把当前点添加到关闭列表里，并且要从开启列表里删除它
            closeList.Add(start);
            openList.RemoveAt(0);
            //找到了
            if (start == end)
            {
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end); 
                //类似于链表回溯，把路径倒叙压入List
                while (end.fatherNode != null)
                {
                    path.Add(end.fatherNode);
                    end = end.fatherNode;
                }
                //把List反转，得到最终路径
                path.Reverse();
                Debug.Log("反转路径已完成   " + path.Count);
                return path;
            }
        } 

    }

    //寻找目标点周围的8个点
    private void FindAroundNodes(int x,int y,int end_x,int end_y)
    {
        //  |x-1,y-1 | x-1,y |x-1,y+1 |
        //  |x,y-1   |  x,y  |x,y+1   |
        //  |x+1,y-1 |x+1,y  |x+1,y+1 | 

        for (int i = x-1; i <= x+1; i++)
        {
            for (int j = y-1; j <= y+1; j++)
            {
                //不检测自己
                if (i == x && j == y) continue;
                //边界检测
                if ((i < 0 || i >= mapW )||(j < 0 || j >= mapH) ) continue;
                //节点不是障碍，并且开放列表里没有
                AStarNode node = nodes[i, j];
                if(node.nodeType == E_Node_Type.Stop ||closeList.Contains(node)) continue;

                if(openList.Contains(node))
                {
                    //开启列表里有了这个点。但是现在这个点又被遍历到了，计算两次那个f最小，然后把这个点的father节点更新 
                    //计算新的g
                    float cur_g = ((Mathf.Abs(i - x) + Mathf.Abs(j - y)) == 1 ? 1.0f : 1.4f) + nodes[x,y].g;
                    //计算h
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
                    //存储父节点
                    node.fatherNode = nodes[x, y];
                    //计算g
                    float cur_g = ((Mathf.Abs(i - x) + Mathf.Abs(j - y)) == 1 ? 1.0f : 1.4f);
                    if (node.fatherNode != null)
                    {
                        cur_g += node.fatherNode.g;
                    }
                    node.g = cur_g;
                    //计算h
                    node.h = Mathf.Abs(i - end_x) + Mathf.Abs(j - end_y);

                    node.f = node.g + node.h;
                    openList.Add(nodes[i, j]);
                }
                
            }
        }
    }
     
}
