using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class signJudge : MonoBehaviour //判断手势 //通过鼠标\触摸按下和抬起的位置，判断手势的距离和方向是否有效
{
    public GameObject onevent;
    private bool thistime=false; //此次是否按下
    private bool lasttime=false; //上一帧是否按下
    private bool flagend=false; //手势完成标志
    private Touch thistouch=new Touch(); //手机触摸的实例
    private Vector2 startpoint=Vector2.zero; //起点
    private Vector2 endpoint=Vector2.zero; //终点
    private int width=UnityEngine.Screen.width; //屏幕宽度，用于确定相对滑动距离
    private int height=UnityEngine.Screen.height; 
    public Direction dir;
    public enum Direction{
        Up,
        Down,
        Left,
        Right,
        None //无效返回
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouse();
        if(flagend){ //手势完成了
            dir=judge(); //两种输入共用的手势判定函数
            setVoid();
            if(dir!=Direction.None){
                onevent.GetComponent<eventProcessor>().onSignMake();
            }
        }
        
    }

    void mouse(){
        lasttime=thistime;
        if(Input.GetMouseButtonDown(0)){
            thistime=true;
        }
        if(Input.GetMouseButtonUp(0)){
            thistime=false;
        }
        if(thistime!=lasttime){ //变动
            if(thistime==true){ //按下
                startpoint=Input.mousePosition;
            }
            if(thistime==false){ //松开
                flagend=true;
                endpoint=Input.mousePosition;
            }
        }

    }

    Direction judge(){
        Vector2 change=endpoint-startpoint;
        float x=System.Math.Abs(change.x);
        float y=System.Math.Abs(change.y);
        if(x>width/10 | y>height/10){ //距离过关
            if(x>y){
                if(change.x>0){
                    return Direction.Right;
                }
                else if(change.x<0){
                    return Direction.Left;
                }
            }
            else if(x<y){
                if(change.y>0){
                    return Direction.Up;
                }
                else if(change.y<0){
                    return Direction.Down;
                }
            }
        }
        return Direction.None; //无效的情况

    }
    void touch(){
        if(Input.touchCount>0){ //有触摸
            thistouch=Input.GetTouch(0); //获取第一个触摸
        }
        if(thistouch.phase==TouchPhase.Began){ //开始
            startpoint=thistouch.position;
        }
        if(thistouch.phase==TouchPhase.Ended){ //结束
            flagend=true;
            endpoint=thistouch.position;
        }

    } //无效 但是可以留着

    void setVoid(){ //重置手势判断
        flagend=false; //置回
        thistime=false;
        lasttime=false;
        startpoint=Vector2.zero;
        endpoint=Vector2.zero;
    }
}
