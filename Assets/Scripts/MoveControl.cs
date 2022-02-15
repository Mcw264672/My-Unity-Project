using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveControl : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    private Animator animator;
    public GameObject idle;
    public bool flag = false;
    private int layerMask = 1 << 10;

    private void Awake()
    {
        animator= GetComponent<Animator>();
    }
    private void Start()
    {
        layerMask = ~layerMask;
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        //����NavMeshAgent���½�ɫλ��
        //navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.baseOffset = -100;//hiden

    }

    private void Update()
    {

        if (GameObject.Find("Btn_Parent").GetComponent<BtnManager>().type == "Move")
        {
            if (GameObject.Find("Area_Btn").GetComponent<CreatDot>().go)//������ָʾ
            {
                Destroy(GameObject.Find("Area_Btn").GetComponent<CreatDot>().go);
            }
            if (flag==true)//�Ѵ������
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
                    {
                        return;
                    }
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                
                        navMeshAgent.nextPosition = transform.position;
                        navMeshAgent.SetDestination(hit.point);
                                
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (GameObject.Find("Canvas").GetComponent<OnGUI>().CheckGuiRaycastObjects())
                    {
                        return;
                    }
                    
                    
                    
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 1000, layerMask))
                    {
                        navMeshAgent.baseOffset = hit.point.y-0.13f;
                        navMeshAgent.enabled = false;
                        navMeshAgent.enabled = true;
                        idle.transform.position = hit.point;
                        flag = true;
                    }
                }
                    
            }
            
            
        }
        Move();
        

    }

    private void Move()
    {
        if (navMeshAgent.remainingDistance < 0.5f)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 1.0f)
            animator.SetBool("Bool1", false);
            return;
        }
        navMeshAgent.nextPosition = transform.position;
        if (navMeshAgent.desiredVelocity == Vector3.zero)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 1.0f)
                animator.SetBool("Bool1", false);
            return;
        }
        
        Quaternion targetQuaternion = Quaternion.LookRotation(navMeshAgent.desiredVelocity, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, Time.deltaTime * 10);
        animator.SetBool("Bool1", true);
        //transform.Translate(Vector3.forward * Time.deltaTime * 3);
    }
}