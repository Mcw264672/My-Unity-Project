using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyFloor : MonoBehaviour
{
    private int layerMask = 1 << 9;//遮罩层10086
    public RaycastHit hit;//射线
    public RaycastHit hit2;//射线2
    public GameObject modifyInput;
    public InputField height;
    public InputField width;
    public InputField length;
    public InputField roation;
    public Material check;
    public Material white;
    public bool flag = false;
    public bool moveFlag = false;
    public bool sFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        modifyInput.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type == "Modify_Btn")
        {
            flag = true;
            modifyInput.SetActive(true);
            if (GameObject.Find("Area_Btn").GetComponent<CreatDot>().go)//清除面积指示
            {
                Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(transform.position, transform.up * hit.distance, Color.yellow);
            if (Input.GetMouseButtonDown(1) && moveFlag == false)//按下鼠标
            {
                if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
                {
                    return;
                }
                if (Physics.Raycast(ray, out hit, 1000, layerMask))
                {
                    for (int i = 0; i < GameObject.Find("Floor_Btn").GetComponent<CreateFloor>().walls.Count; ++i)
                    {
                        GameObject.Find("Floor_Btn").GetComponent<CreateFloor>().walls[i].transform.GetComponent<MeshRenderer>().material = white;
                    }//清理颜色
                    hit.transform.GetComponent<MeshRenderer>().material = check;
                    height.text = (hit.transform.localScale.y * 1000f).ToString();
                    width.text = (hit.transform.localScale.x * 1000f).ToString();
                    length.text = (hit.transform.localScale.z * 1000f).ToString();
                    roation.text = hit.transform.localEulerAngles.y.ToString();
                }
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {

                {
                    for (int i = 0; i < GameObject.Find("Floor_Btn").GetComponent<CreateFloor>().walls.Count; ++i)
                    {
                        if (GameObject.Find("Floor_Btn").GetComponent<CreateFloor>().walls[i] == hit.transform.gameObject)
                        {
                            GameObject.Find("Floor_Btn").GetComponent<CreateFloor>().walls.Remove(GameObject.Find("Floor_Btn").GetComponent<CreateFloor>().walls[i]);
                        }
                    }
                    Destroy(hit.transform.gameObject);
                }

            }

            if (Input.GetMouseButtonDown(0) && moveFlag == false)
            {
                if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
                {
                    return;
                }
                for (int i = 0; i < GameObject.Find("Floor_Btn").GetComponent<CreateFloor>().walls.Count; ++i)
                {
                    GameObject.Find("Floor_Btn").GetComponent<CreateFloor>().walls[i].transform.GetComponent<MeshRenderer>().material = white;
                }//清理颜色

                if (Physics.Raycast(ray, out hit, 1000, layerMask))
                {
                    hit.transform.GetComponent<MeshRenderer>().material = check;
                    height.text = (hit.transform.localScale.y * 1000f).ToString();
                    width.text = (hit.transform.localScale.x * 1000f).ToString();
                    length.text = (hit.transform.localScale.z * 1000f).ToString();
                    roation.text = hit.transform.localEulerAngles.y.ToString();
                    moveFlag = true;
                }
            }

            if (moveFlag == true)
            {
                if (Physics.Raycast(ray, out hit2, 1000, ~layerMask))
                {
                    hit.transform.position = new Vector3(hit2.point.x, hit.transform.position.y, hit2.point.z);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    moveFlag = false;
                }
            }
            hit.transform.localScale = new Vector3(float.Parse(width.text) / 1000, float.Parse(height.text) / 1000, float.Parse(length.text) / 1000);
            hit.transform.localEulerAngles = new Vector3(0, float.Parse(roation.text), 0);

        }

    }
}
