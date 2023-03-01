using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mergeCtl : MonoBehaviour
{
    public GameObject board;
    public GameObject sign;
    public GameObject movec;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        move(sign.GetComponent<signJudge>().dir);
        sign.GetComponent<signJudge>().dir=signJudge.Direction.None; //重置方向
    }

    public void move(signJudge.Direction dir){
        if(dir==signJudge.Direction.None){
            return; //识别不出方向就返回
        }
        boardObject b=board.GetComponent<boardObject>(); //实在想不出什么好名字，这个是简写的board
        bool flag=false; //向前移动标记 //是否要向前移动（如果合并过就不需要了）
        int count=0; //偏移值 //每遇到一个空块，加一
        bool breakflag=false; //switch中跳出循环的标记
        if(dir==signJudge.Direction.Left){
            for(int i=0;i<8;i++){ //竖排
                for(int j=0;j<7;j++){ //横排，从左向右判断处理
                    GameObject h=b.getPositionBlock(j,i); //待判断方块的简写
                    if(h==null){ //此方块不存在
                        continue; //下一个
                    }
                    for(int m=j-1;m>=0;m--){
                        GameObject v=b.getCanMergeBlock(m,i); //此位置方块的简写
                        switch(mergeJudge(v,h)){
                            case 1:
                            count++;
                            continue;
                            case 0:
                            flag=true;
                            breakflag=true;
                            break;
                            case -1:
                            breakflag=true;
                            break;
                        } //具体查看mergeJudge的定义
                        if(breakflag){
                            breakflag=false;
                            break;
                        }
                    }
                    if(!flag && count!=0){ //如果未合并且有偏移值
                        h.GetComponent<girdObject>().tranPosition=new Vector2Int(j-count,i);
                    }
                    flag=false;
                    count=0; //重置
                }                
            }
        }
        if(dir==signJudge.Direction.Right){
            for(int i=0;i<8;i++){
                for(int j=6;j>=0;j--){
                    GameObject h=b.getPositionBlock(j,i); //待判断方块的简写
                    if(h==null){ //此方块不存在
                        continue; //下一个
                    }
                    for(int m=j+1;m<7;m++){
                        GameObject v=b.getCanMergeBlock(m,i); //此位置方块的简写
                        switch(mergeJudge(v,h)){
                            case 1:
                            count++;
                            continue;
                            case 0:
                            flag=true;
                            breakflag=true;
                            break;
                            case -1:
                            breakflag=true;
                            break;
                        } //具体查看mergeJudge的定义
                        if(breakflag){
                            breakflag=false;
                            break;
                        }
                    }
                    if(!flag && count !=0){ //检查
                        h.GetComponent<girdObject>().tranPosition=new Vector2Int(j+count,i);
                    }
                    flag=false;
                    count=0; //重置
                }                
            }
        }
        if(dir==signJudge.Direction.Up){
            for(int i=0;i<7;i++){
                for(int j=0;j<8;j++){
                    GameObject h=b.getPositionBlock(i,j); //待判断方块的简写
                    if(h==null){ //此方块不存在
                        continue; //下一个
                    }
                    for(int m=j-1;m>=0;m--){
                        GameObject v=b.getCanMergeBlock(i,m); //此位置方块的简写
                        switch(mergeJudge(v,h)){
                            case 1:
                            count++;
                            continue;
                            case 0:
                            breakflag=true;
                            flag=true;
                            break;
                            case -1:
                            breakflag=true;
                            break;
                        } //具体查看mergeJudge的定义
                        if(breakflag){
                            breakflag=false;
                            break;
                        }
                    }
                    if(!flag && count!=0){ //检查
                        h.GetComponent<girdObject>().tranPosition=new Vector2Int(i,j-count);
                    }
                    flag=false;
                    count=0; //重置
                }
            }
        }
        if(dir==signJudge.Direction.Down){
            for(int i=0;i<7;i++){
                for(int j=7;j>=0;j--){
                    GameObject h=b.getPositionBlock(i,j); //待判断方块的简写
                    if(h==null){ //此方块不存在
                        continue; //下一个
                    }
                    for(int m=j+1;m<8;m++){
                        GameObject v=b.getCanMergeBlock(i,m); //此位置方块的简写
                        switch(mergeJudge(v,h)){
                            case 1:
                            count++;
                            continue;
                            case 0:
                            flag=true;
                            breakflag=true;
                            break;
                            case -1:
                            breakflag=true;
                            break;
                        } //具体查看mergeJudge的定义
                        if(breakflag){
                            breakflag=false;
                            break;
                        }
                    }
                    if(!flag && count!=0){ //检查
                        h.GetComponent<girdObject>().tranPosition=new Vector2Int(i,j+count);
                    }
                    flag=false;
                    count=0;
                }
            }
        }
        movec.GetComponent<moveCtl>().set(); //设置物理移动
    }

    int mergeJudge(GameObject v, GameObject h){ //对两个方块进行判断，v为待判断方块，h为被判断方块（感觉说不太清）
        if(v==null){ //此位置没有可合并的方块
            return 1; //count自加并且继续的处理
        }
        else if(v.GetComponent<girdObject>().bonus==h.GetComponent<girdObject>().bonus && v.GetComponent<girdObject>().destroy!=true && v.GetComponent<girdObject>().level!=true){ //两方块分数相同，且剩下那个方块没有被合并过
            //可以合并
            h.GetComponent<girdObject>().level=true; //升级
            v.GetComponent<girdObject>().destroy=true; //销毁
            if(v.GetComponent<girdObject>().tranPosition!=girdObject.zero){ //如果被合并方块发生移动
                    h.GetComponent<girdObject>().tranPosition=v.GetComponent<girdObject>().tranPosition;
                //两个方块移到同一个位置
            }
            else{ //也就是被合并方块不发生移动
                h.GetComponent<girdObject>().tranPosition=v.GetComponent<girdObject>().pos;
                //合并方块移到被合并方块位置
            }
            return 0;
            //flag置true并且跳出的处理
        }
        else{
            //有障碍物，退出
            return -1;
            //直接退出的处理 
        }
    }
}