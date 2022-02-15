using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    public int beginX, beginY;//第一个立方体的位置
    public int offSetX=2, offsetY=-2;//偏移位置
    public int mapW=10, mapH=10;


    private Vector2 beginPos = Vector2.right * -1;//开始点
    public Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();
    public Material red;
    public Material yellow;
    public Material green;
    public Material white;
    private int layerMask = 1 << 10;//除去墙体层级
    public bool colorFLag = true;

    private List<AstarNode> list=new List<AstarNode>();
    public List<GameObject> list2=new List<GameObject>();//用于清除颜色
    // Start is called before the first frame update
    void Start()
    {
        //mapH = GameObject.Find("DontDestory").GetComponent<ChangeScenes>().width;
        //mapW = GameObject.Find("DontDestory").GetComponent<ChangeScenes>().length;
        layerMask = ~layerMask;
        AStarManager.Instance.InitMapInfo(mapW,mapH);
        for(int i=0;i<mapW;i++)//创建立方体
        {
            for(int j=0;j<mapH;j++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);//创建立方体
                obj.transform.position = new Vector3(beginX + i * offSetX, 0,beginY + j * offsetY);
                obj.name = i + "_" + j;
                obj.AddComponent<ShootRay>();
                list2.Add(obj);
                cubes.Add(obj.name, obj);//存储立方体到字典
                AstarNode node = AStarManager.Instance.nodes[i,j];//得到格子类型
                if(node.type==E_Node_Type.Stop)//判断阻挡
                {
                    obj.GetComponent<MeshRenderer>().material = red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type == "Wall_Btn")//在墙体模式下，显示鼠标落点
        {
            Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);//销毁dot
            colorFLag = true;
            RaycastHit info0;
            Ray ray0 = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray0, out info0, 1000, layerMask);
            info0.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
            for (int i = 0; i < list2.Count; ++i)
            {
                if(list2[i]!= info0.collider.gameObject)
                list2[i].transform.GetComponent<MeshRenderer>().material = white;//清除颜色
            }
        }
        

        if (GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type == "Way_Btn")
        {
            if(colorFLag==true)//清除墙体模式下的落点
            {
                for (int i = 0; i < list2.Count; ++i)
                {
                    list2[i].transform.GetComponent<MeshRenderer>().material = white;//清除颜色
                }
                colorFLag = false;
            }
            if (Input.GetMouseButtonDown(0))//鼠标左键按下
            {
                //射线检测
                RaycastHit info;
                //得到屏幕鼠标位置发出去的射线
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out info, 1000))
                {
                    //得到点击的立方体
                    if (beginPos == Vector2.right * -1)
                    {
                        //清理颜色
                        if (list != null)
                        {
                            for (int i = 0; i < list.Count; ++i)
                            {
                                cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = white;
                            }
                        }

                        string[] strs = info.collider.gameObject.name.Split('_');
                        beginPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));//得到行列位置,起点
                        info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;//把点击对象改为黄色
                    }
                    else
                    {
                        string[] strs = info.collider.gameObject.name.Split('_');//终点
                        Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                        list = AStarManager.Instance.FindPath(beginPos, endPos);//寻路
                        if (list != null)
                        {
                            for (int i = 0; i < list.Count; ++i)
                            {
                                cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;
                            }
                        }
                        //清除开始点
                        cubes[(int)beginPos.x + "_" + (int)beginPos.y].GetComponent<MeshRenderer>().material = green;
                        beginPos = Vector2.right * -1;
                    }

                }

            }
        }
        
    }
}
