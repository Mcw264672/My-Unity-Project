using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Astar 寻路管理器
/// </summary>
public class AStarManager
{
    private int mapW;//宽
    private int mapH;//高

    private static AStarManager instance;

    public static AStarManager Instance
    {
        get
        {
            if (instance == null)
                instance = new AStarManager();
            return instance;
        }
    }

    public AstarNode[,] nodes;//格子对象容器

    private List<AstarNode> openList=new List<AstarNode>();//开启列表

    private List<AstarNode> closeList=new List<AstarNode>();//关闭列表

    public void InitMapInfo(int w,int h)//初始化地图信息
    {
        //根据宽高创建格子
        this.mapW = w;
        this.mapH = h;
        nodes = new AstarNode[w, h];//格子数
        for(int i=0;i<w;i++)//初始化
        {
            for(int j=0;j<h;j++)
            {
                //AstarNode node = new AstarNode(i,j,Random.Range(0,100)<20?E_Node_Type.Stop:E_Node_Type.Walk);//百分之二十几率阻挡
                AstarNode node = new AstarNode(i, j, E_Node_Type.Walk);
                nodes[i, j] = node;
            }
        }
    }

    public List<AstarNode> FindPath(Vector2 startPos,Vector2 endPos)//寻路方法
    {
        //格子位置应该是坐标系中位置除以格子大小向上取整
        //判断合法：在地图范围内

        if (startPos.x < 0 || startPos.x >= mapW||
            startPos.y<0||startPos.y>=mapH||
            endPos.x<0||endPos.x>=mapW||
            endPos.y<0||endPos.y>=mapH)
        {
            Debug.Log("开始或结束点不在地图内");
            return null;
        }
            
        //判断非阻挡
        AstarNode start = nodes[(int)startPos.x, (int)startPos.y];
        AstarNode end = nodes[(int)endPos.x, (int)endPos.y];
        if (start.type == E_Node_Type.Stop || end.type == E_Node_Type.Stop)
        {
            Debug.Log("开始或结束点阻挡");
            return null;
        }
        //清空两个列表
        closeList.Clear();
        openList.Clear();
        //把开始点放入CloseList *******
        start.father = null;
        start.g = 0;
        start.f = 0;
        start.h = 0;
        closeList.Add(start);       
        while(true)
        {
            //合法：从起点开始，找周围的点，左上，上，右上，右，右下，下，左下，左
            //FindNearlyNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x, start.y - 1, 1, start, end);
            //FindNearlyNodeToOpenList(start.x + 1, start.y - 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x - 1, start.y, 1, start, end);
            FindNearlyNodeToOpenList(start.x + 1, start.y, 1, start, end);
            //FindNearlyNodeToOpenList(start.x - 1, start.y + 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x, start.y + 1, 1, start, end);
            //FindNearlyNodeToOpenList(start.x + 1, start.y + 1, 1.4f, start, end);

            if(openList.Count==0)//死路
            {
                return null;
                Debug.Log("死路");
            }

            //选出OpenList中寻路消耗最小的点放入CloseList，将之从OpenList中移除
            openList.Sort(SortOpenList);
            closeList.Add(openList[0]);
            start = openList[0];//下一次的起点
            openList.RemoveAt(0);
            //如果当前点是终点，结束，否则继寻路
            if(start==end)
            {
                List<AstarNode> path = new List<AstarNode>();
                path.Add(end);
                while(end.father!=null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                path.Reverse();
                return path;
            }
        }
    }

    private int SortOpenList(AstarNode a,AstarNode b)
    {
        if (a.f >= b.f) return 1;
        else return -1;
    }
    private void FindNearlyNodeToOpenList(int x,int y,float g,AstarNode father,AstarNode end)//把临近的点放入openList
    {
        //边界
        if (x < 0 || x >= mapW ||
            y < 0 || y >= mapH)
            return;
        //取点
        AstarNode node = nodes[x, y];
        //判断能走，不在任何列表中就加入OpenList
        if(node==null||node.type==E_Node_Type.Stop||
            closeList.Contains(node)||
            openList.Contains(node))//验证
            return;
        //计算f值
        //f=g+h
        node.father = father;
        node.g = father.g + g;//我离起点的距离=我父亲离起点的距离+我离我父亲的距离
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);//计算当前距离终点的曼哈顿距离
        node.f = node.g + node.h;
        //存入开启列表
        openList.Add(node);
        
    }
}
