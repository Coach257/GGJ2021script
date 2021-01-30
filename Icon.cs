using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    SpriteRenderer sprite;

    bool startDisappear = false;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        Invoke("Destroy", 2f);
        Invoke("Disappear", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * 1*Time.deltaTime);
        if(startDisappear)
            sprite.color = new Color(1, 1, 1, sprite.color.a - 0.01f);
    }

    void Disappear()
    {
        startDisappear = true;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
