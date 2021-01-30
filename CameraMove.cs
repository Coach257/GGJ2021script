using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*这个脚本的作用如下：
 * 1. 相机移动
 * 2. 生成砖块及道具
 */

public class CameraMove : MonoBehaviour
{
    public AudioSource audSou;//控制音乐
    public float moveSpeed = 3;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isGameOver)
        {
            transform.Translate(new Vector2(0, -moveSpeed * Time.deltaTime * GameManager.Instance.gameSpeed));
            
        }
       
    }

    public void GameOver()
    {
        audSou.enabled = false;
    }
}
