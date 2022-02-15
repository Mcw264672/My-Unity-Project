using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetDirection : MonoBehaviour
{
    public Vector3 lastPos;//起点
    public Vector3 curPos;//终点
    public Vector3 offSet;//偏移量
    public RaycastHit hit;//射线
    private LineRenderer line;//线条
    private Vector3 createPos;//创建位置
    public Material color;//颜色
    private int layerMask = 1 << 10;
    private float length = .0f;//长度
    public float height = 1f;//墙高
    public float weight = 1f;//宽
    public GameObject buttonObj;//按钮
    public bool flag=false;

    public GameObject _start;
    public GameObject _end;
    public GameObject _cube;
    public Material lineMateria;
    public GameObject inputArea;

    public InputField heightInputfield;//输入框
    public InputField widthInputfield;

    public List<GameObject> walls;

    private void Start()
    {
        layerMask = ~layerMask;
        line = this.gameObject.AddComponent<LineRenderer>();
        //只有设置了材质 setColor才有作用
        line.material = lineMateria;
        line.SetVertexCount(2);//设置两点
                               //line.SetColors(Color.yellow, Color.red); //设置直线颜色
        line.SetWidth(1f, 1f);//设置直线宽度

        line.SetPosition(0, new Vector3(0,-5,0));
        line.SetPosition(1, new Vector3(0, -5, 0));

        buttonObj.GetComponent<Button>().onClick.AddListener(M);//监听器

        _start.layer = 10;
        _end.layer = 10;
        inputArea.SetActive(false);//隐藏输入框

        heightInputfield.text = 1000.ToString();
        widthInputfield.text = 1000.ToString();

    }

    // Update is called once per frame
    private void Update()
    {
        if(flag==true)
        {
            GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type = "Wall";

            GameObject.Find("Main Camera").GetComponent<Modify>().modifyInput.SetActive(false);
            inputArea.SetActive(true);//显示输入框
            height = float.Parse(heightInputfield.text) / 1000f;
            weight = float.Parse(widthInputfield.text) / 1000f;
            line.SetWidth(weight, weight);
            _start.transform.localScale = new Vector3(weight, height, 1);
            _end.transform.localScale = new Vector3(weight, height, 1);
            if (GameObject.Find("Area_Btn").GetComponent<CreatDot>().go)
            {
                Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);
            }
            //Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);//销毁dot
            if (Input.GetKey(KeyCode.LeftShift))//按住shift开启正交
            {
                //print("shift down");
                if (lastPos == null || lastPos == new Vector3())
                {
                    getStart();
                }
                else
                {
                    if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
                    {
                        return;
                    }
                    if (!line.enabled)
                    {
                        //开始画线
                        line.enabled = true;
                        //起点设为输出端口物体位置
                    }
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    //Debug.DrawRay(transform.position, transform.up * hit.distance, Color.yellow);

                    if (Physics.Raycast(ray, out hit, 1000, layerMask))
                    {
                        offSet = hit.point - lastPos;
                        DoMath(offSet);
                        _end.transform.position = curPos;
                        OK();
                    }
                }
                    
            }
            else
            {
                
                if (lastPos == null || lastPos == new Vector3())
                {
                    getStart();
                }
                else
                {
                    getEnd();
                    OK();
                }
            }
            
        }
    }

    private void DoMath(Vector3 offSet)//计算
    {
        if (Mathf.Abs(offSet.x) > Mathf.Abs(offSet.z))
        {
            if (offSet.x > 0)
            {
                Debug.Log("右" + offSet.x);
                curPos = new Vector3(lastPos.x+ offSet.x, lastPos.y, lastPos.z);
            }
            else
            {
                Debug.Log("左" + offSet.x);
                curPos = new Vector3(lastPos.x + offSet.x, lastPos.y, lastPos.z);
            }
        }
        else
        {
            if (offSet.z > 0)
            {
                Debug.Log("上" + offSet.z);
                curPos = new Vector3(lastPos.x, lastPos.y, lastPos.z+ offSet.z);
            }
            else
            {
                Debug.Log("下" + offSet.z);
                curPos = new Vector3(lastPos.x, lastPos.y, lastPos.z+ offSet.z);
            }
        }
    }

    private void getStart()
    {
        if (Input.GetMouseButtonDown(0))//获取起点
        {
            if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
            {
                return;
            }
            if (!line.enabled)
            {
                //开始画线
                line.enabled = true;
                //起点设为输出端口物体位置
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(transform.position, transform.up * hit.distance, Color.yellow);
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                lastPos = hit.point;
                _start.transform.position = lastPos;


            }

        }
    }

    private void getEnd()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//获取终点
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            curPos = hit.point;
            _end.transform.position = curPos;

        }
    }

    private float GetAngle(Vector3 a, Vector3 b)
    {
        b.x -= a.x;
        b.z -= a.z;

        float deltaAngle = 0;
        if (b.x == 0 && b.z == 0)
        {
            return 0;
        }
        else if (b.x > 0 && b.z > 0)
        {
            deltaAngle = 0;
        }
        else if (b.x > 0 && b.z == 0)
        {
            return 90;
        }
        else if (b.x > 0 && b.z < 0)
        {
            deltaAngle = 180;
        }
        else if (b.x == 0 && b.z < 0)
        {
            return 180;
        }
        else if (b.x < 0 && b.z < 0)
        {
            deltaAngle = -180;
        }
        else if (b.x < 0 && b.z == 0)
        {
            return -90;
        }
        else if (b.x < 0 && b.z > 0)
        {
            deltaAngle = 0;
        }

        float angle = Mathf.Atan(b.x / b.z) * Mathf.Rad2Deg + deltaAngle;
        Debug.Log(angle);
        return angle;
    }

    public void M()
    {
        flag = true;
        
        print("执行了M方法!");
    }

    private void OK()
    {
        line.SetPosition(0, new Vector3(lastPos.x, lastPos.y + .5f, lastPos.z));
        line.SetPosition(1, new Vector3(curPos.x, curPos.y + .5f, curPos.z));

        length = Mathf.Sqrt(Mathf.Pow((curPos.x - lastPos.x), 2) + Mathf.Pow((curPos.z - lastPos.z), 2)) + 1;//计算长度
        createPos = (lastPos + curPos) / 2;//计算位置
                                           //createPos.y = height / 2 + 0.5f;
        createPos.y = hit.point.y + height / 2;

        _start.transform.localEulerAngles = new Vector3(0, GetAngle(lastPos, curPos), 0);
        _end.transform.localEulerAngles = new Vector3(0, GetAngle(lastPos, curPos), 0);

        if (Input.GetMouseButtonDown(0))//确认
        {
            if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
            {
                return;
            }
            curPos.y += 1;
            lastPos.y += 1;

            GameObject obj = Instantiate(_cube);//生成墙体
            obj.transform.position = createPos;//位置
            obj.layer = 10;//更改遮罩层为10
            obj.GetComponent<Transform>().localScale = new Vector3(weight, height, length);//高度，长度
            obj.transform.localEulerAngles = new Vector3(0, GetAngle(lastPos, curPos), 0);//旋转
            obj.GetComponent<MeshRenderer>().material = color;//墙体颜色
            obj.transform.tag = "wall";
            walls.Add(obj);


            //obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off; //关闭阴影
            line.enabled = false;//删除线
                                 //清空
            lastPos = new Vector3(0, 0, 0);
            curPos = new Vector3(0, 0, 0);
            offSet = new Vector3(0, 0, 0);
            _start.transform.position = new Vector3(0, -5, 0);//初始化俩点位置
            _end.transform.position = new Vector3(0, -5, 0);
        }


        if (Input.GetMouseButtonDown(1))//取消
        {
            line.enabled = false;
            lastPos = new Vector3(0, 0, 0);
            curPos = new Vector3(0, 0, 0);
            offSet = new Vector3(0, 0, 0);
            _start.transform.position = new Vector3(0, -5, 0);//初始化俩点位置
            _end.transform.position = new Vector3(0, -5, 0);
        }
    }

}
