using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //״̬��ʾ��public����
    public int nowAction;//��¼Ŀǰ�����ж��Ķ������������ȼ��趨��
    public bool canInterrupt;//��ǰ�����Ƿ�ɱ��������ȼ����
    public bool isDo;//��ʾ�Ƿ������ڶɵ�״̬

    //����������ݵ�public����
    //�ٶ��ࣨ��Ϊ��λ��
    public float moveSpeed;//����move���ٶ�
    public float moveSpeedInDo;//���ڼ�������ƶ��ٶ�
    public float dodgeSpeed;//�������ܵ��ٶ�

    //ǰҡ�ࣨ��Ϊ��λ��
    public float tapOneAheadTime;//�����һ��ǰҡ
    public float tapTwoAheadTime;//����ڶ���ǰҡ
    public float tapThreeAheadTime;//���������ǰҡ
    public float bashOneAheadTime;//�ػ���һ��ǰҡ
    public float bashTwoAheadTime;//�ػ��ڶ���ǰҡ
    public float bashThreeAheadTime;//�ػ�������ǰҡ
    public float doAheadTime;//�ɵ�ǰҡ
    public float dodgeAheadTime;//����ǰҡ

    //����ʱ���ࣨ��Ϊ��λ��
    public float tapOneTime;//�����һ�γ���ʱ��
    public float tapTwoTime;//����ڶ��γ���ʱ��
    public float tapThreeTime;//��������γ���ʱ��
    public float bashOneTime;//�ػ���һ�γ���ʱ��
    public float bashTwoTime;//�ػ��ڶ��γ���ʱ��
    public float bashThreeTime;//�ػ������γ���ʱ��
    public float doTime;//�ɵĳ���ʱ��
    public float shadowExitTime;//Ӱ�ӵĴ���ʱ��
    public float dodgeTime;//���ܳ���ʱ��

    //��ҡ�ࣨ��Ϊ��λ��
    public float tapOneEndTime;//�����һ�κ�ҡ
    public float tapTwoEndTime;//����ڶ��κ�ҡ
    public float tapThreeEndTime;//��������κ�ҡ
    public float bashOneEndTime;//�ػ���һ�κ�ҡ
    public float bashTwoEndTime;//�ػ��ڶ��κ�ҡ
    public float bashThreeEndTime;//�ػ������κ�ҡ
    public float doEndTime;//�ɵĺ�ҡ
    public float dodgeEndTime;//���ܺ�ҡ

    //����prefeb��public����
    public GameObject shadow;//��Ӱ


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
        dodge//����
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
            this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ָ���λ��
            this.pos.z = this.transform.position.z;//�����ָ��λ�õ�z��������Ϊ0
            this.nowAction = (int)Priority.move;
            this.publicTimer = 0;
            this.canInterrupt = true;
            return;
        }

        //���������Ҽ����
        if (Input.GetMouseButtonDown(1) && this.canInterrupt && this.nowAction <= (int)Priority.dodge)
        {
            //������
            this.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ָ���λ��
            this.pos.z = this.transform.position.z;//�����ָ��λ�õ�z��������Ϊ0
            this.nowAction = (int)Priority.dodge;
            this.publicTimer = 0;
            this.canInterrupt = true;//����дtrue��Ϊ��ͳһд����������Ӧ����false������false����ֱ����
                                       //DoTheWork��������д
            return;
        }
    }

    //�������Ƿ��в���,���Դ�������Ӧ
    void KeyboardPressListen()
    {
        //���е�priority�жϲ�ʹ�á�<=������Ϊ���������ͬ�Ĺ�����ʽ����һ������
        //���
        if (Input.GetKeyDown(KeyCode.A) && this.canInterrupt && this.nowAction < (int)Priority.tap && !this.isDo)
        {}
        //�ػ�
        if (Input.GetKeyDown(KeyCode.S) && this.canInterrupt && this.nowAction < (int)Priority.bash && !this.isDo)
        {}
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
                this.nowAction = (int)Priority.idle; //�л�����״̬
                this.canInterrupt = true;
            }
        }
        //��_Q
        if (Input.GetKeyDown(KeyCode.Q) && this.canInterrupt && this.nowAction < (int)Priority.Su)
        {}
        //��_W
        if (Input.GetKeyDown(KeyCode.W) && this.canInterrupt && this.nowAction < (int)Priority.Su)
        {}
        //��_E
        if (Input.GetKeyDown(KeyCode.E) && this.canInterrupt && this.nowAction < (int)Priority.Su)
        {}
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
                    this.nowAction = (int)Priority.idle;
                    this.publicTimer = 0;
                }
                else   //�����ƶ�
                    this.transform.Translate(direction.normalized * Time.deltaTime * moveSpeed);
                break;
            case (int)Priority.tap:
                //�������״̬
                break;
            case (int)Priority.bash:
                //�����ػ�״̬
                break;
            case (int)Priority.Su:
                //������״̬
                break;
            case (int)Priority.do_do:
                //������״̬
                break;
            case (int)Priority.dodge:
                //��������״̬
                //�����ƶ�״̬
                if (this.publicTimer <= dodgeAheadTime)//ǰҡʱ��
                {
                    this.canInterrupt = true;//do nothing
                }
                else if (this.publicTimer <= dodgeAheadTime + dodgeTime)//��������ʱ��
                {
                    this.canInterrupt = false;
                    this.direction = pos - this.transform.position;
                    this.transform.Translate(direction.normalized * Time.deltaTime * dodgeSpeed);
                }
                else if(this.publicTimer <= dodgeAheadTime + dodgeTime + dodgeEndTime)//��ҡʱ��
                {
                    this.canInterrupt = false;//���ɴ�����ܺ�ҡ
                }
                else//�����������ع�����״̬
                {
                    this.nowAction = (int)Priority.idle;
                    this.publicTimer = 0;
                }
                break;
        }

    }

    //�ƻ�����Animation����֮��������������Ϊevent���Զ��ָ�idle��״̬
    void BackToIdle()
    {
        this.nowAction = (int)Priority.idle;
        this.canInterrupt = true;
    }

}
