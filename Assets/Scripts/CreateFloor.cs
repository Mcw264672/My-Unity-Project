using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateFloor : MonoBehaviour
{
    public Vector3 lastPos;//���
    public Vector3 curPos;//�յ�
    public Vector3 offSet;//ƫ����
    public RaycastHit hit;//����
    private Vector3 createPos;//����λ��
    public Material color;//��ɫ
    private float length = .0f;//����
    public float height = 1f;//�ذ��
    public float weight = 1f;//��
    public GameObject buttonObj;//��ť
    public bool flag = false;

    public GameObject _start;
    public GameObject _end;
    public GameObject _cube;
    public Material lineMateria;
    public GameObject inputArea;

    public InputField heightInputfield;//�����

    public List<GameObject> walls;

    private Vector3 temp;//��ʱ��¼
    private int layerMask = 1 << 9;//�ų�����

    private void Start()
    {
        layerMask = ~layerMask;
        _start.layer = layerMask;
        _end.layer = layerMask;
        buttonObj.GetComponent<Button>().onClick.AddListener(M);//������
        inputArea.SetActive(false);//���������

        heightInputfield.text = 1000.ToString();

    }

    // Update is called once per frame
    private void Update()
    {
        if (flag == true)
        {
            GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type = "CreateFloor";

            GameObject.Find("Main Camera").GetComponent<Modify>().modifyInput.SetActive(false);
            inputArea.SetActive(true);//��ʾ�����
            height = float.Parse(heightInputfield.text) / 1000f;
            _start.transform.localScale = new Vector3(weight, height, 1);
            _end.transform.localScale = new Vector3(weight, height, 1);
            if (GameObject.Find("Area_Btn").GetComponent<CreatDot>().go)
            {
                Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);
            }
            //Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);//����dot
            if (lastPos == null || lastPos == new Vector3())
            {
                if (Input.GetMouseButtonDown(0))//��ȡ���
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//��ȡ�յ�
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

                length = Mathf.Sqrt(Mathf.Pow((curPos.x - lastPos.x), 2) + Mathf.Pow((curPos.z - lastPos.z), 2));//���㳤��
                createPos = (lastPos + curPos) / 2;//����λ��
                //createPos.y = height / 2 + 0.5f;
                createPos.y = temp.y-height/2;
                DoMath(offSet);

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
                    obj.layer = 9;//�������ֲ�Ϊ10
                    obj.transform.tag = "floor";
                    obj.GetComponent<Transform>().localScale = new Vector3(weight, height, length);//�߶ȣ�����
                    obj.transform.localEulerAngles = new Vector3(0, GetAngle(lastPos, curPos), 0);//��ת
                    //obj.GetComponent<MeshRenderer>().material = color;//ǽ����ɫ
                    GameObject.Find("Main Camera").GetComponent<GetDirection>().walls.Add(obj);


                    //obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off; //�ر���Ӱ
                    //���
                    lastPos = new Vector3(0, 0, 0);
                    curPos = new Vector3(0, 0, 0);
                    offSet = new Vector3(0, 0, 0);
                    _start.transform.position = new Vector3(0, -5, 0);//��ʼ������λ��
                    _end.transform.position = new Vector3(0, -5, 0);
                }
                if (Input.GetMouseButtonDown(1))//ȡ��
                {
                    lastPos = new Vector3(0, 0, 0);
                    curPos = new Vector3(0, 0, 0);
                    offSet = new Vector3(0, 0, 0);
                    _start.transform.position = new Vector3(0, -5, 0);//��ʼ������λ��
                    _end.transform.position = new Vector3(0, -5, 0);
                }

            }
        }
    }

    private void DoMath(Vector3 offSet)//����
    {
        if (Mathf.Abs(offSet.x) > Mathf.Abs(offSet.y))
        {
            if (offSet.x > 0)
            {
                Debug.Log("��" + offSet.x);
            }
            else
            {
                Debug.Log("��" + offSet.x);
            }
        }
        else
        {
            if (offSet.z > 0)
            {
                Debug.Log("��" + offSet.z);
            }
            else
            {
                Debug.Log("��" + offSet.z);
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

        print("ִ����M����!");
    }

}

