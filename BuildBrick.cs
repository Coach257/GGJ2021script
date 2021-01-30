using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBrick : MonoBehaviour
{
    public GameObject brick;
    public GameObject item;
    public float brick_T = 1;//生成砖块的周期
    public float brick_t = 0;//生成砖块计时器
    public float d = 3.4f;//砖块宽度
    public float l = 14.3f;//场景宽度
    System.Random random = new System.Random();

    float x1, x2, x3, x4;//用来生成道具和砖块

    private Dictionary<Item_Type, int> dicKid, dicYouth, dicWorker;

    private void Awake()
    {
        Random.InitState(System.Environment.TickCount);
    }

    // Start is called before the first frame update
    void Start()
    {
        initDic();
    }

    private void initDic()
    {
        dicKid = new Dictionary<Item_Type, int>();
        dicKid[Item_Type.Kid_Food_Milk] = 32;
        dicKid[Item_Type.Kid_Food_Spicy] = 12;
        dicKid[Item_Type.Kid_Food_VC] = 8;
        dicKid[Item_Type.Kid_Toy_Small] = 40;
        dicKid[Item_Type.Kid_Toy_Big] = 12;
        dicKid[Item_Type.Kid_Event_Virus] = 10;

        dicYouth = new Dictionary<Item_Type, int>();
        dicYouth[Item_Type.Youth_Food_Small] = 20;
        //dicYouth[Item_Type.Youth_Food_Big] = 10;
        dicYouth[Item_Type.Youth_Food_Spicy] = 10;

        dicWorker = new Dictionary<Item_Type, int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isGameOver)
        {
            if (brick_t > brick_T)
            {
                int n = Random.Range(1, 11);
                if (n <= 4)//40%概率出1个砖块
                {
                    x1 = Random.Range(-l + 0.5f * d, l - 0.5f * d);
                    createBrick(x1);
                    n = Random.Range(1, 11);//1砖，50%0道具，30%1道具，20%2道具
                    if (n < 6) ;//50%不生成道具
                    if (n >= 6 && n < 7)//10%在砖上生成1个道具
                    {
                        createItem(x1);
                    }
                    else if (n >= 7 && n < 8)//10%在砖左生成1个道具
                    {
                        createItem(0.5f * (-l + x1));
                    }
                    else if (n >= 8 && n < 9)//10%在砖右生成1个道具
                    {
                        createItem(0.5f * (l + x1));
                    }
                    else if (n >= 9 && n < 10)//10%在砖上生成2个道具
                    {
                        createItem(x1 - 0.5f * d);
                        createItem(x1 + 0.5f * d);
                    }
                    else//10%在砖上生成1个道具且在砖左/右生成1个道道具
                    {
                        createItem(x1);
                        if (x1 <= 0)//方块在零点左，右侧生成
                            createItem(0.5f * (l + x1));
                        else//方块在零点左，右侧生成
                            createItem(-0.5f * (l + x1));
                    }
                }
                else if (n > 4 && n <= 8)//40%概率出2个砖块
                {
                    x1 = Random.Range(-l + 0.5f * d, l - 1.5f * d);
                    createBrick(x1);
                    x2 = Random.Range(x1 + d, l - 0.5f * d);
                    createBrick(x2);
                    n = Random.Range(1, 11);//30%0道具，30%1道具，20%2道具，20%3道具
                    if (n < 4) ;//30%不生成道具
                    if (n >= 4 && n < 7)//30%概率1道具
                    {
                        x1 = Random.Range(-l + 0.5f * d, l - 1.5f * d);
                        createItem(x1);
                    }
                    else if (n >= 7 && n < 9)//20%概率2道具
                    {
                        x1 = Random.Range(-l + 0.5f * d, l - 1.5f * d);
                        createItem(x1);
                        x2 = Random.Range(x1 + d, l - 0.5f * d);
                        createItem(x2);
                    }
                    else//20%概率3道具
                    {
                        x1 = Random.Range(-l + 0.5f * d, l - 2.5f * d);
                        createItem(x1);
                        x2 = Random.Range(x1 + d, l - 1.5f * d);
                        createItem(x2);
                        x3 = Random.Range(x2 + d, l - 0.5f * d);
                        createItem(x3);
                    }

                }
                else//20%概率出3个砖块
                {
                    x1 = Random.Range(-l + 0.5f * d, l - 2.5f * d);
                    createBrick(x1);
                    x2 = Random.Range(x1 + d, l - 1.5f * d);
                    createBrick(x2);
                    x3 = Random.Range(x2 + d, l - 0.5f * d);
                    createBrick(x3);
                    n = Random.Range(1, 11);//10%0道具，40%1道具，20%2道具，20%3道具,10%4道具
                    if (n < 2) ;//10%不生成道具
                    if (n >= 2 && n < 6)//40%概率1道具
                    {
                        x1 = Random.Range(-l + 0.5f * d, l - 1.5f * d);
                        createItem(x1);
                    }
                    else if (n >= 6 && n < 8)//20%概率2道具
                    {
                        x1 = Random.Range(-l + 0.5f * d, l - 1.5f * d);
                        createItem(x1);
                        x2 = Random.Range(x1 + d, 1 - 0.5f * d);
                        createItem(x2);
                    }
                    else if (n >= 8 && n < 10)//20%概率3道具
                    {
                        x1 = Random.Range(-l + 0.5f * d, l - 2.5f * d);
                        createItem(x1);
                        x2 = Random.Range(x1 + d, l - 1.5f * d);
                        createItem(x2);
                        x3 = Random.Range(x2 + d, l - 0.5f * d);
                        createItem(x3);
                    }
                    else//10%概率4道具
                    {
                        x1 = Random.Range(-l + 0.5f * d, l - 3.5f * d);
                        createItem(x1);
                        x2 = Random.Range(x1 + d, l - 2.5f * d);
                        createItem(x2);
                        x3 = Random.Range(x2 + d, l - 1.5f * d);
                        createItem(x3);
                        x4 = Random.Range(x3 + d, l - 0.5f * d);
                        createItem(x4);
                    }
                }
                brick_t = 0;
            }
            brick_t += Time.deltaTime * GameManager.Instance.gameSpeed;
        }
    }

    void createBrick(float x)
    {
        GameObject newBrick = Instantiate(brick, transform.position + new Vector3(x, 0, 0), Quaternion.identity);
        
        int i = random.Next(0,101);//砖块类型
        if (i <= 80)//80%普通砖块
            newBrick.GetComponent<Bricks>().state = Brick_State.None;
        else//20%脆弱砖块
            newBrick.GetComponent<Bricks>().state = Brick_State.Fragile;
    }

    void createItem(float x)
    {
        GameObject newItem = Instantiate(item, transform.position + new Vector3(x, 1, 0), Quaternion.identity);
        Dictionary<Item_Type, int> dic = new Dictionary<Item_Type, int>();
        switch (GameManager.Instance.gameState)
        {
            case Game_State.Kid:
                dic = dicKid;
                if (GameManager.Instance.isInfected)
                {
                    dic[Item_Type.Kid_Event_Pill] = 20;
                }
                else
                {
                    dic[Item_Type.Kid_Event_Pill] = 1;
                }
                break;
            case Game_State.Youth:
                dic = dicYouth;
                break;
            case Game_State.Worker:
                dic = dicWorker;
                break;
        }
        newItem.GetComponent<item>().type = myRamdom(dic);
    }
    
    /*
    Youth_Food_Small,
    Youth_Food_Big,
    Youth_Food_Spicy,
    Youth_Study,
    Youth_Play_Cmpt,
    Youth_Play_Spot,
    Youth_Play_Love,
    Youth_Play_Argue,
    Youth_Event_Exam_T,
    Youth_Event_Exam_F,
    */

    private T myRamdom<T>(Dictionary<T, int> dic)
    {
        int sum = 0;
        foreach (var kvp in dic)
        {
            sum += kvp.Value;
        }
        int ran = random.Next(0, sum);
        foreach (var kvp in dic)
        {
            if (ran < kvp.Value) return kvp.Key;
            ran -= kvp.Value;
        }
        Debug.LogError("WFT");
        return default;
    }
}
