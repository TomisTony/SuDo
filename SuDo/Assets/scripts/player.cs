using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //状态显示的public变量
    public int nowAction;//记录目前正在行动的动作（按照优先级设定）
    public bool canInterrupt;//当前动作是否可被更高优先级打断
    public bool isDo;//表示是否正处于渡的状态
    public int faceDirection ;//表示人物的朝向
    public int tapState;//记录处于轻击的第几段，非轻击状态下为0
    public int suState;//记录当时是溯的哪一种

    //人物具体数据的public变量
    //速度类（秒为单位）
    public float moveSpeed;//人物move的速度
    public float moveSpeedInDo;//渡期间的人物移动速度
    public float dodgeSpeed;//人物闪避的速度

    //前摇类（秒为单位）
    public float tapOneAheadTime;//轻击第一段前摇
    public float tapTwoAheadTime;//轻击第二段前摇
    public float tapThreeAheadTime;//轻击第三段前摇
    public float bashAheadTime;//重击前摇
    public float doAheadTime;//渡的前摇
    public float dodgeAheadTime;//闪避前摇
    public float suOneAheadTime;//溯Q的前摇
    public float suTwoAheadTime;//溯W的前摇
    public float suThreeAheadTime;//溯E的前摇

    //持续时间类（秒为单位）
    public float tapOneTime;//轻击第一段持续时间
    public float tapTwoTime;//轻击第二段持续时间
    public float tapThreeTime;//轻击第三段持续时间
    public float bashTime;//重击持续时间
    public float doTime;//渡的持续时间
    public float shadowExitTime;//影子的存在时间
    public float dodgeTime;//闪避持续时间
    public float hurtTime;//受伤的持续僵直时间
    public float suOneTime;//溯Q的持续时间
    public float suTwoTime;//溯W的持续时间
    public float suThreeTime;//溯E的持续时间

    //后摇类（秒为单位）
    public float tapOneEndTime;//轻击第一段后摇
    public float tapTwoEndTime;//轻击第二段后摇
    public float tapThreeEndTime;//轻击第三段后摇
    public float bashEndTime;//重击后摇
    public float doEndTime;//渡的后摇
    public float dodgeEndTime;//闪避后摇
    public float suOneEndTime;//溯Q的后摇
    public float suTwoEndTime;//溯W的后摇
    public float suThreeEndTime;//溯E的后摇

    //储存prefeb的public变量
    public GameObject shadow;//残影

    //animation
    public Animator ani;//animator控制器

    //特效
    //特效的prefeb
    public GameObject tapOneSpecialEffect;//轻击第一段的特效
    public GameObject tapTwoSpecialEffect;//轻击第二段的特效
    public GameObject tapThreeSpecialEffect;//轻击第三段的特效
    public GameObject bashSpecialEffect;//重击的特效
    public GameObject suOneSpecialEffect;//溯Q的特效
    public GameObject suTwoSpecialEffect;//溯W的特效
    public GameObject suThreeSpecialEffect;//溯E的特效

    //距离修正
    public float tapOneDistanceFix;//轻击第一段的距离修正
    public float tapTwoDistanceFix;//轻击第二段的距离修正
    public float tapThreeDistanceFix;//轻击第三段的距离修正
    public float bashDistanceFix;//重击的距离修正
    public float suOneDistanceFix;//溯Q的距离修正
    public float suTwoDistanceFix;//溯W的距离修正
    public float suThreeDistanceFix;//溯E的距离修正

    //角度修正
    public float tapOneRotationFix;//轻击第一段的角度修正
    public float tapTwoRotationFix;//轻击第二段的角度修正
    public float tapThreeRotationFix;//轻击第三段的角度修正
    public float bashRotationFix;//重击的角度修正
    public float suOneRotationFix;//溯Q的角度修正
    public float suTwoRotationFix;//溯W的角度修正
    public float suThreeRotationFix;//溯E的角度修正


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
        hurt,//受伤
        dodge//闪避
    }

    enum Direction 
            //方向
    { 
        up=1,
        right=2,
        down=3,
        left=4
    }


    // Start is called before the first frame update
    void Start()
    {
        this.nowAction = (int)Priority.idle;
        this.canInterrupt = true;
        this.publicTimer = 0;
        this.isDo = false;
        this.isShadowSave = false;
        this.faceDirection = 1;
        this.tapState = 0;
        this.suState = 0;
        
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
            ChangeDirection();//改变目前的direction
            this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标指向的位置
            this.pos.z = this.transform.position.z;//将鼠标指向位置的z坐标设置为0
            this.nowAction = (int)Priority.move;
            this.publicTimer = 0;
            this.canInterrupt = true;
            this.ani.SetInteger("state", (int)Priority.move);
            this.ani.SetInteger("direction", faceDirection);
            return;
        }

        //如果有鼠标右键点击
        if (Input.GetMouseButtonDown(1) && this.canInterrupt && this.nowAction <= (int)Priority.dodge)
        {
            //处理函数
            ChangeDirection();
            this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标指向的位置
            this.pos.z = this.transform.position.z;//将鼠标指向位置的z坐标设置为0
            this.direction = pos - this.transform.position;
            this.nowAction = (int)Priority.dodge;
            this.publicTimer = 0;
            this.canInterrupt = true;//这里写true是为了统一写法，理论上应该是false，但是false可以直接在
                                     //DoTheWork函数里面写
            this.ani.SetInteger("state", (int)Priority.dodge);
            this.ani.SetInteger("direction", faceDirection);
            return;
        }
    }

    //检测键盘是否有操作,并对此做出响应
    void KeyboardPressListen()
    {
        //所有的priority判断不使用“<=”是因为如果输入相同的攻击方式，那一定是由
        //轻击
        if (Input.GetKeyDown(KeyCode.A) && (this.canInterrupt || this.nowAction == (int)Priority.tap) && this.nowAction <= (int)Priority.tap && !this.isDo)
        {
            
            if (this.nowAction != (int)Priority.tap)
                //interrupt形式的切换状态
            {
                ChangeDirection();
                this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标指向的位置
                this.pos.z = this.transform.position.z;//将鼠标指向位置的z坐标设置为0
                this.direction = pos - this.transform.position;
                this.nowAction = (int)Priority.tap;
                this.canInterrupt = true;
                this.tapState = 1;
                this.publicTimer = 0;
                //开启轻击动画
                this.ani.SetInteger("direction", this.faceDirection);
                this.ani.SetInteger("state", (int)Priority.tap);
                this.ani.SetInteger("tap_state", this.tapState);
                //创建轻击实例
                SpecialEffectsHandle(tapOneSpecialEffect,tapOneDistanceFix,tapOneRotationFix);
            }      
            else if((publicTimer > tapOneAheadTime + tapOneTime && this.tapState == 1) ||
                (publicTimer > tapTwoAheadTime + tapTwoTime && this.tapState == 2))
                //第一段和第二段轻击的后摇期间可以打断进入下一段轻击
            {
                ChangeDirection();
                this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标指向的位置
                this.pos.z = this.transform.position.z;//将鼠标指向位置的z坐标设置为0
                this.direction = pos - this.transform.position;
                this.tapState++;//进入下一个轻击阶段
                this.publicTimer = 0;
                this.nowAction = (int)Priority.tap;
                this.canInterrupt = true;
                //开启轻击动画
                this.ani.SetInteger("direction", this.faceDirection);
                this.ani.SetInteger("state", (int)Priority.tap);
                this.ani.SetInteger("tap_state", this.tapState);
                //创建轻击实例
                if(this.tapState == 2)
                    SpecialEffectsHandle(tapTwoSpecialEffect, tapTwoDistanceFix, tapTwoRotationFix);
                else if(this.tapState == 3)
                    SpecialEffectsHandle(tapThreeSpecialEffect, tapThreeDistanceFix, tapThreeRotationFix);
            }
                

        }
        //重击
        if (Input.GetKeyDown(KeyCode.S) && this.canInterrupt && this.nowAction < (int)Priority.bash && !this.isDo)
        {
            ChangeDirection();
            this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标指向的位置
            this.pos.z = this.transform.position.z;//将鼠标指向位置的z坐标设置为0
            this.direction = pos - this.transform.position;
            this.nowAction = (int)Priority.bash;
            this.canInterrupt = true;
            this.publicTimer = 0;
            //开启轻击动画
            this.ani.SetInteger("direction", this.faceDirection);
            this.ani.SetInteger("state", (int)Priority.bash);
            //创建重击实例
            SpecialEffectsHandle(bashSpecialEffect, bashDistanceFix, bashRotationFix);
        }
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
                BackToIdle();
            }
        }
        //溯_Q
        if (Input.GetKeyDown(KeyCode.Q) && this.canInterrupt && this.nowAction < (int)Priority.Su )
        {
            if (isDo)
                //正在渡期间，捕捉技能
            {

            }
            else
                //释放溯Q
            {
                ChangeDirection();
                this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标指向的位置
                this.pos.z = this.transform.position.z;//将鼠标指向位置的z坐标设置为0
                this.direction = pos - this.transform.position;
                this.nowAction = (int)Priority.Su;
                this.canInterrupt = true;
                this.publicTimer = 0;
                this.suState = 1;
                //开启轻击动画
                this.ani.SetInteger("direction", this.faceDirection);
                this.ani.SetInteger("state", (int)Priority.Su);
                this.ani.SetInteger("su_state",this.suState);
                //创建溯Q实例
                SpecialEffectsHandle(suOneSpecialEffect, suOneDistanceFix, suOneRotationFix);
            }
        }
        //溯_W
        if (Input.GetKeyDown(KeyCode.W) && this.canInterrupt && this.nowAction < (int)Priority.Su)
        {
            if (isDo)
            //正在渡期间，捕捉技能
            {

            }
            else
            //释放溯W
            {
                ChangeDirection();
                this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标指向的位置
                this.pos.z = this.transform.position.z;//将鼠标指向位置的z坐标设置为0
                this.direction = pos - this.transform.position;
                this.nowAction = (int)Priority.Su;
                this.canInterrupt = true;
                this.publicTimer = 0;
                this.suState = 2;
                //开启轻击动画
                this.ani.SetInteger("direction", this.faceDirection);
                this.ani.SetInteger("state", (int)Priority.Su);
                this.ani.SetInteger("su_state", this.suState);
                //创建溯W实例
                SpecialEffectsHandle(suTwoSpecialEffect, suTwoDistanceFix, suTwoRotationFix);
            }
        }
        //溯_E
        if (Input.GetKeyDown(KeyCode.E) && this.canInterrupt && this.nowAction < (int)Priority.Su)
        {
            if (isDo)
            //正在渡期间，捕捉技能
            {

            }
            else
            //释放溯E
            {
                ChangeDirection();
                this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//鼠标指向的位置
                this.pos.z = this.transform.position.z;//将鼠标指向位置的z坐标设置为0
                this.direction = pos - this.transform.position;
                this.nowAction = (int)Priority.Su;
                this.canInterrupt = true;
                this.publicTimer = 0;
                this.suState = 3;
                //开启轻击动画
                this.ani.SetInteger("direction", this.faceDirection);
                this.ani.SetInteger("state", (int)Priority.Su);
                this.ani.SetInteger("su_state", this.suState);
                //创建溯E实例
                SpecialEffectsHandle(suThreeSpecialEffect, suThreeDistanceFix, suThreeRotationFix);
            }
        }
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
                    BackToIdle();
                }
                else   //人物移动
                    this.transform.Translate(direction.normalized * Time.deltaTime * moveSpeed);
                break;
            case (int)Priority.tap:
                //延续轻击状态
                switch (this.tapState) {
                    case 1:
                        //第一段轻击
                        if (this.publicTimer <= tapOneAheadTime)//前摇时间
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= tapOneAheadTime + tapOneTime)//动作进行时间
                        {
                            this.canInterrupt = false;
                            
                        }
                        else if (this.publicTimer <= tapOneAheadTime + tapOneTime + tapOneEndTime)//后摇时间
                        {
                            this.canInterrupt = false;//不可打断闪避后摇
                        }
                        else//动作结束，回归闲置状态
                        {
                            BackToIdle();
                        }
                        break;

                    case 2:
                        //第二段轻击
                        if (this.publicTimer <= tapTwoAheadTime)//前摇时间
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= tapTwoAheadTime + tapTwoTime)//动作进行时间
                        {
                            this.canInterrupt = false;

                        }
                        else if (this.publicTimer <= tapTwoAheadTime + tapTwoTime + tapTwoEndTime)//后摇时间
                        {
                            this.canInterrupt = false;//不可打断闪避后摇
                        }
                        else//动作结束，回归闲置状态
                        {
                            BackToIdle();
                        }
                        break;
                        
                    case 3:
                        //第三段轻击
                        if (this.publicTimer <= tapThreeAheadTime)//前摇时间
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= tapThreeAheadTime + tapThreeTime)//动作进行时间
                        {
                            this.canInterrupt = false;

                        }
                        else if (this.publicTimer <= tapThreeAheadTime + tapThreeTime + tapThreeEndTime)//后摇时间
                        {
                            this.canInterrupt = false;//不可打断闪避后摇
                        }
                        else//动作结束，回归闲置状态
                        {
                            BackToIdle();
                        }
                        break;
                }
                break;
            case (int)Priority.bash:
                //延续重击状态
                if (this.publicTimer <= bashAheadTime)//前摇时间
                {
                    this.canInterrupt = true;//do nothing
                }
                else if (this.publicTimer <= bashAheadTime + bashTime)//动作进行时间
                {
                    this.canInterrupt = false;

                }
                else if (this.publicTimer <= bashAheadTime + bashTime + bashEndTime)//后摇时间
                {
                    this.canInterrupt = false;//不可打断闪避后摇
                }
                else//动作结束，回归闲置状态
                {
                    BackToIdle();
                }
                break;
            case (int)Priority.Su:
                //延续溯状态
                switch (suState) {
                    case 1:
                        //溯Q
                        if (this.publicTimer <= suOneAheadTime)//前摇时间
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= suOneAheadTime + suOneTime)//动作进行时间
                        {
                            this.canInterrupt = false;

                        }
                        else if (this.publicTimer <= suOneAheadTime + suOneTime + suOneEndTime)//后摇时间
                        {
                            this.canInterrupt = false;//不可打断闪避后摇
                        }
                        else//动作结束，回归闲置状态
                        {
                            BackToIdle();
                        }
                        break;
                    case 2:
                        //溯W
                        if (this.publicTimer <= suTwoAheadTime)//前摇时间
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= suTwoAheadTime + suTwoTime)//动作进行时间
                        {
                            this.canInterrupt = false;

                        }
                        else if (this.publicTimer <= suTwoAheadTime + suTwoTime + suTwoEndTime)//后摇时间
                        {
                            this.canInterrupt = false;//不可打断闪避后摇
                        }
                        else//动作结束，回归闲置状态
                        {
                            BackToIdle();
                        }
                        break;
                    case 3:
                        //溯E
                        if (this.publicTimer <= suThreeAheadTime)//前摇时间
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= suThreeAheadTime + suThreeTime)//动作进行时间
                        {
                            this.canInterrupt = false;

                        }
                        else if (this.publicTimer <= suThreeAheadTime + suThreeTime + suThreeEndTime)//后摇时间
                        {
                            this.canInterrupt = false;//不可打断闪避后摇
                        }
                        else//动作结束，回归闲置状态
                        {
                            BackToIdle();
                        }
                        break;
                }
                break;
            case (int)Priority.do_do:
                //延续渡状态,不需要延续XD
                break;
            case (int)Priority.hurt:
                //延续受伤僵直状态
                if (this.publicTimer < this.hurtTime)
                    this.canInterrupt = false;
                else
                    BackToIdle();
                break;
            case (int)Priority.dodge:
                //延续闪避状态
                if (this.publicTimer <= dodgeAheadTime)//前摇时间
                {
                    this.canInterrupt = true;//do nothing
                }
                else if (this.publicTimer <= dodgeAheadTime + dodgeTime)//动作进行时间
                {
                    this.canInterrupt = false;
                    this.transform.Translate(direction.normalized * Time.deltaTime * dodgeSpeed);
                }
                else if(this.publicTimer <= dodgeAheadTime + dodgeTime + dodgeEndTime)//后摇时间
                {
                    this.canInterrupt = false;//不可打断闪避后摇
                }
                else//动作结束，回归闲置状态
                {
                    BackToIdle();
                }
                break;
        }

    }

    //计划是在Animation结束之后调用这个函数作为event，自动恢复idle的状态
    void BackToIdle()
    {
        this.nowAction = (int)Priority.idle;
        this.canInterrupt = true;
        this.publicTimer = 0;
        this.tapState = 0;
        this.suState = 0;
        this.ani.SetInteger("state", (int)Priority.idle);
        this.ani.SetInteger("direction", faceDirection);//播放对应的待机动画

    }

    //判断是否进入受伤僵直
    void Hurt()
    {
        if(this.nowAction < (int)Priority.hurt && this.canInterrupt)
        {
            this.nowAction = (int)Priority.hurt;
            this.canInterrupt = true;
            this.publicTimer = 0;
            this.ani.SetInteger("state", (int)Priority.hurt);
            this.ani.SetInteger("direction", faceDirection);//播放对应的受击动画
        }
    }

    //改变direction
    void ChangeDirection()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        if (mouse.y > mouse.x && mouse.y > -1 * mouse.x)
            this.faceDirection = (int)Direction.up;
        else if (mouse.y < mouse.x && mouse.y > -1 * mouse.x)
            this.faceDirection = (int)Direction.right;
        else if (mouse.y < mouse.x && mouse.y < -1 * mouse.x)
            this.faceDirection = (int)Direction.down;
        else if (mouse.y > mouse.x && mouse.y < -1 * mouse.x)
            this.faceDirection = (int)Direction.left;
    }

    void SpecialEffectsHandle(GameObject effectObject, float distanceFix, float rotationFix)
    {
        GameObject effect = Instantiate(effectObject);//实例化
        effect.transform.position = this.transform.position + direction.normalized * distanceFix;
        //注意该处的角度变换，x与y是没有变换的。x的变换程度是目前鼠标指向的位置向量与vector3.up之间的
        //较小角度（<180°）加上rotationFix
        effect.transform.rotation = Quaternion.Euler(0,0,
                                    Vector3.Angle(Vector3.up,this.direction) + rotationFix);
    }

}
