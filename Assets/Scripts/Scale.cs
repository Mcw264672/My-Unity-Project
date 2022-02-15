using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour
{
    public GameObject _text;
    public float ScaleSpeed = 1.0f;
    public Dropdown dropdown;
    private bool flag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.orthographic == true)//正交模式下启用比例尺
        {
            //Zoom out
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {

                Camera.main.orthographicSize +=ScaleSpeed ;
                if (flag == false)
                    _text.GetComponent<Text>().text = (float.Parse(_text.GetComponent<Text>().text)+0.4f).ToString(".00");
                else
                    _text.GetComponent<Text>().text = (float.Parse(_text.GetComponent<Text>().text) + 400.0f).ToString(".00");
            }
            //Zoom in
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (Camera.main.orthographicSize > 1)
                {
                    Camera.main.orthographicSize -= ScaleSpeed;
                    if (flag == false)
                        _text.GetComponent<Text>().text = (float.Parse(_text.GetComponent<Text>().text) -0.4f).ToString(".00");
                    else
                        _text.GetComponent<Text>().text = (float.Parse(_text.GetComponent<Text>().text) - 400.0f).ToString(".00");
                }
            }
            
        }

        switch(dropdown.value)
        {
            case 0:
                if(flag==true)
                {
                    _text.GetComponent<Text>().text = (float.Parse(_text.GetComponent<Text>().text) /1000).ToString(".00");
                    flag = false;
                }
                break;
            case 1:
                if(flag==false)
                {
                    _text.GetComponent<Text>().text = (float.Parse(_text.GetComponent<Text>().text) * 1000).ToString(".00");
                    flag = true;
                }
                break;
        }
            
    }
}
