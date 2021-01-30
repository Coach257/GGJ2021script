using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAnimation : MonoBehaviour
{
    Material mat;
    public float speed = 5;//初始移动速度
    float real_speed = 0;//考虑到加速后的移动速度
    float dtY = 0;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        real_speed = speed * GameManager.Instance.gameSpeed;
        dtY += Time.deltaTime*speed;
        mat.SetTextureOffset("_MainTex", new Vector2(0, dtY));
    }
}
