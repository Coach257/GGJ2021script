using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*这个脚本的作用如下：
 * 1. 基本操作，AD空格控制角色，R重玩，Esc退出
 * 2. 上出界、下出界时结局判定
 * 3. emotion、body、health自然改变量，及这三个数值导致的GameOver
 * 4. 游戏结束时，出现结局文字
 */

public class PlayerMove : MonoBehaviour
{
    public List<Sprite> Sprites;//0站立，1跳跃，234移动
    public float v = 10;//移动速度
    public float ren_T = 0.25f;//贴图切换的周期
    public float health_T = 1;//自然回复/扣除health的周期
    public float emotion_T = 1;//自然回复/扣除emotion的周期
    public float body_T = 1;//自然回复/扣除body的周期
    public float health_virus_T = 1;//感染时扣除health的周期
    Rigidbody2D rig;
    SpriteRenderer ren;
    BoxCollider2D box;
    float ren_t = 0;//切换贴图用的计时器
    float health_t = 0;//自然回复/扣除health的计时器
    float emotion_t = 0;//自然回复/扣除emotion的计时器
    float body_t = 0;//自然回复/扣除body的计时器
    float health_virus_t = 1;//感染时扣除health的计时器
    float play_time = 0;//游戏时长
    Text result;//结局文字
    Text result_quit;//结局时指引重来/退出的文字
    //private GameObject camera;

    private Transform isGround;
    private Animator animator;


    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        result = GameObject.Find("Result").GetComponent<Text>();//告知结果的UI
        result_quit = GameObject.Find("Result_Quit").GetComponent<Text>();//提示退出的UI
        animator = GetComponent<Animator>();
        isGround = transform.Find("isGround");
        GameManager.Instance.init(Game_State.Kid);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        valueController();//此函数用来控制自然回复/扣除量，满足胜利条件，或血量不够GameOver也在里面
        gamestateController();
        //非GameOver时，可以控制角色移动
        if (!GameManager.Instance.isGameOver)
        {
            Move();
            Jump();
            Fall();
        }
    }

    #region move and jump

    private bool GetIsGround()
    {
        RaycastHit2D info = Physics2D.Raycast(isGround.transform.position, Vector2.down, 0.1f);
        if (info.collider != null && info.collider.gameObject.TryGetComponent(out Bricks bricks))
        {
            return true;
        }
        return false;
    }

    private void Move()
    {
        int dir = (int)Input.GetAxisRaw("Horizontal");
        float x = 0;
        if(dir != 0)
        {
            x = v * dir;
            if (dir * transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            animator.SetBool("walking", true);
        }
        else
        {
            x = 0;
            animator.SetBool("walking", false);
        }
        rig.velocity = new Vector2(x, rig.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.W) && animator.GetBool("jumping") == false)
        {
            rig.velocity = new Vector2(rig.velocity.x, 6);
        }
    }

    private void Fall()
    {
        animator.SetBool("jumping", !GetIsGround());
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TopThorn")//碰到上面的东西，失败
        {
            GameManager.Instance.gameState = Game_State.KidGameOver1;
            Result();
        }
        if (collision.tag == "DownEdge")//下面掉出去
        {
            GameManager.Instance.gameState = Game_State.KidGameOver2;
            Result();
        }
    }


    void valueController()//控制游戏参数及结束判定
    {
        switch (GameManager.Instance.gameState)
        {
            case Game_State.Kid:
                //体魄减少
                if (body_t >= body_T)
                {
                    GameManager.Instance.add("Body", -1);
                    GameManager.Instance.add("Emotion",-1);
                    body_t = 0;
                }
                body_t += Time.deltaTime;
                //条件下，健康减少；不满足条件时，health_t清零
                if (GameManager.Instance.UIValueList["Body"].nowvalue < 5 || GameManager.Instance.UIValueList["Emotion"].nowvalue < 20)
                {
                    if (health_t >= health_T)
                    {
                        GameManager.Instance.add("Body",-1);
                        health_t = 0;
                    }
                    health_t += Time.deltaTime;
                }
                else
                    health_t = 0;
                //感染病毒时，健康减少
                if (GameManager.Instance.isInfected)
                {
                    if (health_virus_t >= health_virus_T)
                    {
                        GameManager.Instance.add("Body",-1);
                        health_virus_t = 0;
                    }
                    health_virus_t += Time.deltaTime;
                }
                break;
        }
        //HP为0，失败
        if (GameManager.Instance.UIValueList["Body"].nowvalue <= 0)
        {
            GameManager.Instance.gameState = Game_State.KidGameOver3;
            Result();
        }
        //emotion足够，胜利
        if(GameManager.Instance.UIValueList["Emotion"].nowvalue >= GameManager.Instance.UIValueList["Emotion"].maxvalue)
        {
            if(GameManager.Instance.gameState == Game_State.Kid)
            {
                GameManager.Instance.gameState = Game_State.KidWin;
            }
            if (GameManager.Instance.gameState == Game_State.Youth)
            {
                GameManager.Instance.gameState = Game_State.YouthWin;
            }

            Result();
        }
    }
    void gamestateController()
    {
        //任意时刻可以重玩、退出游戏
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.gameReload();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //暂停继续
            if(Time.timeScale == 0)
            {
                gamecontinue();
            }
            else
            {
                gamepause();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameObject.Find("Audio Source").GetComponent<AudioController>().change();

        }
        //孩子胜利，C继续游戏
        if (GameManager.Instance.gameState == Game_State.KidWin && Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.init(Game_State.Youth);//初始化至Youth状态
        }
        //少年胜利，C继续游戏
        if (GameManager.Instance.gameState == Game_State.YouthWin && Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.init(Game_State.Worker);//初始化至Worker状态
        }
    }
    public void Result()//出现结局时
    {
        //SceneManager.LoadScene("End");
        //Player及照相机静止
        box.enabled = false;
        rig.gravityScale = 0;
        rig.velocity = Vector2.zero;
        GameManager.Instance.isGameOver = true;

        //显示游戏结束UI
        result.enabled = true;
        result_quit.enabled = true;
        switch (GameManager.Instance.gameState)
        {
            case Game_State.KidWin:
                result.text = "Win!\n愉快的童年时光是短暂的";
                result.color = new Color32(78, 187, 78, 255);
                break;
            case Game_State.KidGameOver1:
                result.text = "GameOver\n胡闹的时候被妈妈逮到了，\n暴打了一顿！";
                result.color = new Color(1, 0, 0, 1);
                break;
            case Game_State.KidGameOver2:
                result.text = "GameOver\n玩耍的时候脚下要站稳，\n注意安全呐！";
                result.color = new Color(1, 0, 0, 1);
                break;
            case Game_State.KidGameOver3:
                result.text = "GameOver\n小孩子要多吃饭、心情好，\n才能健康成长！";
                result.color = new Color(1, 0, 0, 1);
                break;
        }
    }
    public void initplayerstate()//继续游戏，恢复玩家状态
    {
        box.enabled = true;
        rig.gravityScale = 1;
        rig.velocity = new Vector2(5,0);
        GameManager.Instance.isGameOver = false;

        //显示游戏结束UI
        result.enabled = false;
        result_quit.enabled = false;
    }

    public void gamecontinue()
    {
        Time.timeScale = 1;
        GameObject.Find("Pause").GetComponent<Image>().sprite = Resources.Load("UI/暂停", typeof(Sprite)) as Sprite;
    }
    public void gamepause()
    {
        Time.timeScale = 0;
        GameObject.Find("Pause").GetComponent<Image>().sprite = Resources.Load("UI/继续", typeof(Sprite)) as Sprite;
    }

}
