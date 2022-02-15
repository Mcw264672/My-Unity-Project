using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Astar Ѱ·������
/// </summary>
public class AStarManager
{
    private int mapW;//��
    private int mapH;//��

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

    public AstarNode[,] nodes;//���Ӷ�������

    private List<AstarNode> openList=new List<AstarNode>();//�����б�

    private List<AstarNode> closeList=new List<AstarNode>();//�ر��б�

    public void InitMapInfo(int w,int h)//��ʼ����ͼ��Ϣ
    {
        //���ݿ�ߴ�������
        this.mapW = w;
        this.mapH = h;
        nodes = new AstarNode[w, h];//������
        for(int i=0;i<w;i++)//��ʼ��
        {
            for(int j=0;j<h;j++)
            {
                //AstarNode node = new AstarNode(i,j,Random.Range(0,100)<20?E_Node_Type.Stop:E_Node_Type.Walk);//�ٷ�֮��ʮ�����赲
                AstarNode node = new AstarNode(i, j, E_Node_Type.Walk);
                nodes[i, j] = node;
            }
        }
    }

    public List<AstarNode> FindPath(Vector2 startPos,Vector2 endPos)//Ѱ·����
    {
        //����λ��Ӧ��������ϵ��λ�ó��Ը��Ӵ�С����ȡ��
        //�жϺϷ����ڵ�ͼ��Χ��

        if (startPos.x < 0 || startPos.x >= mapW||
            startPos.y<0||startPos.y>=mapH||
            endPos.x<0||endPos.x>=mapW||
            endPos.y<0||endPos.y>=mapH)
        {
            Debug.Log("��ʼ������㲻�ڵ�ͼ��");
            return null;
        }
            
        //�жϷ��赲
        AstarNode start = nodes[(int)startPos.x, (int)startPos.y];
        AstarNode end = nodes[(int)endPos.x, (int)endPos.y];
        if (start.type == E_Node_Type.Stop || end.type == E_Node_Type.Stop)
        {
            Debug.Log("��ʼ��������赲");
            return null;
        }
        //��������б�
        closeList.Clear();
        openList.Clear();
        //�ѿ�ʼ�����CloseList *******
        start.father = null;
        start.g = 0;
        start.f = 0;
        start.h = 0;
        closeList.Add(start);       
        while(true)
        {
            //�Ϸ�������㿪ʼ������Χ�ĵ㣬���ϣ��ϣ����ϣ��ң����£��£����£���
            //FindNearlyNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x, start.y - 1, 1, start, end);
            //FindNearlyNodeToOpenList(start.x + 1, start.y - 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x - 1, start.y, 1, start, end);
            FindNearlyNodeToOpenList(start.x + 1, start.y, 1, start, end);
            //FindNearlyNodeToOpenList(start.x - 1, start.y + 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x, start.y + 1, 1, start, end);
            //FindNearlyNodeToOpenList(start.x + 1, start.y + 1, 1.4f, start, end);

            if(openList.Count==0)//��·
            {
                return null;
                Debug.Log("��·");
            }

            //ѡ��OpenList��Ѱ·������С�ĵ����CloseList����֮��OpenList���Ƴ�
            openList.Sort(SortOpenList);
            closeList.Add(openList[0]);
            start = openList[0];//��һ�ε����
            openList.RemoveAt(0);
            //�����ǰ�����յ㣬�����������Ѱ·
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
    private void FindNearlyNodeToOpenList(int x,int y,float g,AstarNode father,AstarNode end)//���ٽ��ĵ����openList
    {
        //�߽�
        if (x < 0 || x >= mapW ||
            y < 0 || y >= mapH)
            return;
        //ȡ��
        AstarNode node = nodes[x, y];
        //�ж����ߣ������κ��б��оͼ���OpenList
        if(node==null||node.type==E_Node_Type.Stop||
            closeList.Contains(node)||
            openList.Contains(node))//��֤
            return;
        //����fֵ
        //f=g+h
        node.father = father;
        node.g = father.g + g;//�������ľ���=�Ҹ��������ľ���+�����Ҹ��׵ľ���
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);//���㵱ǰ�����յ�������پ���
        node.f = node.g + node.h;
        //���뿪���б�
        openList.Add(node);
        
    }
}
