using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : MonoBehaviour
{
    /// <summary>
    /// 当动画播放完
    /// </summary>
    private void AniFin()//第一种销毁方式
    {
        //ObjectPool.Instance.Add(ObjectType.BombEffect, gameObject);
    }

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //第二种销毁方式 
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);//0代表Base Layer,GetCurrentAnimatorStateInfo(0)即是获取当前动画控制器当前正在播放的动画状态
        // normalizedTime：获得当前播放动画的播放进度（0-1的浮点值）  这里1是播放完
        if (info.normalizedTime >= 1 && info.IsName("BombEffect"))
        {
            ObjectPool.Instance.Add(ObjectType.BombEffect, gameObject);
        }
    }
}
