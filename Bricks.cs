using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*这个脚本的作用如下：
 * 1. 声明砖块的类型Brick_State
 * 3. 角色踩上砖块时的效果
 * 4. 砖块出上界时消除
 */

public enum Brick_State//砖块类型
{
    None,//普通，红色
    Fragile,//接触后会变透明
}

public class Bricks : MonoBehaviour
{

    public Brick_State state = Brick_State.None;

    SpriteRenderer ren;
    BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();

        switch (state)//砖块类型
        {
            case Brick_State.None:
                break;
            case Brick_State.Fragile:
                ren.color = new Color32(29, 231, 201, 255);
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] arr = collision.contacts;
        if(arr[0].normal == Vector2.down)
            switch (state)
            {
                case (Brick_State.None):
                    break;
                case (Brick_State.Fragile):
                    ren.color = new Color32(29, 231, 201, 70);
                    box.enabled = false;
                    Invoke("recover", 0.5f);//恢复
                    break;
            }
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Outside")
        {
            Destroy(gameObject);
        }
    }

    private void recover()
    {
        ren.color = new Color32(29, 231, 201, 255);
        box.enabled = true;
    }
}
