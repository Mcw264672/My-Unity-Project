using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Area : MonoBehaviour
{
    public Material white;
    public Camera camera;
    public int fontSize = 30;//文字大小
    private static Material lineMaterial;//画线材质

    bool sb = false;
    public GameObject aim;//小球预制体
    private List<Vector3> lv;//GL 绘制的顶点数组  顺序是  0->1  2->3 4->5    取法 0 1 3 5 7 9
    private List<GameObject> aims;
    private Vector3 v3;
    public GameObject _text;

    static void CreateLineMaterial()
    {
        if(!lineMaterial)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");//小球材质
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }    
    }


    public void OnRenderObject()
    {
        CreateLineMaterial();
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        for(int i=0;i<lv.Count;++i)
        {
            GL.Vertex3(lv[i].x, lv[i].y, lv[i].z);
        }
        GL.End();
        GL.PopMatrix();
    }


    private void Start()
    {
        lv = new List<Vector3>();
        aims = new List<GameObject>();
        v3 = new Vector3();

    }

    private void Update()
    {
        if(GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type == "Area_Btn")
        {
            
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                    Vector3 temp = new Vector3(hit.point.x, 1, hit.point.z);//生成点的位置
                /*
                 
                 if(temp.x%0.5f!=0)
                    {
                        if(temp.x % 0.5f > 0.25f)
                        {
                            temp.x += temp.x % 0.5f;
                        }
                        else
                        {
                            temp.x -= temp.x % 0.5f;
                        }
                    }
                    if (temp.z % 0.5f != 0)
                    {
                        if (temp.z % 0.5f > 0.25f)
                        {
                            temp.z += temp.z % 0.5f;
                        }
                        else
                        {
                            temp.z -= temp.z % 0.5f;
                        }
                    }
                 */


                GameObject.Find("Area_Btn").GetComponent<CreatDot>().go.transform.position = temp;//可见

                    if (Input.GetMouseButtonDown(0))//绘制多边形
                    {
                        if(GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
                        {
                            return;
                        }
                        GameObject go = Instantiate(aim, temp, Quaternion.Euler(90, 0, 0)) as GameObject;//创建圆点
                        aims.Add(go);
                        if (lv.Count >= 2)
                        {
                            if (sb)//存入点就是反复存入来自动连线，0--1 1--2 2--3......
                            {
                                lv.RemoveAt(lv.Count - 1);
                                lv.RemoveAt(lv.Count - 1);
                            }
                            lv.Add(lv[lv.Count - 1]);
                            lv.Add(temp);
                            lv.Add(lv[0]);
                            lv.Add(temp);

                            sb = true;
                        }
                        else
                        {
                        lv.Add(temp);
                        }
                    }
            }
        }
        
    }


    private void OnGUI()
    {
        GUIStyle text = new GUIStyle();
        text.fontSize = fontSize;
        if(lv.Count>=2)
        {
            for(int i=0;i<lv.Count-1;i+=2)
            {
                Vector3 s = new Vector3((lv[i].x + lv[i + 1].x) / 2, (lv[i].y + lv[i + 1].y) / 2, (lv[i].z + lv[i + 1].z) / 2);
                Vector3 a = camera.WorldToScreenPoint(s);
                //注意屏幕坐标系与GUI的ui坐标系y轴相反,ToString(".000")保留小数点后3位数，几个零几位数
                //显示线段的长度
                //GUI.Label(new Rect(a.x, Screen.height - a.y, 100, 20), "<color=yellow>" + Vector3.Distance(lv[i],lv[i+1]).ToString(".00")+ "</color>" + "<color=yellow>" + "米" + "</color>", text);

            }
        }
        if(lv.Count>2)
        {
            _text.GetComponent<Text>().text= ComputePolygonArea(lv).ToString(".00");
        }
    }

    public void ClearLines()//清除
    {
        sb = false;
        for (int i = 0; i < aims.Count; i++)
        {
            GameObject.Destroy(aims[i]);
        }
        lv.Clear();
        aims.Clear();
    }


    private double ComputePolygonArea(List<Vector3> points)//计算多边形的面积
    {
        int pointNum = points.Count;
        if (pointNum < 3) return 0.0;
        float s = points[0].z * (points[pointNum - 1].x - points[1].x);
        for(int i=1;i<pointNum;++i)
        {
            s += points[i].z * (points[i - 1].x - points[(i + 1) % pointNum].x);
        }
        return Mathf.Abs(s / 2.0f);
    }


}
