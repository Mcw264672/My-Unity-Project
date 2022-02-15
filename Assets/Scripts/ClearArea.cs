using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearArea : MonoBehaviour
{
    public GameObject buttonObj;//°´Å¥
    public GameObject _text;
    // Start is called before the first frame update
    void Start()
    {
        buttonObj.GetComponent<Button>().onClick.AddListener(M);
    }

    public void M()
    {
        if(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go)
        {
            Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);
        }
        GameObject.Find("Main Camera").GetComponent<Area>().ClearLines();
        _text.GetComponent<Text>().text = "00.00";
    }
}
