using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if(GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type == "Wall_Btn"&& Input.GetMouseButtonDown(0))
        {
            shoot();
        }
    }

    private void shoot()
    {
        RaycastHit hit; //��������

        //��������ǰ���壬����ռ�ķ�����ײ��Ϣ�������룬
        if (Physics.Raycast(transform.position, transform.up, out hit, Mathf.Infinity))
        {
            string[] strs = transform.name.Split('_');
            AStarManager.Instance.nodes[int.Parse(strs[0]), int.Parse(strs[1])].type=E_Node_Type.Stop;
            Debug.DrawRay(transform.position, transform.up * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
            //���ؼ�⵽�����������
            //Debug.Log(hit.transform.name);
            //���ص�ǰ���������
            Debug.Log(transform.name);
            //���ؼ�⵽�����������
            //Debug.Log(hit.transform.position);
            //AStarManager.Instance.nodes
        }
        else
        {
            string[] strs = transform.name.Split('_');
            AStarManager.Instance.nodes[int.Parse(strs[0]), int.Parse(strs[1])].type = E_Node_Type.Walk;
        }
    }
}
