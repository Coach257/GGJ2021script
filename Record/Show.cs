using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Record
{
    public class Show : MonoBehaviour
    {
        public GameObject ShowItem;
        public Transform LeftPos;
        public Transform RightPos;

        private GameObject canvas;
        private List<GameObject> showItems = new List<GameObject>();
        private List<Record> records;
        private int count = 0;

        private void Awake()
        {
            canvas = GameObject.Find("Canvas");

        }

        void Start()
        {
            records = Controller.GetRecords();
            InvokeRepeating("createItem", 0, 2);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.init(Game_State.Kid);//初始化至Kid状态
                SceneManager.LoadScene(0);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }


        private void createItem()
        {
            // 边界条件
            if(count >= records.Count)
            {
                CancelInvoke();
                Invoke("Stop", 2);
                return;
            }

            Record record = records[records.Count - 1 - count];
            GameObject item = Instantiate(ShowItem,canvas.transform);

            // 修改位置
            if (count % 2 == 0)
                item.transform.position = LeftPos.position;
            else
                item.transform.position = RightPos.position;

            // 设置年龄和图片
            item.GetComponent<ShowItem>().SetAge(record.Year);
            item.GetComponent<ShowItem>().SetImage(record.Text);
            showItems.Add(item);
            count++;

        }

        private void Stop()
        {
            foreach(GameObject item in showItems)
            {
                item.GetComponent<ShowItem>().Stop();
            }
        }
    }
}

