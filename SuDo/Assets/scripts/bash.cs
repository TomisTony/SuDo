using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bash : MonoBehaviour
{
    public Animator ani;//animator������
    int direction;

    enum Priority
    //���ȼ�ö��
    {
        idle,//����
        move,//�ж�
        tap,//���
        bash,//�ػ�
        Su,//��
        do_do,//�ɣ�������Ϊdo����Ϊ�����֣�
        hurt,//����
        dodge//����
    }

    // Start is called before the first frame update
    void Start()
    {
        direction = GameObject.Find("Player").GetComponent<player>().faceDirection;
        this.ani.SetInteger("direction", direction);
    }

    // Update is called once per frame
    void Update()
    {
        //destroy���ʵ��������
        int state;
        state = GameObject.Find("Player").GetComponent<player>().nowAction;
        if (state != (int)Priority.bash)
            Destroy(gameObject);
    }
}
