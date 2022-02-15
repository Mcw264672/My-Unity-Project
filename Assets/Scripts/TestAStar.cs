using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    public int beginX, beginY;//��һ���������λ��
    public int offSetX=2, offsetY=-2;//ƫ��λ��
    public int mapW=10, mapH=10;


    private Vector2 beginPos = Vector2.right * -1;//��ʼ��
    public Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();
    public Material red;
    public Material yellow;
    public Material green;
    public Material white;
    private int layerMask = 1 << 10;//��ȥǽ��㼶
    public bool colorFLag = true;

    private List<AstarNode> list=new List<AstarNode>();
    public List<GameObject> list2=new List<GameObject>();//���������ɫ
    // Start is called before the first frame update
    void Start()
    {
        //mapH = GameObject.Find("DontDestory").GetComponent<ChangeScenes>().width;
        //mapW = GameObject.Find("DontDestory").GetComponent<ChangeScenes>().length;
        layerMask = ~layerMask;
        AStarManager.Instance.InitMapInfo(mapW,mapH);
        for(int i=0;i<mapW;i++)//����������
        {
            for(int j=0;j<mapH;j++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);//����������
                obj.transform.position = new Vector3(beginX + i * offSetX, 0,beginY + j * offsetY);
                obj.name = i + "_" + j;
                obj.AddComponent<ShootRay>();
                list2.Add(obj);
                cubes.Add(obj.name, obj);//�洢�����嵽�ֵ�
                AstarNode node = AStarManager.Instance.nodes[i,j];//�õ���������
                if(node.type==E_Node_Type.Stop)//�ж��赲
                {
                    obj.GetComponent<MeshRenderer>().material = red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type == "Wall_Btn")//��ǽ��ģʽ�£���ʾ������
        {
            Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);//����dot
            colorFLag = true;
            RaycastHit info0;
            Ray ray0 = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray0, out info0, 1000, layerMask);
            info0.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
            for (int i = 0; i < list2.Count; ++i)
            {
                if(list2[i]!= info0.collider.gameObject)
                list2[i].transform.GetComponent<MeshRenderer>().material = white;//�����ɫ
            }
        }
        

        if (GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type == "Way_Btn")
        {
            if(colorFLag==true)//���ǽ��ģʽ�µ����
            {
                for (int i = 0; i < list2.Count; ++i)
                {
                    list2[i].transform.GetComponent<MeshRenderer>().material = white;//�����ɫ
                }
                colorFLag = false;
            }
            if (Input.GetMouseButtonDown(0))//����������
            {
                //���߼��
                RaycastHit info;
                //�õ���Ļ���λ�÷���ȥ������
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out info, 1000))
                {
                    //�õ������������
                    if (beginPos == Vector2.right * -1)
                    {
                        //������ɫ
                        if (list != null)
                        {
                            for (int i = 0; i < list.Count; ++i)
                            {
                                cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = white;
                            }
                        }

                        string[] strs = info.collider.gameObject.name.Split('_');
                        beginPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));//�õ�����λ��,���
                        info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;//�ѵ�������Ϊ��ɫ
                    }
                    else
                    {
                        string[] strs = info.collider.gameObject.name.Split('_');//�յ�
                        Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                        list = AStarManager.Instance.FindPath(beginPos, endPos);//Ѱ·
                        if (list != null)
                        {
                            for (int i = 0; i < list.Count; ++i)
                            {
                                cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;
                            }
                        }
                        //�����ʼ��
                        cubes[(int)beginPos.x + "_" + (int)beginPos.y].GetComponent<MeshRenderer>().material = green;
                        beginPos = Vector2.right * -1;
                    }

                }

            }
        }
        
    }
}
