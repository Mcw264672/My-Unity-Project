using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// length----z
/// width----x
/// heigth----y
/// </summary>
public class Modify : MonoBehaviour
{
    private int layerMask = 1 << 10;//遮罩层10
    public RaycastHit hit;//射线
    public RaycastHit hit2;//射线2
    public GameObject modifyInput;
    public InputField height;
    public InputField width;
    public InputField length;
    public InputField roation;
    public Material check;
    public Material grew;
    public bool flag=false;
    public bool moveFlag = false;
    public bool sFlag = false;
    private float pre;
    private Transform target;

    public Material white;
    // Start is called before the first frame update
    void Start()
    {
        layerMask += 1<<9;
        modifyInput.SetActive(false);
        height.onEndEdit.AddListener(delegate { heightInputEnd(height); });
        width.onEndEdit.AddListener(delegate { widthInputEnd(width); });
        length.onEndEdit.AddListener(delegate { lengthInputEnd(length); });
        roation.onEndEdit.AddListener(delegate { roationInputEnd(roation); });
    }
    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type == "Modify_Btn")
        {
            flag = true;
            modifyInput.SetActive(true);
            if (GameObject.Find("Area_Btn").GetComponent<CreatDot>().go)//清除面积指示
            {
                Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);
            }
            //创建射线，只检测10+9号层
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(transform.position, transform.up * hit.distance, Color.yellow);
            if (Input.GetMouseButtonDown(1)&&moveFlag==false)//按下鼠标
            {
                if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
                {
                    return;
                }
                if (Physics.Raycast(ray, out hit, 1000, layerMask))
                {
                    for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<GetDirection>().walls.Count; ++i)
                    {
                        if(GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].transform.tag=="wall")
                        {
                            GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].transform.GetComponent<MeshRenderer>().material = grew;
                        }
                        else if(GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].transform.tag=="floor")
                        {
                            GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].transform.GetComponent<MeshRenderer>().material = white;
                        }
                        
                    }//清理颜色
                    target = hit.transform;
                    target.GetComponent<MeshRenderer>().material = check;
                    pre = target.localScale.y;
                    
                    height.text = (target.localScale.y*1000f).ToString();
                    width.text = (target.localScale.x*1000f).ToString();
                    length.text = (target.localScale.z*1000f).ToString();
                    roation.text = target.localEulerAngles.y.ToString();
                }
            }
            
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                
                {
                    for(int i=0;i< GameObject.Find("Main Camera").GetComponent<GetDirection>().walls.Count;++i)
                    {
                        if(GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i]==hit.transform.gameObject)
                        {
                            GameObject.Find("Main Camera").GetComponent<GetDirection>().walls.Remove(GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i]);
                        }
                    }
                    Destroy(target.gameObject);
                }
                
            }

            if(Input.GetMouseButtonDown(0)&&moveFlag==false)
            {
                if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
                {
                    return;
                }
                for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<GetDirection>().walls.Count; ++i)
                {
                    if (GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].transform.tag == "wall")
                    {
                        GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].transform.GetComponent<MeshRenderer>().material = grew;
                    }
                    else if (GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].transform.tag == "floor")
                    {
                        GameObject.Find("Main Camera").GetComponent<GetDirection>().walls[i].transform.GetComponent<MeshRenderer>().material = white;
                    }

                }//清理颜色

                if (Physics.Raycast(ray, out hit, 1000, layerMask))
                {
                    target = hit.transform;
                    target.GetComponent<MeshRenderer>().material = check;
                    pre = target.localScale.y;
                    height.text = (target.localScale.y * 1000f).ToString();
                    width.text = (target.localScale.x * 1000f).ToString();
                    length.text = (target.localScale.z * 1000f).ToString();
                    roation.text = target.localEulerAngles.y.ToString();
                    moveFlag = true;
                }
            }

            if(moveFlag==true)
            {
                if (Physics.Raycast(ray, out hit2, 1000, ~layerMask))
                {
                    hit.transform.position = new Vector3(hit2.point.x,hit.transform.position.y,hit2.point.z);
                }
                    
                if(Input.GetMouseButtonDown(1))
                {
                    moveFlag = false;
                }
            }
            
            

        }
        
    }
    private void heightInputEnd(InputField height)
    {

        target.position =new Vector3(target.position.x, target.position.y-pre/2+float.Parse(height.text) / 2000, target.position.z) ;
        target.localScale = new Vector3(target.localScale.x, float.Parse(height.text) / 1000, target.localScale.z);
        pre = float.Parse(height.text) / 1000;


    }
    private void widthInputEnd(InputField width)
    {
        target.localScale = new Vector3(float.Parse(width.text) / 1000, target.localScale.y, target.localScale.z);
    }
    private void lengthInputEnd(InputField length)
    {
        target.localScale = new Vector3(target.localScale.x, target.localScale.y, float.Parse(length.text) / 1000);
    }
    private void roationInputEnd(InputField roation)
    {
        target.localEulerAngles = new Vector3(0, float.Parse(roation.text), 0);
    }
}
