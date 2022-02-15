using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public InputField input1, input2;
    public int length, width;
    public GameObject buttonObj;//°´Å¥
    public GameObject gameObject;//¼Ä´æ
    // Start is called before the first frame update
    void Start()
    {
        buttonObj.GetComponent<Button>().onClick.AddListener(M);
        GameObject.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        OnClick();
    }
    public void OnClick()
    {
        length = int.Parse(input1.text);
        width = int.Parse(input2.text);
        

    }

    public void M()
    {
        if(length!=0&&width!=0)
        {
            SceneManager.LoadScene("Scenes1");
        }
        
    }
}
