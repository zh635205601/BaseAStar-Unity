using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForAStar : MonoBehaviour
{ 
    public int beginX = -4;
    public int beginY = -4;

    public int mapW = 5;
    public int mapH = 5;

    public int offsetX = 2;
    public int offsetY = 2;

    public Material red;
    public Material green;
    public Material blue;
    public Material yellow;
    public Material normal;

    private Dictionary<string, GameObject> cubesObj = new Dictionary<string, GameObject>();

    private int start_x = -1;
    private int start_y = -1;
    private int end_x;
    private int end_y;
    // Start is called before the first frame update
    void Start()
    {
        AStarMgr.Instance.InitMapNodes(5, 5);
        for (int i = 0; i < mapW; i++)
        {
            for (int j = 0; j < mapH; j++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i * offsetX, beginY + j * offsetY, 7);
                obj.name = i.ToString() + "_" + j.ToString();
                cubesObj.Add(obj.name, obj);
                AStarNode node = AStarMgr.Instance.nodes[i, j];
                if (node.nodeType == E_Node_Type.Stop)
                {
                    obj.GetComponent<MeshRenderer>().material = red;
                }
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out info,1000))
            {
                if (info.collider.gameObject.name == "ReStart")
                {
                    start_x = -1;
                    start_y = -1;
                    foreach (var item in cubesObj)
                    {
                        string[] pos = item.Value.name.Split("_");
                        int index_x = int.Parse(pos[0]);
                        int index_y = int.Parse(pos[1]);
                        if (AStarMgr.Instance.nodes[index_x,index_y].nodeType == E_Node_Type.Walk)
                        {
                            item.Value.GetComponent<MeshRenderer>().material = normal;
                        }
                    }
                }
                else
                {
                    //第一次点击
                    if (start_x == -1 && start_y == -1)
                    {
                        string[] pos = info.collider.gameObject.name.Split("_");
                        start_x = int.Parse(pos[0]);
                        start_y = int.Parse(pos[1]);
                        info.collider.gameObject.GetComponent<MeshRenderer>().material = green;
                    }
                    else
                    {
                        string[] pos = info.collider.gameObject.name.Split("_");
                        end_x = int.Parse(pos[0]);
                        end_y = int.Parse(pos[1]);
                        info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                        Debug.Log("开始寻路");
                        List<AStarNode> path = AStarMgr.Instance.FindPath(start_x, start_y, end_x, end_y);
                        if (path != null)
                        {
                            Debug.Log("寻路完成" + path.Count);
                            for (int i = 0; i < path.Count; i++)
                            {
                                cubesObj[path[i].x + "_" + path[i].y].GetComponent<MeshRenderer>().material = blue;
                            }
                        }
                    }
                }
                
            }

        }
    }
}
