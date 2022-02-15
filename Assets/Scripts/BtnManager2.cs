using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class BtnManager2 : MonoBehaviour
{
    public string type;

    public Transform btnParent;//按钮的父节点
    private Button[] btns;
    public Material grew;
    public Material white;

    private void Start()
    {
        //初始化数组长度
        btns = new Button[btnParent.childCount];
        //便利父节点下所有的按钮
        for (int i = 0; i < btns.Length; i++)
        {
            //赋值按钮
            btns[i] = btnParent.GetChild(i).GetComponent<Button>();
            //为按钮添加点击事件
            btns[i].onClick.AddListener(OnClick);

        }

    }

    private void OnClick()
    {
        //按下时 判断当前点击的按钮的名字
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

            }//清理颜色
            GameObject.Find("Main Camera").GetComponent<Modify>().flag = false;

        }
        //print("当前按钮是: " + type);
    }
}
