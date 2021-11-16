using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //状态显示的public变量
    public int nowAction;//记录目前正在行动的动作（按照优先级设定）
    public bool canInterrupt;//当前动作是否可被更高优先级打断
    public bool isDo;//表示是否正处于渡的状态

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
    public float shadowExitTime;//影子的存在时间
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

    //储存prefeb的public变量
    public GameObject shadow;//残影


    //private变量
    private float publicTimer;//通用的计时器（用于当前行动持续时间的计时）
    private float shadowTimer;//影子的额外存在计时器
    private Vector3 pos;//记录上一次鼠标的成功输入的点击位置（相对位置）
    private Vector3 direction;//通用的人物位移矢量
    private GameObject shadowInGame;//shadow的实例化
    private bool isShadowSave;//影子是否还存在

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
        this.isDo = false;
        this.isShadowSave = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*总体思想是在每一帧角色都会默认执行当前状态的延续，但是会提前检测输入的情
        况，然后判断输入情况和角色目前动作优先级以及角色是否处于可打断的状态,如果可
        以打断，那就直接改变角色正要去做的事情，总体的思路还是借鉴了LC-3的Interrupt的思想*/

        TimerDeal();        //计时器处理模块
        ButtonClickListen();//检测鼠标是否有操作
        KeyboardPressListen();//检测键盘是否有操作

        //执行角色目前状态的延续状态
        DoTheWork();
    }

    void TimerDeal()
    {
        publicTimer += Time.deltaTime;//主计时器计时
        if (this.isShadowSave && !this.isDo) this.shadowTimer += Time.deltaTime;//渡结束之后影子的存活开始计时
        if(this.shadowTimer >= this.shadowExitTime)//shadow存在的时间到了
        {
            this.isShadowSave = false;
            this.shadowTimer = 0;
            Destroy(shadowInGame);
        }
    }

    //检测鼠标是否有操作,并对此做出响应
    void ButtonClickListen()
    {
        //如果有鼠标左键点击
        if (Input.GetMouseButtonDown(0) && this.canInterrupt && this.nowAction <= (int)Priority.move)
        {
            //处理函数
            this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标指向的位置
            this.pos.z = this.transform.position.z;//将鼠标指向位置的z坐标设置为0
            this.nowAction = (int)Priority.move;
            this.publicTimer = 0;
            this.canInterrupt = true;
            return;
        }

        //如果有鼠标右键点击
        if (Input.GetMouseButtonDown(1) && this.canInterrupt && this.nowAction <= (int)Priority.dodge)
        {
            //处理函数
            this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标指向的位置
            this.pos.z = this.transform.position.z;//将鼠标指向位置的z坐标设置为0
            this.nowAction = (int)Priority.dodge;
            this.publicTimer = 0;
            this.canInterrupt = true;//这里写true是为了统一写法，理论上应该是false，但是false可以直接在
                                       //DoTheWork函数里面写
            return;
        }
    }

    //检测键盘是否有操作,并对此做出响应
    void KeyboardPressListen()
    {
        //所有的priority判断不使用“<=”是因为如果输入相同的攻击方式，那一定是由
        //轻击
        if (Input.GetKeyDown(KeyCode.A) && this.canInterrupt && this.nowAction < (int)Priority.tap && !this.isDo)
        {}
        //重击
        if (Input.GetKeyDown(KeyCode.S) && this.canInterrupt && this.nowAction < (int)Priority.bash && !this.isDo)
        {}
        //渡相关
        //渡的处理比较特殊，因为渡状态其实是作为一个背景板存在的，所以只有按键瞬间的鼠标处理
        if (Input.GetKeyDown(KeyCode.Space) && this.canInterrupt && this.nowAction <= (int)Priority.do_do)
        {
            if(!this.isDo && !isShadowSave)//开启渡
            {
                this.isDo = true;
                this.isShadowSave = true;
                shadowInGame = Instantiate(shadow);
                shadowInGame.transform.position = this.transform.position;//shadow实例化
            }
            else if(this.isDo)//结束渡
            {
                this.isDo = false;
                this.isShadowSave = true;
                this.shadowTimer = 0;
            }
            else if(!this.isDo && isShadowSave)//跟残影交换位置
            {
                Vector3 position = this.transform.position;
                this.transform.position = this.shadowInGame.transform.position;
                this.shadowInGame.transform.position = position;
                this.nowAction = (int)Priority.idle; //切换闲置状态
                this.canInterrupt = true;
            }
        }
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
                this.canInterrupt = true;
                break;
            case (int)Priority.move:
                //延续移动状态
                this.canInterrupt = true;
                this.direction = pos - this.transform.position;
                if (direction.magnitude <= 0.1) //控制人物停下
                {
                    this.nowAction = (int)Priority.idle;
                    this.publicTimer = 0;
                }
                else   //人物移动
                    this.transform.Translate(direction.normalized * Time.deltaTime * moveSpeed);
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
                //延续移动状态
                if (this.publicTimer <= dodgeAheadTime)//前摇时间
                {
                    this.canInterrupt = true;//do nothing
                }
                else if (this.publicTimer <= dodgeAheadTime + dodgeTime)//动作进行时间
                {
                    this.canInterrupt = false;
                    this.direction = pos - this.transform.position;
                    this.transform.Translate(direction.normalized * Time.deltaTime * dodgeSpeed);
                }
                else if(this.publicTimer <= dodgeAheadTime + dodgeTime + dodgeEndTime)//后摇时间
                {
                    this.canInterrupt = false;//不可打断闪避后摇
                }
                else//动作结束，回归闲置状态
                {
                    this.nowAction = (int)Priority.idle;
                    this.publicTimer = 0;
                }
                break;
        }

    }

    //计划是在Animation结束之后调用这个函数作为event，自动恢复idle的状态
    void BackToIdle()
    {
        this.nowAction = (int)Priority.idle;
        this.canInterrupt = true;
    }

}
