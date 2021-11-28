using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bash : MonoBehaviour
{
    public Animator ani;//animator控制器
    int direction;

    enum Priority
    //优先级枚举
    {
        idle,//待机
        move,//行动
        tap,//轻击
        bash,//重击
        Su,//溯
        do_do,//渡（不命名为do是因为保留字）
        hurt,//受伤
        dodge//闪避
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
        //destroy这个实例的条件
        int state;
        state = GameObject.Find("Player").GetComponent<player>().nowAction;
        if (state != (int)Priority.bash)
            Destroy(gameObject);
    }
}
