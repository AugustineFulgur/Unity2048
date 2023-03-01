
using UnityEngine;

public class eventProcessor : MonoBehaviour //事件处理器，静态方法 //其实就是事件发生时调用的方法，做一些额外的操作
{
    public GameObject board;
    public GameObject sign;
    public AudioSource onCreate;
    public AudioSource onMerge; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onBlockMerge(int bonus){ //发生合并事件
        board.GetComponent<boardObject>().score+=bonus;
        onMerge.Play();
    }

    public void onBlockCreate(){ //方块创建
        onCreate.Play();
    }

    public void onSignMake(){ //做出手势
        Debug.Log(sign.GetComponent<signJudge>().dir);
    }
}
