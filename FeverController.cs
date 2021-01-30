using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*这个脚本的作用如下：
 * 1. 记录、控制游戏倒计时，控制fever的开关、持续、冷却等
 */


public class FeverController : MonoBehaviour
{
    PlayerMove player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isGameOver)
        {
            GameManager.Instance.gameTime -= Time.deltaTime;//阶段倒计时
            //用来进入fever
            if (GameManager.Instance.feverTime <= 0 && GameManager.Instance.feverCold <= 0)//没有fever，且不在冷却中
            {
                if (GameManager.Instance.gameTime <= 150)
                {
                    GameManager.Instance.gameSpeed = 1.5f;//fever后的游戏速度
                    GameManager.Instance.feverTime = 30;//fever持续时间
                    GameManager.Instance.feverCold = 45;//fever冷却时间（包括持续）
                }
            }
            //feverTime、feverCold倒计时
            if (GameManager.Instance.feverTime >= 0)
                GameManager.Instance.feverTime -= Time.deltaTime;
            if (GameManager.Instance.feverCold >= 0)
                GameManager.Instance.feverCold -= Time.deltaTime;
            //fever结束时复原
            if (GameManager.Instance.feverTime <= 0 && GameManager.Instance.gameSpeed > 1)
            {
                GameManager.Instance.gameSpeed = 1;
            }
        }
    }
}
