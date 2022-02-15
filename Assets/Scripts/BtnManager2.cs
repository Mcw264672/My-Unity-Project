using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class BtnManager2 : MonoBehaviour
{
    public string type;

    public Transform btnParent;//��ť�ĸ��ڵ�
    private Button[] btns;
    public Material grew;
    public Material white;

    private void Start()
    {
        //��ʼ�����鳤��
        btns = new Button[btnParent.childCount];
        //�������ڵ������еİ�ť
        for (int i = 0; i < btns.Length; i++)
        {
            //��ֵ��ť
            btns[i] = btnParent.GetChild(i).GetComponent<Button>();
            //Ϊ��ť��ӵ���¼�
            btns[i].onClick.AddListener(OnClick);

        }

    }

    private void OnClick()
    {
        //����ʱ �жϵ�ǰ����İ�ť������
        type = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name;
        GameObject.Find("Main Camera").GetComponent<GetDirection>().flag = false;
        GameObject.Find("Floor_Btn").GetComponent<CreateFloor>().flag = false;
        GameObject.Find("Main Camera").GetComponent<GetDirection>().inputArea.SetActive(false);
        GameObject.Find("Floor_Btn").GetComponent<CreateFloor>().inputArea.SetActive(false);
        if (GameObject.Find("Idle").GetComponent<MoveControl>().flag == true)
        {
            GameObject.Find("Idle").GetComponent<MoveControl>().flag = false;
            GameObject.Find("Idle").GetComponent<MoveControl>().navMeshAgent.baseOffset = -100f;
        }
        GameObject.Find("Main Camera").GetComponent<Modify>().modifyInput.SetActive(false);
        if (GameObject.Find("Main Camera").GetComponent<Modify>().flag == true)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<GetDirection>().walls.Count; ++i)
            {
                if (GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].tag == "wall")
                {
                    GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].transform.GetComponent<MeshRenderer>().material = grew;
                }
                else if (GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].tag == "floor")
                {
                    GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].transform.GetComponent<MeshRenderer>().material = white;
                }

            }//������ɫ
            GameObject.Find("Main Camera").GetComponent<Modify>().flag = false;

        }
        //print("��ǰ��ť��: " + type);
    }
}
