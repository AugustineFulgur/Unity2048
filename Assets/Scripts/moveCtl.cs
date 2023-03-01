using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCtl : MonoBehaviour //移动控制器
{
    public GameObject board; //boardObject
    public const float sec=0.3f; //完成移动的时间（秒）
    private float del=0.0f;
    private bool move; //move为真开始移动
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(del>=sec){
            complete();
        } //时间到了，停止
        else if(move){
            run();
        }

    }

    private void run(){ 
        float time=Time.deltaTime;
        if(del+Time.deltaTime>sec){ //防止帧数波动导致的移动误差
            time=sec-del;
        }
        foreach(GameObject m in board.GetComponent<boardObject>().objboard){
            girdObject v=m.GetComponent<girdObject>(); //简写
            if(v.GetComponent<girdObject>().tran==girdObject.zero){
                continue; //无传送就跳过
            }
            m.transform.Translate(time*v.tran.x*(1/sec),time*v.tran.y*(1/sec),0);
        }
        del+=Time.deltaTime;
    }

    public void set(){ //设置移动器，需要在run之前调用 外部通过调用set实现移动
        foreach(GameObject m in board.GetComponent<boardObject>().objboard){
            girdObject v=m.GetComponent<girdObject>(); //简写
            if(v.tranPosition!=girdObject.zero){ //有传送位置
                v.tran=board.GetComponent<boardObject>().board[v.tranPosition.x][v.tranPosition.y]-board.GetComponent<boardObject>().board[v.pos.x][v.pos.y]; //修正传送距离
            }           
        }//更新tran
        move=true;
    }

    private void complete(){ //用于更新objboard
        List<GameObject> b=board.GetComponent<boardObject>().objboard;
        //想来想去，最后还是决定采用一个最简单粗暴的方法。
        //在一次移动之中，如果发生了合并，必然会有一个方块被销毁，一个方块发生升级。
        //我们将销毁和升级分别用一个bool表示，这就让发生合并的两个方块之间不会发生冲突。
        //然后，我们首先销毁所有被标记为销毁的方块，然后将所有方块移动。这样就不会发生合并冲突。

        for(int i=b.Count-1;i>=0;i--){ //销毁所有需要销毁的块（这里不能够使用foreach）
            if(b[i].GetComponent<girdObject>().destroy==true){
                GameObject m=b[i];
                b.Remove(m);
                Destroy(m,0f);
                Debug.Log("销毁一个方块。");
            }
        }
        foreach(GameObject m in b){ //升级与移动
            girdObject v=m.GetComponent<girdObject>(); //简写
            v.tran=girdObject.zero; //擦除tran
            if(v.level){ //需要升级
                v.levelup();
                v.level=false; //擦除升级标记
            }
            if(v.tranPosition!=girdObject.zero){ //需要移动
                v.pos=v.tranPosition;
                v.tranPosition=girdObject.zero; //擦除tranPosition
            }
        }
        board.GetComponent<boardObject>().roundEnd();
        board.GetComponent<boardObject>().createNew(); //创建一个新的方块
        move=false;
        del=0f; //置空
    }

}
