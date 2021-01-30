using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
