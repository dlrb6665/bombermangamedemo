using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public Text txt_HP, txt_Level, txt_Time, txt_Enemy;
    public GameObject gameoverPanel;
    public Animator LevelFadeAnim;

    private void Awake()
    {
        Instance = this;
        Init();
    }
    private void Init()
    {
        //                      Find查找子物体                                   AddListener:监听
        gameoverPanel.transform.Find("btn_Again").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            //重新加载当前正在运行的场景
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//或者 SceneManager.LoadScene(0);
        });
        gameoverPanel.transform.Find("btn_Main").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Start");
        });
    }
    //刷新信息
    public void Refresh(int hp,int level,int time,int enemy)
    {
        txt_HP.text = "HP : " + hp.ToString();
        txt_Level.text = "Level : " + level.ToString();
        txt_Time.text = "Time : " + time.ToString();
        txt_Enemy.text = "Enemy : " + enemy.ToString();
    }
    public void ShowGameoverPanel()
    {
        gameoverPanel.SetActive(true);//物体设置为可显示状态
    }
    /// <summary>
    /// 播放关卡提示动画
    /// </summary>
    /// <param name="levelIndex"></param>
    public void PlayLevelFade(int levelIndex)
    {
        Time.timeScale = 0.0f;
        LevelFadeAnim.transform.Find("txt_Level").GetComponent<Text>().text = "Level " + levelIndex.ToString();
        LevelFadeAnim.Play("LevelFade",0,0);
        startDealy = true;
    }

    private bool startDealy = true;
    private float timer = 0;
    private void Update()
    {
        if (startDealy)
        {
            //      unscaledDeltaTime:不受Time.timeScale影响
            timer += Time.unscaledDeltaTime;//这里不能使用Time.deltaTime,其会受到Time.timeScale的影响
            if (timer > 0.7f)
            {
                startDealy = false;
                Time.timeScale = 1;
                timer = 0;
            }
        }
        
    }

}
