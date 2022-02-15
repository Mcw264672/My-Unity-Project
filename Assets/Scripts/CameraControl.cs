using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Target;
    //��ת�ٶ�  
    private float SpeedX = 50;
    private float SpeedY = 50;

    //�Ƕ�����  
    private float MinLimitY = -180;
    private float MaxLimitY = 180;

    //��ת�Ƕ�  
    private float mX = 0.0F;
    private float mY = 0.0F;

    //�Ƿ����ò�ֵ  
    public bool isNeedDamping = true;
    //�ٶ�  
    public float Damping = 10F;

    //�洢�Ƕȵ���Ԫ��  
    private Quaternion mRotation;

    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;

    public float move_speed = .5f;


    public float ScaleSpeed = 1.0f;


    void LateUpdate()
    {

        //����м���ת  
        if (Input.GetMouseButton(2)&&Camera.main.orthographic==false)
        {
            //��ȡ�������  
            mX += Input.GetAxis("Mouse X") * SpeedX * 0.02F;
            mY -= Input.GetAxis("Mouse Y") * SpeedY * 0.02F;
            //��Χ����  
            mY = ClampAngle(mY, MinLimitY, MaxLimitY);
            //������ת  
            mRotation = Quaternion.Euler(mY, mX, 0);
            //�����Ƿ��ֵ��ȡ��ͬ�ĽǶȼ��㷽ʽ  
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

    //�Ƕ�����  
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
        if(Camera.main.orthographic==false)//͸��ģʽ
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
        else//����ģʽ
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
