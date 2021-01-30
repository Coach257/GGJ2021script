using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*这个脚本的作用如下：
 * 1. 游戏的不同状态（包括不同阶段、不同胜利结局、不同失败结局）
 * 2. 游戏的不同数值变量，及修改数值的函数（包括 emotion、body、health 等）
 * 3. 角色状态，例如 isInfected 感染病毒与否，isGameOver 是否游戏结束
 * 4. 根据参数，将游戏初始化，包括加载场景、负面状态清除、各数据初始化
 */

public enum Game_State//游戏的不同阶段
{
    Kid,
    KidWin,
    Youth,
    YouthWin,
    Worker,
    WorkerWin,
    KidGameOver1,//被妈妈逮住
    KidGameOver2,//摔下去
    KidGameOver3,//体魄为0
}

public enum Fever_State//影响游戏速度的值，在 Player 中修改
{
    slow,//慢
    normal,//一般
    fast,//快
}

public class UI_Value
{
    public string name;
    public int nowvalue;
    public int maxvalue;
    public UI_Value(int nowvalue,int maxvalue,string name)
    {
        this.nowvalue = nowvalue;
        this.maxvalue = maxvalue;
        this.name = name;
    }
    public void add(int x)
    {
        this.nowvalue += x;
        this.nowvalue = Mathf.Min(this.nowvalue, this.maxvalue);
        this.nowvalue = Mathf.Max(this.nowvalue, 0);
    }
}


public class GameManager
{
    public bool isGameOver = false;//主要影响角色控制和摄像机
    public bool isInfected = false;//是否染病毒
    public Game_State gameState = Game_State.Kid;
    public float gameTime = 180;//当前阶段倒计时
    public float feverTime = 0;//fever时间
    public float feverCold = 0;//fever 冷却，平时30s，fever15s
    public float gameSpeed = 1;
    public Dictionary<string ,UI_Value> UIValueList= new Dictionary<string,UI_Value>();
    public PlayerMove playerMove;
    public MyUI myUI;


    private GameManager() { }
    static GameManager game = null;
    static public GameManager Instance//单例，用来储存游戏数据
    {
        get
        {
            if (game == null)
            {
                game = new GameManager();
            }
                
            return game;
        }
    }
    public void initGameManager()
    {
        game.playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
        game.myUI = GameObject.Find("Text").GetComponent<MyUI>();
        game.myUI.initAllUI();
        game.feverTime = 0;
        game.gameTime = 180;
        game.feverTime = 0;
        game.feverCold = 0;
        game.gameSpeed = 1;
    }

    public void init(Game_State state)
    {
        myUI.updateplayer(state);
        switch (state)
        {
            case Game_State.Kid:
                //加载场景，各状态初始化
                gameState = Game_State.Kid;
                isGameOver = false;
                isInfected = false;
                //各数据上限及当前值初始化
                UIValueList["Body"] = new UI_Value(30, 100, "体魄");
                UIValueList["Emotion"] = new UI_Value(30, 100,"情绪");
                myUI.enable("Body");
                myUI.enable("Emotion");
                playerMove.initplayerstate();
                break;
            case Game_State.Youth:
                gameState = Game_State.Youth;
                isGameOver = false;
                isInfected = false;
                //各数据上限及当前值初始化
                UIValueList["Study"] = new UI_Value(30, 100, "学业");
                UIValueList["Emotion"].nowvalue = 30;
                MyUI.GetInstance().enable("Study");
                playerMove.initplayerstate();
                break;
            case Game_State.Worker:
                gameState = Game_State.Worker;
                isGameOver = false;
                isInfected = false;
                //各数据上限及当前值初始化
                UIValueList["Relation"] = new UI_Value(30, 100, "人际");
                UIValueList["Family"] = new UI_Value(30, 100, "家庭");
                UIValueList["Emotion"].nowvalue = 30;
                MyUI.GetInstance().enable("Relation");
                MyUI.GetInstance().enable("Family");
                playerMove.initplayerstate();
                break;
        }
    }
    public void add(string name ,int a)
    {
        UIValueList[name].add(a);
    }

    public void gameReload()
    {
        SceneManager.LoadScene("SampleScene");
    }

    
}