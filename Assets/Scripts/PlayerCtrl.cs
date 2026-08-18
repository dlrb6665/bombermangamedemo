using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public int HP = 0;
    public int range = 0;
    public int bombCount = 1;

    private float speed = 0.1f;
    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;
    private Color color;

    /// <summary>
    /// 标记人物是否受伤
    /// </summary>
    private bool isInjured = false;
    private float bomBoomTime = 0;

    /// <summary>
    /// 存放当前场景中存在的炸弹
    /// </summary>
    private List<GameObject> bombList = new List<GameObject>();

    protected Joystick joystick;
    protected JoyButton joyButton;
    protected bool isDropping;

    public void ResetPlayer()
    {
        foreach (var item in bombList)
        {
            bombCount++;
            item.GetComponent<Bomb>().StopAllCoroutines();//关闭携程
            ObjectPool.Instance.Add(ObjectType.Bomb, item);
        }
        bombList.Clear();

        StopCoroutine("Injured");
        color.a = 1;
        spriteRenderer.color = color;
    }

    /// <summary>
    /// 增加速度
    /// </summary>
    /// <param name="value"></param>
    public void AddSpeed(float value = 0.03f)
    {
        speed += value;
        if (speed > 0.2f) speed = 0.2f;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        joystick = FindObjectOfType<Joystick>();
        joyButton = FindObjectOfType<JoyButton>();
    }

    /// <summary>
    /// 初始化方法
    /// </summary>
    /// <param name="range"></param>
    /// <param name="HP"></param>
    /// <param name="boomTime"></param>
    public void Init(int range,int HP,float boomTime)
    {
        this.range = range;
        this.HP = HP;
        this.bomBoomTime = boomTime;
    }

    private void Update()
    {
        Move();
        CreateBomb();
        //安卓端
        /*if (!isDropping && joyButton.Pressed)
        {
            isDropping = true;
            CreateBomb();
            //Debug.Log(0);
        }
        if (isDropping && !joyButton.Pressed)
        {
            isDropping = false;
            //Debug.Log(1);
        }*/
    }

    /// <summary>
    /// 移动方法
    /// </summary>
    private void Move()
    {
        //PC端
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);
        rig.MovePosition(transform.position + new Vector3(h, v) * speed);//玩家移动

        //安卓端
        /*anim.SetFloat("Horizontal", joystick.Horizontal);
        anim.SetFloat("Vertical", joystick.Vertical);
        rig.MovePosition(transform.position + new Vector3(joystick.Horizontal, joystick.Vertical) * speed);*/
    }
    private void CreateBomb()
    {
        //安卓端 Input.GetMouseButtonDown(0)
        if (Input.GetKeyDown(KeyCode.Space) && bombCount > 0)
        {
            AudioController.Instance.PlayFire();

            bombCount--;
            GameObject bomb = ObjectPool.Instance.Get(ObjectType.Bomb, 
                new Vector3(Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.y)));
            //                                                Lambda表达式
            //() => { bombCount++;bombList.Remove(bomb);} 等于  private void AniFinFun() { bombCount++;bombList.Remove(bomb);}
            bomb.GetComponent<Bomb>().Init(range, bomBoomTime, () => { 
                bombCount++;
                bombList.Remove(bomb);
            });

            bombList.Add(bomb);
        }
    }
    /// <summary>
    /// 播放结束
    /// </summary>
    public void PlayDieAni()
    {
        Time.timeScale = 0.0f;//动画忽略影响，把Update Mode改为Unscaled Time 即 更新模式 改为 未缩放的时间
        anim.SetTrigger("Die");
    }
    /// <summary>
    /// 结束动画播放完毕
    /// </summary>
    private void DieAniFin()
    {
        Destroy(gameObject);
        GameController.Instance.Gameover();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInjured) return;//受伤、无敌模式
        if (collision.CompareTag(Tags.Enemy) ||collision.CompareTag(Tags.BombEffect))
        {
            //游戏结束
            if (HP <= 0)
            {
                PlayDieAni();
                return;
            }
            HP--;
            StartCoroutine("Injured", 2f);
        }
    }
    IEnumerator Injured(float time)
    { 
        isInjured = true;
        for (int i = 0; i < time * 2; i++)
        {
            color.a = 0;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.25f);
            color.a = 1;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.25f);
        }
        isInjured = false;
    }


}
