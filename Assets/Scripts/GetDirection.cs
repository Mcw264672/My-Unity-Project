using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetDirection : MonoBehaviour
{
    public Vector3 lastPos;//���
    public Vector3 curPos;//�յ�
    public Vector3 offSet;//ƫ����
    public RaycastHit hit;//����
    private LineRenderer line;//����
    private Vector3 createPos;//����λ��
    public Material color;//��ɫ
    private int layerMask = 1 << 10;
    private float length = .0f;//����
    public float height = 1f;//ǽ��
    public float weight = 1f;//��
    public GameObject buttonObj;//��ť
    public bool flag=false;

    public GameObject _start;
    public GameObject _end;
    public GameObject _cube;
    public Material lineMateria;
    public GameObject inputArea;

    public InputField heightInputfield;//�����
    public InputField widthInputfield;

    public List<GameObject> walls;

    private void Start()
    {
        layerMask = ~layerMask;
        line = this.gameObject.AddComponent<LineRenderer>();
        //ֻ�������˲��� setColor��������
        line.material = lineMateria;
        line.SetVertexCount(2);//��������
                               //line.SetColors(Color.yellow, Color.red); //����ֱ����ɫ
        line.SetWidth(1f, 1f);//����ֱ�߿��

        line.SetPosition(0, new Vector3(0,-5,0));
        line.SetPosition(1, new Vector3(0, -5, 0));

        buttonObj.GetComponent<Button>().onClick.AddListener(M);//������

        _start.layer = 10;
        _end.layer = 10;
        inputArea.SetActive(false);//���������

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
            inputArea.SetActive(true);//��ʾ�����
            height = float.Parse(heightInputfield.text) / 1000f;
            weight = float.Parse(widthInputfield.text) / 1000f;
            line.SetWidth(weight, weight);
            _start.transform.localScale = new Vector3(weight, height, 1);
            _end.transform.localScale = new Vector3(weight, height, 1);
            if (GameObject.Find("Area_Btn").GetComponent<CreatDot>().go)
            {
                Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);
            }
            //Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);//����dot
            if (Input.GetKey(KeyCode.LeftShift))//��סshift��������
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
                        //��ʼ����
                        line.enabled = true;
                        //�����Ϊ����˿�����λ��
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

    private void DoMath(Vector3 offSet)//����
    {
        if (Mathf.Abs(offSet.x) > Mathf.Abs(offSet.z))
        {
            if (offSet.x > 0)
            {
                Debug.Log("��" + offSet.x);
                curPos = new Vector3(lastPos.x+ offSet.x, lastPos.y, lastPos.z);
            }
            else
            {
                Debug.Log("��" + offSet.x);
                curPos = new Vector3(lastPos.x + offSet.x, lastPos.y, lastPos.z);
            }
        }
        else
        {
            if (offSet.z > 0)
            {
                Debug.Log("��" + offSet.z);
                curPos = new Vector3(lastPos.x, lastPos.y, lastPos.z+ offSet.z);
            }
            else
            {
                Debug.Log("��" + offSet.z);
                curPos = new Vector3(lastPos.x, lastPos.y, lastPos.z+ offSet.z);
            }
        }
    }

    private void getStart()
    {
        if (Input.GetMouseButtonDown(0))//��ȡ���
        {
            if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
            {
                return;
            }
            if (!line.enabled)
            {
                //��ʼ����
                line.enabled = true;
                //�����Ϊ����˿�����λ��
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//��ȡ�յ�
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
        
        print("ִ����M����!");
    }

    private void OK()
    {
        line.SetPosition(0, new Vector3(lastPos.x, lastPos.y + .5f, lastPos.z));
        line.SetPosition(1, new Vector3(curPos.x, curPos.y + .5f, curPos.z));

        length = Mathf.Sqrt(Mathf.Pow((curPos.x - lastPos.x), 2) + Mathf.Pow((curPos.z - lastPos.z), 2)) + 1;//���㳤��
        createPos = (lastPos + curPos) / 2;//����λ��
                                           //createPos.y = height / 2 + 0.5f;
        createPos.y = hit.point.y + height / 2;

        _start.transform.localEulerAngles = new Vector3(0, GetAngle(lastPos, curPos), 0);
        _end.transform.localEulerAngles = new Vector3(0, GetAngle(lastPos, curPos), 0);

        if (Input.GetMouseButtonDown(0))//ȷ��
        {
            if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
            {
                return;
            }
            curPos.y += 1;
            lastPos.y += 1;

            GameObject obj = Instantiate(_cube);//����ǽ��
            obj.transform.position = createPos;//λ��
            obj.layer = 10;//�������ֲ�Ϊ10
            obj.GetComponent<Transform>().localScale = new Vector3(weight, height, length);//�߶ȣ�����
            obj.transform.localEulerAngles = new Vector3(0, GetAngle(lastPos, curPos), 0);//��ת
            obj.GetComponent<MeshRenderer>().material = color;//ǽ����ɫ
            obj.transform.tag = "wall";
            walls.Add(obj);


            //obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off; //�ر���Ӱ
            line.enabled = false;//ɾ����
                                 //���
            lastPos = new Vector3(0, 0, 0);
            curPos = new Vector3(0, 0, 0);
            offSet = new Vector3(0, 0, 0);
            _start.transform.position = new Vector3(0, -5, 0);//��ʼ������λ��
            _end.transform.position = new Vector3(0, -5, 0);
        }


        if (Input.GetMouseButtonDown(1))//ȡ��
        {
            line.enabled = false;
            lastPos = new Vector3(0, 0, 0);
            curPos = new Vector3(0, 0, 0);
            offSet = new Vector3(0, 0, 0);
            _start.transform.position = new Vector3(0, -5, 0);//��ʼ������λ��
            _end.transform.position = new Vector3(0, -5, 0);
        }
    }

}
