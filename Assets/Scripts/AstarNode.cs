using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Node_Type
{
    Walk, //能走
    Stop, //不能走
}
/// <summary>
/// AStar格子类
/// </summary>
public class AstarNode
{
    public float f;//寻路消耗  
    public float g;//离起点的距离
    public float h;//离终点的距离
    public AstarNode father;//父对象
    public int x, y;//坐标

    public E_Node_Type type;//格子类型

    public AstarNode(int x,int y, E_Node_Type type) //构造函数
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }


}
