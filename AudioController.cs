using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*这个脚本的作用如下：
 * 1. 游戏结束时关闭音乐
 */

public class AudioController : MonoBehaviour
{
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver == true)
            source.enabled = false;
        source.pitch = GameManager.Instance.gameSpeed;
    }
    public void change()
    {
        if (source.mute)
        {
            source.mute = false;
            GameObject.Find("Loud").GetComponent<Image>().sprite = Resources.Load("UI/声音", typeof(Sprite)) as Sprite;
        }
        else
        {
            source.mute = true;
            GameObject.Find("Loud").GetComponent<Image>().sprite = Resources.Load("UI/静音", typeof(Sprite)) as Sprite;
        }
    }
}
