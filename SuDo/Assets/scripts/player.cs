using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //状态显示的public变量
    public int nowAction;//记录目前正在行动的动作（按照优先级设定）
    public bool canInterrupt;//当前动作是否可被更高优先级打断

    //人物具体数据的public变量
    //速度类（秒为单位）
    public float moveSpeed;//人物move的速度
    public float moveSpeedInDo;//渡期间的人物移动速度
    public float dodgeSpeed;//人物闪避的速度

    //前摇类（秒为单位）
    public float tapOneAheadTime;//轻击第一段前摇
    public float tapTwoAheadTime;//轻击第二段前摇
    public float tapThreeAheadTime;//轻击第三段前摇
    public float bashOneAheadTime;//重击第一段前摇
    public float bashTwoAheadTime;//重击第二段前摇
    public float bashThreeAheadTime;//重击第三段前摇
    public float doAheadTime;//渡的前摇
    public float dodgeAheadTime;//闪避前摇

    //持续时间类（秒为单位）
    public float tapOneTime;//轻击第一段持续时间
    public float tapTwoTime;//轻击第二段持续时间
    public float tapThreeTime;//轻击第三段持续时间
    public float bashOneTime;//重击第一段持续时间
    public float bashTwoTime;//重击第二段持续时间
    public float bashThreeTime;//重击第三段持续时间
    public float doTime;//渡的持续时间
    public float dodgeTime;//闪避持续时间

    //后摇类（秒为单位）
    public float tapOneEndTime;//轻击第一段后摇
    public float tapTwoEndTime;//轻击第二段后摇
    public float tapThreeEndTime;//轻击第三段后摇
    public float bashOneEndTime;//重击第一段后摇
    public float bashTwoEndTime;//重击第二段后摇
    public float bashThreeEndTime;//重击第三段后摇
    public float doEndTime;//渡的后摇
    public float dodgeEndTime;//闪避后摇

    //其他数据类
    public float playerHP;//人物的血量


    //private变量
    private float publicTimer;//通用的计时器（用于当前行动持续时间的计时）

    enum Priority
        //优先级枚举
    {
        idle,//待机
        move,//行动
        tap,//轻击
        bash,//重击
        Su,//溯
        do_do,//渡（不命名为do是因为保留字）
        dodge//闪避
    }

    // Start is called before the first frame update
    void Start()
    {
        this.nowAction = (int)Priority.idle;
        this.canInterrupt = true;
        this.publicTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /*总体思想是在每一帧角色都会默认执行当前状态的延续，但是会提前检测输入的情
        况，然后判断输入情况和角色目前动作优先级以及角色是否处于可打断的状态,如果可
        以打断，那就直接改变角色正要去做的事情，总体的思路还是借鉴了LC-3的Interrupt的思想*/

        publicTimer += Time.deltaTime;//主计时器计时

        ButtonClickListen();//检测鼠标是否有操作
        KeyboardPressListen();//检测键盘是否有操作

        //执行角色目前状态的延续状态
        DoTheWork();
    }

    //检测鼠标是否有操作,并对此做出响应
    void ButtonClickListen()
    {
        //如果有鼠标左键点击
        if (Input.GetMouseButtonDown(0) && this.canInterrupt && this.nowAction < (int)Priority.move)
        {
            //处理函数
            return;
        }

        //如果有鼠标右键点击
        if (Input.GetMouseButtonDown(1) && this.canInterrupt && this.nowAction < (int)Priority.dodge)
        {
            //处理函数
            return;
        }
    }

    //检测键盘是否有操作,并对此做出响应
    void KeyboardPressListen()
    {
        //轻击
        if (Input.GetKeyDown(KeyCode.A) && this.canInterrupt && this.nowAction < (int)Priority.tap)
        {}
        //重击
        if (Input.GetKeyDown(KeyCode.S) && this.canInterrupt && this.nowAction < (int)Priority.bash)
        {}
        //渡相关
        if (Input.GetKeyDown(KeyCode.Space) && this.canInterrupt && this.nowAction < (int)Priority.do_do)
        {}
        //溯_Q
        if (Input.GetKeyDown(KeyCode.Q) && this.canInterrupt && this.nowAction < (int)Priority.Su)
        {}
        //溯_W
        if (Input.GetKeyDown(KeyCode.W) && this.canInterrupt && this.nowAction < (int)Priority.Su)
        {}
        //溯_E
        if (Input.GetKeyDown(KeyCode.E) && this.canInterrupt && this.nowAction < (int)Priority.Su)
        {}
    }

    //执行角色目前状态的延续状态
    void DoTheWork()
    {
        switch (this.nowAction) {
            case (int)Priority.idle:
                //延续闲置状态
                break;
            case (int)Priority.move:
                //延续移动状态
                break;
            case (int)Priority.tap:
                //延续轻击状态
                break;
            case (int)Priority.bash:
                //延续重击状态
                break;
            case (int)Priority.Su:
                //延续溯状态
                break;
            case (int)Priority.do_do:
                //延续渡状态
                break;
            case (int)Priority.dodge:
                //延续闪避状态
                break;
        }

    }

    //计划是在Animation结束之后调用这个函数作为event，自动恢复idle的状态
    void BackToIdle()
    {
        this.nowAction = (int)Priority.idle;
        this.canInterrupt = true;
    }

    //用来向外提供血量变化的接口
    public void changeHp(float HPchange)
    {
        this.playerHP += HPchange;
    }
}
