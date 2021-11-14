using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //״̬��ʾ��public����
    public int nowAction;//��¼Ŀǰ�����ж��Ķ������������ȼ��趨��
    public bool canInterrupt;//��ǰ�����Ƿ�ɱ��������ȼ����

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

    //����������
    public float playerHP;//�����Ѫ��


    //private����
    private float publicTimer;//ͨ�õļ�ʱ�������ڵ�ǰ�ж�����ʱ��ļ�ʱ��

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
    }

    // Update is called once per frame
    void Update()
    {
        /*����˼������ÿһ֡��ɫ����Ĭ��ִ�е�ǰ״̬�����������ǻ���ǰ����������
        ����Ȼ���ж���������ͽ�ɫĿǰ�������ȼ��Լ���ɫ�Ƿ��ڿɴ�ϵ�״̬,�����
        �Դ�ϣ��Ǿ�ֱ�Ӹı��ɫ��Ҫȥ�������飬�����˼·���ǽ����LC-3��Interrupt��˼��*/

        publicTimer += Time.deltaTime;//����ʱ����ʱ

        ButtonClickListen();//�������Ƿ��в���
        KeyboardPressListen();//�������Ƿ��в���

        //ִ�н�ɫĿǰ״̬������״̬
        DoTheWork();
    }

    //�������Ƿ��в���,���Դ�������Ӧ
    void ButtonClickListen()
    {
        //��������������
        if (Input.GetMouseButtonDown(0) && this.canInterrupt && this.nowAction < (int)Priority.move)
        {
            //������
            return;
        }

        //���������Ҽ����
        if (Input.GetMouseButtonDown(1) && this.canInterrupt && this.nowAction < (int)Priority.dodge)
        {
            //������
            return;
        }
    }

    //�������Ƿ��в���,���Դ�������Ӧ
    void KeyboardPressListen()
    {
        //���
        if (Input.GetKeyDown(KeyCode.A) && this.canInterrupt && this.nowAction < (int)Priority.tap)
        {}
        //�ػ�
        if (Input.GetKeyDown(KeyCode.S) && this.canInterrupt && this.nowAction < (int)Priority.bash)
        {}
        //�����
        if (Input.GetKeyDown(KeyCode.Space) && this.canInterrupt && this.nowAction < (int)Priority.do_do)
        {}
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
                break;
            case (int)Priority.move:
                //�����ƶ�״̬
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
                break;
        }

    }

    //�ƻ�����Animation����֮��������������Ϊevent���Զ��ָ�idle��״̬
    void BackToIdle()
    {
        this.nowAction = (int)Priority.idle;
        this.canInterrupt = true;
    }

    //���������ṩѪ���仯�Ľӿ�
    public void changeHp(float HPchange)
    {
        this.playerHP += HPchange;
    }
}
