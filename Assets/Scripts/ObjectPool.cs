using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//对象池
public enum ObjectType
{
    SuperWall,
    Wall,
    Prop,
    Bomb,
    Enemy,
    BombEffect
}

[System.Serializable]//标签
public class Type_Prefab//物体的类型和预制体的对照关系
{
    public ObjectType type;
    public GameObject prefab;
}
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;//单例
    public List<Type_Prefab> type_Prefabs = new List<Type_Prefab>();//这里还没有实例化，要加上标签
    /// <summary>
    /// 通过物体类型获取该预制体
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject GetPrebyType(ObjectType type)
    {
        foreach (var item in type_Prefabs)
        {
            if (item.type == type)
                return item.prefab;
        }
        return null;
    }
    /// <summary>
    /// 物体类型和对应的对象池关系字典
    /// </summary>
    private Dictionary<ObjectType, List<GameObject>> dic = 
        new Dictionary<ObjectType, List<GameObject>>();

    //"Awake" 是一个 Unity 中的生命周期函数，它会在游戏对象被激活时调用。
    //"Instance" 是一个静态变量，在这个函数中，它被赋值为 "this"，也就是当前游戏对象的引用。
    //因此，这段代码的作用是将当前游戏对象赋值给 "Instance" 变量，并且这个变量是静态的，因此可以在整个程序中访问。
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 通过物体类型从相对应的对象池中取东西
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject Get(ObjectType type,Vector2 pos)
    {
        GameObject temp = null;
        //判断字典中有没有与该类型匹配的对象池，没有则创建
        if (dic.ContainsKey(type) == false)
            dic.Add(type, new List<GameObject>());
        //判断该类型对象池中有没有物体
        if (dic[type].Count > 0)
        {
            int index = dic[type].Count - 1;
            temp = dic[type][index];
            dic[type].RemoveAt(index);
        }
        else
        {
            GameObject pre = GetPrebyType(type);
            if (pre != null)
            {
                temp = Instantiate(pre, transform);
            }
        }
        //显示物体
        temp.SetActive(true);
        //方向
        temp.transform.position = pos;
        temp.transform.rotation = Quaternion.identity;
        
        return temp;
    }
    /// <summary>
    /// 回收
    /// </summary>
    /// <param name="type"></param>
    public void Add(ObjectType type,GameObject go)
    {
        //判断该类型是否有对应的对象池以及对象池中不存在该物体
        if(dic.ContainsKey(type)&& dic[type].Contains(go) == false)//dic[type].Contains(go) == false 保证对象池唯一
        {
            //放入对象池
            dic[type].Add(go);
        }
        //隐藏物体
        go.SetActive(false);
    }
}
