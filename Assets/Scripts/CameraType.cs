using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraType : MonoBehaviour
{
    public GameObject buttonObj;//�л���ť
    public Vector3 perPos;//͸�����λ��
    public Vector3 ortPos;//�������λ��
    public Vector3 angle;//͸������Ƕ�
    private void Start()
    {
        perPos = new Vector3(504, 14, 485);
        angle = new Vector3(35,340,0);
        buttonObj.GetComponent<Button>().onClick.AddListener(M);
        buttonObj.GetComponent<Button>().onClick.AddListener(F);
    }
     public void M()
     {
        if(Camera.main.orthographic == true)
        {
            ortPos = Camera.main.transform.position;
            Camera.main.orthographic = false;
            //Camera.main.transform.localEulerAngles = new Vector3(45, 0, 0);
            Camera.main.transform.position = perPos;
            Camera.main.transform.localEulerAngles = angle;
        }
        else
        {
            perPos = Camera.main.transform.position;
            angle = Camera.main.transform.localEulerAngles;
            Camera.main.orthographic = true;
            
            Camera.main.transform.position = new Vector3(500,20,500);
            
            
            Camera.main.transform.localEulerAngles = new Vector3(90, 0, 0);
        }
         print("ִ����M����!");
     }
     public void F()
     {
             print("ִ����N����!");
     }
}

