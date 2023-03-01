using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class girdObject : MonoBehaviour //格子类实例，现在它有位置属性了
{
    // Start is called before the first frame update
    public int bonus=2; //初始当然是2
    public static Vector2Int zero=new Vector2Int(-999,-999); //“重定义”的零向量
    public Vector2 tran=zero; //用于移动
    public Vector2Int tranPosition=zero; //要移动到的位置
    public bool level=false; //升级标志
    public bool destroy=false; //销毁标志
    public int rank=1; //阶
    public Vector2Int pos = zero; //自述位置属性
    public GameObject eventp; //事件管理器
    void Start()
    {
        //这玩意被创造的时候有音效
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setgird(Vector2Int position){
        this.bonus=2; //可以赋初值的（伪）构造函数
        this.rank=1;
        this.pos = position;
        updateSprite(); //更新精灵

    }

    public void updateSprite(){ //更新精灵
        gameObject.GetComponent<SpriteRenderer>().sprite=Resources.Load<Sprite>("cube-"+bonus.ToString());
    }

    public void levelup(){ //合并之后的升级函数
        eventp.GetComponent<eventProcessor>().onBlockMerge(bonus);
        rank+=1;
        bonus*=2;
        updateSprite(); //更新精灵
    }

}
