using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatDot : MonoBehaviour
{
    public Material red;
    public GameObject buttonObj;//按钮
    public GameObject aim;//小球预制体
    public GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        buttonObj.GetComponent<Button>().onClick.AddListener(M);
    }

    // Update is called once per frame
    public void M()
    {
        go = Instantiate(aim, new Vector3(0,0,0), Quaternion.Euler(90, 0, 0)) as GameObject;//创建圆点
        go.GetComponent<MeshRenderer>().material = red;
    }
}
