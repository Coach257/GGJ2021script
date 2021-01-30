using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItem : MonoBehaviour
{
    private float moveSpeed = 1f;
    private Transform ageText0,ageText1,ageText2;

    private string NUMBER_PATH = "Record/Number/";
    private string IMG_PATH = "Record/";
    private void Awake()
    {
        ageText0 = transform.Find("age0");
        ageText1 = transform.Find("age1");
        ageText2 = transform.Find("age2");
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime * 60);
    }

    public void SetAge(int year)
    {
        LoadImage(NUMBER_PATH,(year % 10).ToString(), ageText0.gameObject);
        if(year >= 10)
        {
            LoadImage(NUMBER_PATH, (year/10%10).ToString(), ageText1.gameObject);
        }
        if(year >= 100)
        {
            LoadImage(NUMBER_PATH, (year / 100 % 10).ToString(), ageText2.gameObject);
        }
    }

    public void SetImage(string name)
    {
        LoadImage(IMG_PATH, name, gameObject);
    }

    public void Stop()
    {
        moveSpeed = 0;
    }

    private void LoadImage(string path, string name,GameObject gameObject)
    {
        Texture texture = Resources.Load(path + name) as Texture;
        Debug.Log("load image " + name + " tex:" + texture);
        gameObject.GetComponent<RawImage>().texture = texture;
    }
}
