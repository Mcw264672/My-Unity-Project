using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Node_Type
{
    Walk, //����
    Stop, //������
}
/// <summary>
/// AStar������
/// </summary>
public class AstarNode
{
    public float f;//Ѱ·����  
    public float g;//�����ľ���
    public float h;//���յ�ľ���
    public AstarNode father;//������
    public int x, y;//����

    public E_Node_Type type;//��������

    public AstarNode(int x,int y, E_Node_Type type) //���캯��
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }


}
