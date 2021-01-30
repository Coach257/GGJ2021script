using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*这个脚本的作用如下：
 * 1. 显示“体魄”“情绪”“健康”数字的UI（条在UI_Body和UI_Emotion里）
 */
public class UI_Image
{
    public string name;
    public Image BackgroundImage;
    public Image ValueImage;
    public UI_Image(Image backgroundimage, Image valueimage, string name)
    {
        this.BackgroundImage = backgroundimage;
        this.ValueImage = valueimage;
        this.name = name;
    }
    public void EnableUI()
    {
        this.BackgroundImage.enabled = true;
        this.ValueImage.enabled = true;
    }
    public void DisableUI()
    {
        this.BackgroundImage.enabled = false;
        this.ValueImage.enabled = false;
    }
    public void update()
    {
        UI_Value uI_Value = GameManager.Instance.UIValueList[this.name];
        this.ValueImage.fillAmount = ((float)uI_Value.nowvalue / uI_Value.maxvalue);
    }
}

public class MyUI : MonoBehaviour
{
    static public Dictionary<string, UI_Image> EnabledUI = new Dictionary<string, UI_Image>();
    static public Dictionary<string, UI_Image> AllUI = new Dictionary<string, UI_Image>();
    public Image propImage;
    public Text propText;
    public Image playerImage;
    public Text playerText;


    public void enable(string name)
    {
        UI_Image uI_Image = AllUI[name];
        uI_Image.EnableUI();
        EnabledUI.Add(name, uI_Image);
    }
    private static MyUI instance;
    public static MyUI GetInstance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Awake()
    {
        initAllUI();
        instance = this;
        GameManager.Instance.initGameManager();
        propImage = GameObject.Find("道具").GetComponent<Image>();
        propText = GameObject.Find("道具值").GetComponent<Text>();
        playerImage = GameObject.Find("playershow").GetComponent<Image>();
        playerText = GameObject.Find("playershowvalue").GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateImage();//血条更新
    }
    void UpdateImage()
    {
        foreach (KeyValuePair<string, UI_Image> kvp in EnabledUI)
        {
            kvp.Value.update();
        }
    }
    public void initAllUI()
    {
        AllUI.Clear();
        EnabledUI.Clear();
        AllUI.Add("Emotion", new UI_Image(GameObject.Find("Emotion背景").GetComponent<Image>(), GameObject.Find("Emotion值").GetComponent<Image>(), "Emotion"));
        AllUI.Add("Body", new UI_Image(GameObject.Find("HP背景").GetComponent<Image>(), GameObject.Find("HP值").GetComponent<Image>(), "Body"));
        AllUI.Add("Study", new UI_Image(GameObject.Find("Study背景").GetComponent<Image>(), GameObject.Find("Study值").GetComponent<Image>(), "Study")); //study是学业，也是事业
        AllUI.Add("Relation", new UI_Image(GameObject.Find("Relation背景").GetComponent<Image>(), GameObject.Find("Relation值").GetComponent<Image>(), "Relation"));
        AllUI.Add("Family", new UI_Image(GameObject.Find("Family背景").GetComponent<Image>(), GameObject.Find("Family值").GetComponent<Image>(), "Family"));
        foreach (KeyValuePair<string, UI_Image> kvp in AllUI)
        {
            kvp.Value.DisableUI();
        }
    }
    public void updateprop(item prop)
    {
        propImage.sprite = prop.ren.sprite;
        propText.text = prop.type.ToString();
    }
    public void updateplayer(Game_State game_state)
    {
        switch (game_state)
        {
            case Game_State.Kid:
                playerImage.sprite = Resources.Load("Player/BigRightStanding", typeof(Sprite)) as Sprite;
                playerText.text = "小屁孩";
                break;
            case Game_State.Youth:
                playerImage.sprite = Resources.Load("Player/BigRightStanding", typeof(Sprite)) as Sprite;
                playerText.text = "学生时期";
                break;
            case Game_State.Worker:
                playerImage.sprite = Resources.Load("Player/BigRightStanding", typeof(Sprite)) as Sprite;
                playerText.text = "上班族";
                break;
        }
    }

}
