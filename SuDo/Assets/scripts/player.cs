using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //״̬��ʾ��public����
    public int nowAction;//��¼Ŀǰ�����ж��Ķ������������ȼ��趨��
    public bool canInterrupt;//��ǰ�����Ƿ�ɱ��������ȼ����
    public bool isDo;//��ʾ�Ƿ������ڶɵ�״̬
    public int faceDirection ;//��ʾ����ĳ���
    public int tapState;//��¼��������ĵڼ��Σ������״̬��Ϊ0
    public int suState;//��¼��ʱ���ݵ���һ��

    //����������ݵ�public����
    //�ٶ��ࣨ��Ϊ��λ��
    public float moveSpeed;//����move���ٶ�
    public float moveSpeedInDo;//���ڼ�������ƶ��ٶ�
    public float dodgeSpeed;//�������ܵ��ٶ�

    //ǰҡ�ࣨ��Ϊ��λ��
    public float tapOneAheadTime;//�����һ��ǰҡ
    public float tapTwoAheadTime;//����ڶ���ǰҡ
    public float tapThreeAheadTime;//���������ǰҡ
    public float bashAheadTime;//�ػ�ǰҡ
    public float doAheadTime;//�ɵ�ǰҡ
    public float dodgeAheadTime;//����ǰҡ
    public float suOneAheadTime;//��Q��ǰҡ
    public float suTwoAheadTime;//��W��ǰҡ
    public float suThreeAheadTime;//��E��ǰҡ

    //����ʱ���ࣨ��Ϊ��λ��
    public float tapOneTime;//�����һ�γ���ʱ��
    public float tapTwoTime;//����ڶ��γ���ʱ��
    public float tapThreeTime;//��������γ���ʱ��
    public float bashTime;//�ػ�����ʱ��
    public float doTime;//�ɵĳ���ʱ��
    public float shadowExitTime;//Ӱ�ӵĴ���ʱ��
    public float dodgeTime;//���ܳ���ʱ��
    public float hurtTime;//���˵ĳ�����ֱʱ��
    public float suOneTime;//��Q�ĳ���ʱ��
    public float suTwoTime;//��W�ĳ���ʱ��
    public float suThreeTime;//��E�ĳ���ʱ��

    //��ҡ�ࣨ��Ϊ��λ��
    public float tapOneEndTime;//�����һ�κ�ҡ
    public float tapTwoEndTime;//����ڶ��κ�ҡ
    public float tapThreeEndTime;//��������κ�ҡ
    public float bashEndTime;//�ػ���ҡ
    public float doEndTime;//�ɵĺ�ҡ
    public float dodgeEndTime;//���ܺ�ҡ
    public float suOneEndTime;//��Q�ĺ�ҡ
    public float suTwoEndTime;//��W�ĺ�ҡ
    public float suThreeEndTime;//��E�ĺ�ҡ

    //����prefeb��public����
    public GameObject shadow;//��Ӱ

    //animation
    public Animator ani;//animator������

    //��Ч
    //��Ч��prefeb
    public GameObject tapOneSpecialEffect;//�����һ�ε���Ч
    public GameObject tapTwoSpecialEffect;//����ڶ��ε���Ч
    public GameObject tapThreeSpecialEffect;//��������ε���Ч
    public GameObject bashSpecialEffect;//�ػ�����Ч
    public GameObject suOneSpecialEffect;//��Q����Ч
    public GameObject suTwoSpecialEffect;//��W����Ч
    public GameObject suThreeSpecialEffect;//��E����Ч

    //��������
    public float tapOneDistanceFix;//�����һ�εľ�������
    public float tapTwoDistanceFix;//����ڶ��εľ�������
    public float tapThreeDistanceFix;//��������εľ�������
    public float bashDistanceFix;//�ػ��ľ�������
    public float suOneDistanceFix;//��Q�ľ�������
    public float suTwoDistanceFix;//��W�ľ�������
    public float suThreeDistanceFix;//��E�ľ�������

    //�Ƕ�����
    public float tapOneRotationFix;//�����һ�εĽǶ�����
    public float tapTwoRotationFix;//����ڶ��εĽǶ�����
    public float tapThreeRotationFix;//��������εĽǶ�����
    public float bashRotationFix;//�ػ��ĽǶ�����
    public float suOneRotationFix;//��Q�ĽǶ�����
    public float suTwoRotationFix;//��W�ĽǶ�����
    public float suThreeRotationFix;//��E�ĽǶ�����


    //private����
    private float publicTimer;//ͨ�õļ�ʱ�������ڵ�ǰ�ж�����ʱ��ļ�ʱ��
    private float shadowTimer;//Ӱ�ӵĶ�����ڼ�ʱ��
    private Vector3 pos;//��¼��һ�����ĳɹ�����ĵ��λ�ã����λ�ã�
    private Vector3 direction;//ͨ�õ�����λ��ʸ��
    private GameObject shadowInGame;//shadow��ʵ����
    private bool isShadowSave;//Ӱ���Ƿ񻹴���

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

    enum Direction 
            //����
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
        /*����˼������ÿһ֡��ɫ����Ĭ��ִ�е�ǰ״̬�����������ǻ���ǰ����������
        ����Ȼ���ж���������ͽ�ɫĿǰ�������ȼ��Լ���ɫ�Ƿ��ڿɴ�ϵ�״̬,�����
        �Դ�ϣ��Ǿ�ֱ�Ӹı��ɫ��Ҫȥ�������飬�����˼·���ǽ����LC-3��Interrupt��˼��*/

        TimerDeal();        //��ʱ������ģ��
        ButtonClickListen();//�������Ƿ��в���
        KeyboardPressListen();//�������Ƿ��в���

        //ִ�н�ɫĿǰ״̬������״̬
        DoTheWork();
    }

    void TimerDeal()
    {
        publicTimer += Time.deltaTime;//����ʱ����ʱ
        if (this.isShadowSave && !this.isDo) this.shadowTimer += Time.deltaTime;//�ɽ���֮��Ӱ�ӵĴ�ʼ��ʱ
        if(this.shadowTimer >= this.shadowExitTime)//shadow���ڵ�ʱ�䵽��
        {
            this.isShadowSave = false;
            this.shadowTimer = 0;
            Destroy(shadowInGame);
        }
    }

    //�������Ƿ��в���,���Դ�������Ӧ
    void ButtonClickListen()
    {
        //��������������
        if (Input.GetMouseButtonDown(0) && this.canInterrupt && this.nowAction <= (int)Priority.move)
        {
            //������
            ChangeDirection();//�ı�Ŀǰ��direction
            this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ָ���λ��
            this.pos.z = this.transform.position.z;//�����ָ��λ�õ�z��������Ϊ0
            this.nowAction = (int)Priority.move;
            this.publicTimer = 0;
            this.canInterrupt = true;
            this.ani.SetInteger("state", (int)Priority.move);
            this.ani.SetInteger("direction", faceDirection);
            return;
        }

        //���������Ҽ����
        if (Input.GetMouseButtonDown(1) && this.canInterrupt && this.nowAction <= (int)Priority.dodge)
        {
            //������
            ChangeDirection();
            this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ָ���λ��
            this.pos.z = this.transform.position.z;//�����ָ��λ�õ�z��������Ϊ0
            this.direction = pos - this.transform.position;
            this.nowAction = (int)Priority.dodge;
            this.publicTimer = 0;
            this.canInterrupt = true;//����дtrue��Ϊ��ͳһд����������Ӧ����false������false����ֱ����
                                     //DoTheWork��������д
            this.ani.SetInteger("state", (int)Priority.dodge);
            this.ani.SetInteger("direction", faceDirection);
            return;
        }
    }

    //�������Ƿ��в���,���Դ�������Ӧ
    void KeyboardPressListen()
    {
        //���е�priority�жϲ�ʹ�á�<=������Ϊ���������ͬ�Ĺ�����ʽ����һ������
        //���
        if (Input.GetKeyDown(KeyCode.A) && (this.canInterrupt || this.nowAction == (int)Priority.tap) && this.nowAction <= (int)Priority.tap && !this.isDo)
        {
            
            if (this.nowAction != (int)Priority.tap)
                //interrupt��ʽ���л�״̬
            {
                ChangeDirection();
                this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ָ���λ��
                this.pos.z = this.transform.position.z;//�����ָ��λ�õ�z��������Ϊ0
                this.direction = pos - this.transform.position;
                this.nowAction = (int)Priority.tap;
                this.canInterrupt = true;
                this.tapState = 1;
                this.publicTimer = 0;
                //�����������
                this.ani.SetInteger("direction", this.faceDirection);
                this.ani.SetInteger("state", (int)Priority.tap);
                this.ani.SetInteger("tap_state", this.tapState);
                //�������ʵ��
                SpecialEffectsHandle(tapOneSpecialEffect,tapOneDistanceFix,tapOneRotationFix);
            }      
            else if((publicTimer > tapOneAheadTime + tapOneTime && this.tapState == 1) ||
                (publicTimer > tapTwoAheadTime + tapTwoTime && this.tapState == 2))
                //��һ�κ͵ڶ�������ĺ�ҡ�ڼ���Դ�Ͻ�����һ�����
            {
                ChangeDirection();
                this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ָ���λ��
                this.pos.z = this.transform.position.z;//�����ָ��λ�õ�z��������Ϊ0
                this.direction = pos - this.transform.position;
                this.tapState++;//������һ������׶�
                this.publicTimer = 0;
                this.nowAction = (int)Priority.tap;
                this.canInterrupt = true;
                //�����������
                this.ani.SetInteger("direction", this.faceDirection);
                this.ani.SetInteger("state", (int)Priority.tap);
                this.ani.SetInteger("tap_state", this.tapState);
                //�������ʵ��
                if(this.tapState == 2)
                    SpecialEffectsHandle(tapTwoSpecialEffect, tapTwoDistanceFix, tapTwoRotationFix);
                else if(this.tapState == 3)
                    SpecialEffectsHandle(tapThreeSpecialEffect, tapThreeDistanceFix, tapThreeRotationFix);
            }
                

        }
        //�ػ�
        if (Input.GetKeyDown(KeyCode.S) && this.canInterrupt && this.nowAction < (int)Priority.bash && !this.isDo)
        {
            ChangeDirection();
            this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ָ���λ��
            this.pos.z = this.transform.position.z;//�����ָ��λ�õ�z��������Ϊ0
            this.direction = pos - this.transform.position;
            this.nowAction = (int)Priority.bash;
            this.canInterrupt = true;
            this.publicTimer = 0;
            //�����������
            this.ani.SetInteger("direction", this.faceDirection);
            this.ani.SetInteger("state", (int)Priority.bash);
            //�����ػ�ʵ��
            SpecialEffectsHandle(bashSpecialEffect, bashDistanceFix, bashRotationFix);
        }
        //�����
        //�ɵĴ���Ƚ����⣬��Ϊ��״̬��ʵ����Ϊһ����������ڵģ�����ֻ�а���˲�����괦��
        if (Input.GetKeyDown(KeyCode.Space) && this.canInterrupt && this.nowAction <= (int)Priority.do_do)
        {
            if(!this.isDo && !isShadowSave)//������
            {
                this.isDo = true;
                this.isShadowSave = true;
                shadowInGame = Instantiate(shadow);
                shadowInGame.transform.position = this.transform.position;//shadowʵ����
            }
            else if(this.isDo)//������
            {
                this.isDo = false;
                this.isShadowSave = true;
                this.shadowTimer = 0;
            }
            else if(!this.isDo && isShadowSave)//����Ӱ����λ��
            {
                Vector3 position = this.transform.position;
                this.transform.position = this.shadowInGame.transform.position;
                this.shadowInGame.transform.position = position;
                BackToIdle();
            }
        }
        //��_Q
        if (Input.GetKeyDown(KeyCode.Q) && this.canInterrupt && this.nowAction < (int)Priority.Su )
        {
            if (isDo)
                //���ڶ��ڼ䣬��׽����
            {

            }
            else
                //�ͷ���Q
            {
                ChangeDirection();
                this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ָ���λ��
                this.pos.z = this.transform.position.z;//�����ָ��λ�õ�z��������Ϊ0
                this.direction = pos - this.transform.position;
                this.nowAction = (int)Priority.Su;
                this.canInterrupt = true;
                this.publicTimer = 0;
                this.suState = 1;
                //�����������
                this.ani.SetInteger("direction", this.faceDirection);
                this.ani.SetInteger("state", (int)Priority.Su);
                this.ani.SetInteger("su_state",this.suState);
                //������Qʵ��
                SpecialEffectsHandle(suOneSpecialEffect, suOneDistanceFix, suOneRotationFix);
            }
        }
        //��_W
        if (Input.GetKeyDown(KeyCode.W) && this.canInterrupt && this.nowAction < (int)Priority.Su)
        {
            if (isDo)
            //���ڶ��ڼ䣬��׽����
            {

            }
            else
            //�ͷ���W
            {
                ChangeDirection();
                this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ָ���λ��
                this.pos.z = this.transform.position.z;//�����ָ��λ�õ�z��������Ϊ0
                this.direction = pos - this.transform.position;
                this.nowAction = (int)Priority.Su;
                this.canInterrupt = true;
                this.publicTimer = 0;
                this.suState = 2;
                //�����������
                this.ani.SetInteger("direction", this.faceDirection);
                this.ani.SetInteger("state", (int)Priority.Su);
                this.ani.SetInteger("su_state", this.suState);
                //������Wʵ��
                SpecialEffectsHandle(suTwoSpecialEffect, suTwoDistanceFix, suTwoRotationFix);
            }
        }
        //��_E
        if (Input.GetKeyDown(KeyCode.E) && this.canInterrupt && this.nowAction < (int)Priority.Su)
        {
            if (isDo)
            //���ڶ��ڼ䣬��׽����
            {

            }
            else
            //�ͷ���E
            {
                ChangeDirection();
                this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ָ���λ��
                this.pos.z = this.transform.position.z;//�����ָ��λ�õ�z��������Ϊ0
                this.direction = pos - this.transform.position;
                this.nowAction = (int)Priority.Su;
                this.canInterrupt = true;
                this.publicTimer = 0;
                this.suState = 3;
                //�����������
                this.ani.SetInteger("direction", this.faceDirection);
                this.ani.SetInteger("state", (int)Priority.Su);
                this.ani.SetInteger("su_state", this.suState);
                //������Eʵ��
                SpecialEffectsHandle(suThreeSpecialEffect, suThreeDistanceFix, suThreeRotationFix);
            }
        }
    }

    //ִ�н�ɫĿǰ״̬������״̬
    void DoTheWork()
    {
        switch (this.nowAction) {
            case (int)Priority.idle:
                //��������״̬
                this.canInterrupt = true;
                break;
            case (int)Priority.move:
                //�����ƶ�״̬
                this.canInterrupt = true;
                this.direction = pos - this.transform.position;
                if (direction.magnitude <= 0.1) //��������ͣ��
                {
                    BackToIdle();
                }
                else   //�����ƶ�
                    this.transform.Translate(direction.normalized * Time.deltaTime * moveSpeed);
                break;
            case (int)Priority.tap:
                //�������״̬
                switch (this.tapState) {
                    case 1:
                        //��һ�����
                        if (this.publicTimer <= tapOneAheadTime)//ǰҡʱ��
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= tapOneAheadTime + tapOneTime)//��������ʱ��
                        {
                            this.canInterrupt = false;
                            
                        }
                        else if (this.publicTimer <= tapOneAheadTime + tapOneTime + tapOneEndTime)//��ҡʱ��
                        {
                            this.canInterrupt = false;//���ɴ�����ܺ�ҡ
                        }
                        else//�����������ع�����״̬
                        {
                            BackToIdle();
                        }
                        break;

                    case 2:
                        //�ڶ������
                        if (this.publicTimer <= tapTwoAheadTime)//ǰҡʱ��
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= tapTwoAheadTime + tapTwoTime)//��������ʱ��
                        {
                            this.canInterrupt = false;

                        }
                        else if (this.publicTimer <= tapTwoAheadTime + tapTwoTime + tapTwoEndTime)//��ҡʱ��
                        {
                            this.canInterrupt = false;//���ɴ�����ܺ�ҡ
                        }
                        else//�����������ع�����״̬
                        {
                            BackToIdle();
                        }
                        break;
                        
                    case 3:
                        //���������
                        if (this.publicTimer <= tapThreeAheadTime)//ǰҡʱ��
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= tapThreeAheadTime + tapThreeTime)//��������ʱ��
                        {
                            this.canInterrupt = false;

                        }
                        else if (this.publicTimer <= tapThreeAheadTime + tapThreeTime + tapThreeEndTime)//��ҡʱ��
                        {
                            this.canInterrupt = false;//���ɴ�����ܺ�ҡ
                        }
                        else//�����������ع�����״̬
                        {
                            BackToIdle();
                        }
                        break;
                }
                break;
            case (int)Priority.bash:
                //�����ػ�״̬
                if (this.publicTimer <= bashAheadTime)//ǰҡʱ��
                {
                    this.canInterrupt = true;//do nothing
                }
                else if (this.publicTimer <= bashAheadTime + bashTime)//��������ʱ��
                {
                    this.canInterrupt = false;

                }
                else if (this.publicTimer <= bashAheadTime + bashTime + bashEndTime)//��ҡʱ��
                {
                    this.canInterrupt = false;//���ɴ�����ܺ�ҡ
                }
                else//�����������ع�����״̬
                {
                    BackToIdle();
                }
                break;
            case (int)Priority.Su:
                //������״̬
                switch (suState) {
                    case 1:
                        //��Q
                        if (this.publicTimer <= suOneAheadTime)//ǰҡʱ��
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= suOneAheadTime + suOneTime)//��������ʱ��
                        {
                            this.canInterrupt = false;

                        }
                        else if (this.publicTimer <= suOneAheadTime + suOneTime + suOneEndTime)//��ҡʱ��
                        {
                            this.canInterrupt = false;//���ɴ�����ܺ�ҡ
                        }
                        else//�����������ع�����״̬
                        {
                            BackToIdle();
                        }
                        break;
                    case 2:
                        //��W
                        if (this.publicTimer <= suTwoAheadTime)//ǰҡʱ��
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= suTwoAheadTime + suTwoTime)//��������ʱ��
                        {
                            this.canInterrupt = false;

                        }
                        else if (this.publicTimer <= suTwoAheadTime + suTwoTime + suTwoEndTime)//��ҡʱ��
                        {
                            this.canInterrupt = false;//���ɴ�����ܺ�ҡ
                        }
                        else//�����������ع�����״̬
                        {
                            BackToIdle();
                        }
                        break;
                    case 3:
                        //��E
                        if (this.publicTimer <= suThreeAheadTime)//ǰҡʱ��
                        {
                            this.canInterrupt = true;//do nothing
                        }
                        else if (this.publicTimer <= suThreeAheadTime + suThreeTime)//��������ʱ��
                        {
                            this.canInterrupt = false;

                        }
                        else if (this.publicTimer <= suThreeAheadTime + suThreeTime + suThreeEndTime)//��ҡʱ��
                        {
                            this.canInterrupt = false;//���ɴ�����ܺ�ҡ
                        }
                        else//�����������ع�����״̬
                        {
                            BackToIdle();
                        }
                        break;
                }
                break;
            case (int)Priority.do_do:
                //������״̬,����Ҫ����XD
                break;
            case (int)Priority.hurt:
                //�������˽�ֱ״̬
                if (this.publicTimer < this.hurtTime)
                    this.canInterrupt = false;
                else
                    BackToIdle();
                break;
            case (int)Priority.dodge:
                //��������״̬
                if (this.publicTimer <= dodgeAheadTime)//ǰҡʱ��
                {
                    this.canInterrupt = true;//do nothing
                }
                else if (this.publicTimer <= dodgeAheadTime + dodgeTime)//��������ʱ��
                {
                    this.canInterrupt = false;
                    this.transform.Translate(direction.normalized * Time.deltaTime * dodgeSpeed);
                }
                else if(this.publicTimer <= dodgeAheadTime + dodgeTime + dodgeEndTime)//��ҡʱ��
                {
                    this.canInterrupt = false;//���ɴ�����ܺ�ҡ
                }
                else//�����������ع�����״̬
                {
                    BackToIdle();
                }
                break;
        }

    }

    //�ƻ�����Animation����֮��������������Ϊevent���Զ��ָ�idle��״̬
    void BackToIdle()
    {
        this.nowAction = (int)Priority.idle;
        this.canInterrupt = true;
        this.publicTimer = 0;
        this.tapState = 0;
        this.suState = 0;
        this.ani.SetInteger("state", (int)Priority.idle);
        this.ani.SetInteger("direction", faceDirection);//���Ŷ�Ӧ�Ĵ�������

    }

    //�ж��Ƿ�������˽�ֱ
    void Hurt()
    {
        if(this.nowAction < (int)Priority.hurt && this.canInterrupt)
        {
            this.nowAction = (int)Priority.hurt;
            this.canInterrupt = true;
            this.publicTimer = 0;
            this.ani.SetInteger("state", (int)Priority.hurt);
            this.ani.SetInteger("direction", faceDirection);//���Ŷ�Ӧ���ܻ�����
        }
    }

    //�ı�direction
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
        GameObject effect = Instantiate(effectObject);//ʵ����
        effect.transform.position = this.transform.position + direction.normalized * distanceFix;
        //ע��ô��ĽǶȱ任��x��y��û�б任�ġ�x�ı任�̶���Ŀǰ���ָ���λ��������vector3.up֮���
        //��С�Ƕȣ�<180�㣩����rotationFix
        effect.transform.rotation = Quaternion.Euler(0,0,
                                    Vector3.Angle(Vector3.up,this.direction) + rotationFix);
    }

}
