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
        RaycastHit hit; //创建射线

        //参数：当前物体，世界空间的方向，碰撞信息，最大距离，
        if (Physics.Raycast(transform.position, transform.up, out hit, Mathf.Infinity))
        {
            string[] strs = transform.name.Split('_');
            AStarManager.Instance.nodes[int.Parse(strs[0]), int.Parse(strs[1])].type=E_Node_Type.Stop;
            Debug.DrawRay(transform.position, transform.up * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
            //返回检测到的物体的名字
            //Debug.Log(hit.transform.name);
            //返回当前物体的名字
            Debug.Log(transform.name);
            //返回检测到的物体的坐标
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
