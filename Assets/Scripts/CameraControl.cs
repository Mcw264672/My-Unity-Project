using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Target;
    //旋转速度  
    private float SpeedX = 50;
    private float SpeedY = 50;

    //角度限制  
    private float MinLimitY = -180;
    private float MaxLimitY = 180;

    //旋转角度  
    private float mX = 0.0F;
    private float mY = 0.0F;

    //是否启用差值  
    public bool isNeedDamping = true;
    //速度  
    public float Damping = 10F;

    //存储角度的四元数  
    private Quaternion mRotation;

    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;

    public float move_speed = .5f;


    public float ScaleSpeed = 1.0f;


    void LateUpdate()
    {

        //鼠标中键旋转  
        if (Input.GetMouseButton(2)&&Camera.main.orthographic==false)
        {
            //获取鼠标输入  
            mX += Input.GetAxis("Mouse X") * SpeedX * 0.02F;
            mY -= Input.GetAxis("Mouse Y") * SpeedY * 0.02F;
            //范围限制  
            mY = ClampAngle(mY, MinLimitY, MaxLimitY);
            //计算旋转  
            mRotation = Quaternion.Euler(mY, mX, 0);
            //根据是否插值采取不同的角度计算方式  
            if (isNeedDamping)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, mRotation, Time.deltaTime * Damping);
            }
            else
            {
                transform.rotation = mRotation;
            }
        }
    }

    //角度限制  
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        Zoom();
        Move();
        
    }

    void Zoom()
    {
        if(Camera.main.orthographic==false)//透视模式
        {
            //Zoom out
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Camera.main.transform.Translate(0, 0, -1 * ScaleSpeed);
            }
            //Zoom in
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Camera.main.transform.Translate(0, 0, 1 * ScaleSpeed);
            }
        }
        else//正交模式
        {
            //Zoom out
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                
                Camera.main.orthographicSize += ScaleSpeed;
            }
            //Zoom in
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if(Camera.main.orthographicSize>1)
                {
                    Camera.main.orthographicSize -= ScaleSpeed;
                }
               
                
            }
        }
        


    }

    void Move()
    {
        if (Input.GetMouseButton(1))
        {
            transform.Translate(Vector3.left * Input.GetAxis("Mouse X") * move_speed);
            transform.Translate(Vector3.up * Input.GetAxis("Mouse Y") * -move_speed);
        }
    }
}
