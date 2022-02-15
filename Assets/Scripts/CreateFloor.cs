using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateFloor : MonoBehaviour
{
    public Vector3 lastPos;//起点
    public Vector3 curPos;//终点
    public Vector3 offSet;//偏移量
    public RaycastHit hit;//射线
    private Vector3 createPos;//创建位置
    public Material color;//颜色
    private float length = .0f;//长度
    public float height = 1f;//地板高
    public float weight = 1f;//宽
    public GameObject buttonObj;//按钮
    public bool flag = false;

    public GameObject _start;
    public GameObject _end;
    public GameObject _cube;
    public Material lineMateria;
    public GameObject inputArea;

    public InputField heightInputfield;//输入框

    public List<GameObject> walls;

    private Vector3 temp;//临时记录
    private int layerMask = 1 << 9;//排除顶端

    private void Start()
    {
        layerMask = ~layerMask;
        _start.layer = layerMask;
        _end.layer = layerMask;
        buttonObj.GetComponent<Button>().onClick.AddListener(M);//监听器
        inputArea.SetActive(false);//隐藏输入框

        heightInputfield.text = 1000.ToString();

    }

    // Update is called once per frame
    private void Update()
    {
        if (flag == true)
        {
            GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type = "CreateFloor";

            GameObject.Find("Main Camera").GetComponent<Modify>().modifyInput.SetActive(false);
            inputArea.SetActive(true);//显示输入框
            height = float.Parse(heightInputfield.text) / 1000f;
            _start.transform.localScale = new Vector3(weight, height, 1);
            _end.transform.localScale = new Vector3(weight, height, 1);
            if (GameObject.Find("Area_Btn").GetComponent<CreatDot>().go)
            {
                Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);
            }
            //Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);//销毁dot
            if (lastPos == null || lastPos == new Vector3())
            {
                if (Input.GetMouseButtonDown(0))//获取起点
                {
                    if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
                    {
                        return;
                    }
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    //Debug.DrawRay(transform.position, transform.up * hit.distance, Color.yellow);
                    if (Physics.Raycast(ray, out hit, 1000, layerMask))
                    {
                        lastPos = new Vector3(hit.point.x,hit.point.y+height, hit.point.z) ;
                        temp = lastPos;
                        _start.transform.position = lastPos;
                    }

                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//获取终点
                if (Physics.Raycast(ray, out hit, 1000,layerMask))
                {

                    weight = lastPos.z-hit.point.z+weight/2;

                    curPos = new Vector3(hit.point.x, lastPos.y, temp.z-weight/2);
                    lastPos= new Vector3(lastPos.x, lastPos.y, temp.z - weight / 2);
                    _start.transform.localScale = new Vector3(weight,height,(hit.point.x-lastPos.x)/2);
                    _start.transform.position = new Vector3(temp.x + (hit.point.x - lastPos.x) / 4, _start.transform.position.y, temp.z-weight/2);
                    _end.transform.position = new Vector3(hit.point.x - (hit.point.x - lastPos.x) / 4, _start.transform.position.y, temp.z-weight/2);
                    _end.transform.localScale = new Vector3(weight,height,(hit.point.x-lastPos.x)/2);

                }
                offSet = curPos - lastPos;

                length = Mathf.Sqrt(Mathf.Pow((curPos.x - lastPos.x), 2) + Mathf.Pow((curPos.z - lastPos.z), 2));//计算长度
                createPos = (lastPos + curPos) / 2;//计算位置
                //createPos.y = height / 2 + 0.5f;
                createPos.y = temp.y-height/2;
                DoMath(offSet);

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
                    obj.layer = 9;//更改遮罩层为10
                    obj.transform.tag = "floor";
                    obj.GetComponent<Transform>().localScale = new Vector3(weight, height, length);//高度，长度
                    obj.transform.localEulerAngles = new Vector3(0, GetAngle(lastPos, curPos), 0);//旋转
                    //obj.GetComponent<MeshRenderer>().material = color;//墙体颜色
                    GameObject.Find("Main Camera").GetComponent<GetDirection>().walls.Add(obj);


                    //obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off; //关闭阴影
                    //清空
                    lastPos = new Vector3(0, 0, 0);
                    curPos = new Vector3(0, 0, 0);
                    offSet = new Vector3(0, 0, 0);
                    _start.transform.position = new Vector3(0, -5, 0);//初始化俩点位置
                    _end.transform.position = new Vector3(0, -5, 0);
                }
                if (Input.GetMouseButtonDown(1))//取消
                {
                    lastPos = new Vector3(0, 0, 0);
                    curPos = new Vector3(0, 0, 0);
                    offSet = new Vector3(0, 0, 0);
                    _start.transform.position = new Vector3(0, -5, 0);//初始化俩点位置
                    _end.transform.position = new Vector3(0, -5, 0);
                }

            }
        }
    }

    private void DoMath(Vector3 offSet)//计算
    {
        if (Mathf.Abs(offSet.x) > Mathf.Abs(offSet.y))
        {
            if (offSet.x > 0)
            {
                Debug.Log("右" + offSet.x);
            }
            else
            {
                Debug.Log("左" + offSet.x);
            }
        }
        else
        {
            if (offSet.z > 0)
            {
                Debug.Log("上" + offSet.z);
            }
            else
            {
                Debug.Log("下" + offSet.z);
            }
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

}

