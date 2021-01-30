using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*这个脚本的作用如下：
 * 1. 声明公开enum，所有道具类型（包括不同阶段、不同胜利结局、不同失败结局）
 * 2. List分类储存所有道具的贴图
 * 3. 储存道具的属性，在Start中用Switch修改
 */

public enum Item_Type
{
    None,
    Kid_Food_Milk,//牛奶&奶粉，体魄+10,概率24%
    Kid_Food_Spicy,//辣条，情绪+25，健康-1,概率8%
    Kid_Food_VC,//维C软糖，情绪+10，体魄+10,概率8%
    Kid_Toy_Small,//小积木，情绪+5，概率48%
    Kid_Toy_Big,//大积木&大汽车&大娃娃，情绪+20，体魄-2，概率12%
    Kid_Event_Virus,//病毒，情绪-50，特殊出现
    Kid_Event_Pill,//药物，健康+50，情绪-50，特殊出现

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

    Worker_Food,
    Worker_Play,
    Worker_Work,
    Worker_Travel,
    Worker_Event_JiaBan,
    Worker_Event_QingJiaZhang,
    Worker_Event_AoYeGongZuo,
    Worker_Event_ZhongCaiPiao,
    Worker_Event_JuCan,

    Elder_Event_Bed,
    Elder_Event_Tomb,
}

public class item : MonoBehaviour
{
    public Item_Type type = Item_Type.None;
    public GameObject Icon;

    public int A = 40;//旋转幅度
    public int omega = 10;//角速度
    int body = 0;//健康
    int emotion = 0;//情绪
    int study = 0;
    int relation = 0;
    int family = 0;


    public SpriteRenderer ren;

    void Awake()
    {
        ren = GetComponent<SpriteRenderer>();
        Random.InitState(System.Environment.TickCount);
        
        
    }

    void Start()
    {
        List<Sprite> sprites = GetItemImage(type.ToString());
        if(sprites.Count > 0)
        {
            ren.sprite = sprites[Random.Range(0, sprites.Count)];
        }
        else
        {
            Debug.LogError("No Spite for " + type);
        }
        InitProp();        
    }

    private void InitProp()
    {
        switch (type)
        {
            #region kid
            case Item_Type.Kid_Food_Milk:
                body = 5;
                break;
            case Item_Type.Kid_Food_Spicy:
                emotion = 15;
                body = -1;
                break;
            case Item_Type.Kid_Food_VC:
                emotion = 5;
                body = 5;
                break;
            case Item_Type.Kid_Toy_Small:
                emotion = 2;
                break;
            case Item_Type.Kid_Toy_Big:
                emotion = 10;
                body = -1;
                break;
            case Item_Type.Kid_Event_Virus:
                emotion = -50;
                GameManager.Instance.isInfected = true;
                break;
            case Item_Type.Kid_Event_Pill:
                body = 50;
                emotion = -50;
                break;
            #endregion
            #region youth
            case Item_Type.Youth_Food_Small:
                body = 10; emotion = 5;
                break;
            case Item_Type.Youth_Food_Big:
                body = 30; emotion = 15;
                break;
            case Item_Type.Youth_Food_Spicy:
                emotion = 20; body = -10;
                break;
            case Item_Type.Youth_Study:
                study = 10; emotion = -20;
                break;
            case Item_Type.Youth_Play_Cmpt:
            case Item_Type.Youth_Play_Love:
                emotion = 20; study = -5;
                break;
            case Item_Type.Youth_Play_Spot:
                body = 20;
                emotion = 20; study = -5;
                break;
            case Item_Type.Youth_Play_Argue:
                emotion = -10; study = -10;
                break;
            case Item_Type.Youth_Event_Exam_T:
                emotion = -1; study = 5;
                break;
            case Item_Type.Youth_Event_Exam_F:
                emotion = -10; study = -20;
                break;
            #endregion
            #region worker
            case Item_Type.Worker_Food:
                body = 10; emotion = 5;
                break;
            case Item_Type.Worker_Play:
                emotion = 5; study = -5;
                break;
            case Item_Type.Worker_Travel:
                family = 5; study = -5;
                break;
            case Item_Type.Worker_Work:
                study = 10; emotion = -1; family = -1;
                break;
            case Item_Type.Worker_Event_JiaBan:
                relation = 5; study = 2; family = -5;
                break;
            case Item_Type.Worker_Event_QingJiaZhang:
                family = 2; emotion = -5;
                break;
            case Item_Type.Worker_Event_AoYeGongZuo:
                study = 2; emotion = 5;
                break;
            case Item_Type.Worker_Event_ZhongCaiPiao:
                study = 10; emotion = 5; family = 5;
                break;
            case Item_Type.Worker_Event_JuCan:
                relation = 10; study = -2;
                break;
            #endregion
            #region elder
            case Item_Type.Elder_Event_Bed:
                body = -50;
                break;
            case Item_Type.Elder_Event_Tomb:
                body = -100;
                break;
                #endregion
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")//被玩家拾取
        {
            Record.Controller.AddRecord((int)TimeCtrl.GetYear(), type);
            if(emotion != 0)
                GameManager.Instance.add("Emotion", emotion);
            if(body != 0)
                GameManager.Instance.add("Body", body);
            if(study != 0)
                GameManager.Instance.add("Study", study);
            if(family != 0)
                GameManager.Instance.add("Family", family);
            if(relation != 0)
                GameManager.Instance.add("Relation", relation);
            
            CreateIcon();
            ShowPropUI();
            Destroy(gameObject);
        }
        if (collision.tag == "Outside")//超出上界后消失
        {
            Destroy(gameObject);
        }
    }

    private void CreateIcon()
    {
        GameObject icon = Instantiate(Icon);
        icon.transform.position = transform.position + Vector3.up * 0.5f;
        if(GetIconName() == null)
        {
            return;
        }
        Texture2D tex = Resources.Load("Icon/"+GetIconName()) as Texture2D;
        if(tex == null)
            Debug.LogError("need img for icon " + GetIconName());
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f,0.5f));
        icon.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private string GetIconName()
    {
        int max = 0;
        foreach(int i in new int[] { body, emotion, study, family, relation })
        {
            if (i > max) max = i;
        }
        if (max == 0) return null;
        if (max == body) return "body";
        else if (max == emotion) return "emotion";
        else if (max == study)
        {
            if (GameManager.Instance.gameState == Game_State.Youth)
            {
                return "study";
            }
            return "work";
        }
        else if (max == family) return "family";
        else if (max == relation) return "relation";
        return null;
    }

    // Update is called once per frame
    void Update()//旋转效果
    {
        transform.rotation = Quaternion.Euler(0, 0, A * Mathf.Sin(omega * Time.time));
    }

    private List<Sprite> GetItemImage(string name)
    {
        Object[] objects = Resources.LoadAll<Texture2D>("Item");
        List<Sprite> results = new List<Sprite>();
        foreach (Object ob in objects)
        {
            if (ob.name.Contains(name))
            {
                Debug.Log("ob name "+ob);
                Texture2D tex = ob as Texture2D;
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                new Vector2(0.5f, 0.5f));
                if (sprite == null)
                {
                    Debug.Log(name + " need img:" + ob.name);
                }
                results.Add(sprite);
            }
        }
        return results;
    }
    private void ShowPropUI()
    {
        MyUI.GetInstance().updateprop(this);
    }

    
}
