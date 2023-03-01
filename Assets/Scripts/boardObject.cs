using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
using TMPro;

public class boardObject : MonoBehaviour //棋盘
{
    public Vector2[][] board; 
        //所有格子对象初始位置（Vector2）的二维数组
    public List<GameObject> objboard;
        //所有格子实例对象的列表
    private List<Vector2Int> dic; 
        //空值表
    public int score=0; //分数
    public GameObject onevent;
    private const float boardx=-3.74f+0.5f;
    private const float boardy=4.16f-0.5f;
    public const float lenx=1.082f;
    public const float leny=1.048f; //长度以及边界
    public GameObject cube; //指定的预制体对象
    private Vector2Int gird; //生成新格子用
    public GameObject scoreText; //表示分数的字体

    // Start is called before the first frame update
    void Start()
    {
        board=new Vector2[7][];
        objboard=new List<GameObject>();
        dic=new List<Vector2Int>();
        for(int x=0;x<7;x++){
            board[x]=new Vector2[8];
            for(int y=0;y<8;y++){
                board[x][y]=new Vector2(boardx+lenx*x,boardy-leny*y); //实体位置表
            }
        }    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createNew(){ //随机创建一个新的格子
        ran();
        GameObject o=Instantiate<GameObject>(cube);
        o.GetComponent<girdObject>().setgird(gird); //初始化
        o.transform.position=board[gird.x][gird.y]; //传送到此位置
        objboard.Add(o); //放入位置数组
        onevent.GetComponent<eventProcessor>().onBlockCreate(); 
        Debug.Log("创建一个方块");
        gird=girdObject.zero;
    }

    void ran(){ //返回一个无对象的坐标 
        for(int i=0;i<7;i++){
            for(int j=0;j<8;j++){
                dic.Add(new Vector2Int(i,j));
            }
        } //建立一个满的表
        foreach(GameObject m in objboard){ //老混淆大师了
            if(dic.Contains(m.GetComponent<girdObject>().pos)==false){
                Vector2Int a=m.GetComponent<girdObject>().pos;
                Debug.Log("发现重叠块在位置"+a.x.ToString()+","+a.y.ToString());
            }
            dic.Remove(m.GetComponent<girdObject>().pos); //移除有方块的位置，得到一个空值表
        }
        gird=dic[Random.Range(0,dic.Count-1)]; //取出一个随机的空值赋给gird 
        dic=new List<Vector2Int>(); //置空dic  
    }

    int judge(){ //结局判定
        foreach(GameObject m in objboard){
            if(m.GetComponent<girdObject>().bonus==2048){
                return 1; //有目标方块，胜利结局
            }
        }
        if(objboard.Count<56){
            return 0; //还有空位，继续
        }
        return -1; //失败结局
    }

    public GameObject getPositionBlock(int x,int y){ //获取此位置上的方块（当前位置在此位置上）
        Vector2Int a=new Vector2Int(x,y); 
        foreach(GameObject m in objboard){
            if(m.GetComponent<girdObject>().pos==a){
                return m;
            }
        }
        return null;
    }

    public GameObject getCanMergeBlock(int x,int y){ //获取此位置上可以合并的方块（包括即将销毁的，算作障碍物）
        Vector2Int a=new Vector2Int(x,y);
        foreach(GameObject m in objboard){
            girdObject v=m.GetComponent<girdObject>(); //临时的对象v
            if(v.pos==a && v.tranPosition==girdObject.zero){
                //这个方块：在此位置上&不需要移动 无论是否会被消灭（因为即使被消灭了也还有一个方块占据位置，作为合并后方块）
                return m; //这个方块可以用于合并处理
            }
            if(v.tranPosition==a && v.level!=true){
                //这个方块：要移动到此位置上&不要升级（没有被合并处理过）
                return m;
            }
        }
        return null;
    }

    public void roundEnd(){ //处理回合结束的函数
        scoreText.GetComponent<TMP_Text>().text=score.ToString(); //更新分数面板
        int x=judge(); //判断结局
    }
}
