using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Record
{
    /*
     * 记录的管理
     * 
     * 两种添加记录的方法
     * 1.输入时间和接触物体的类型Record.Controller.AddRecord(year, item_type);
     * 2.输入时间和图片的名称Record.Controller.AddRecord(year,text);
     * 
     */
    public class Controller 
    {
        static private List<Record> records = new List<Record>();
        static private List<Item_Type> firstTouchItems = new List<Item_Type>();

        
        static public void AddRecord(int year,string text)
        {
            records.Add(new Record(year, text));
        }

        static public void AddRecord(int year, Item_Type item)
        {
            //记录第一次接触
            if (firstTouchItems.Contains(item))
            {
                return;
            }
            firstTouchItems.Add(item);

            records.Add(new Record(year, getText(item)));
        }

        static public List<Record> GetRecords()
        {
            if(records.Count == 0)
            {
                AddRecord(12,Item_Type.Kid_Food_Milk);
                AddRecord(20, Item_Type.Kid_Toy_Big);
                AddRecord(39, Item_Type.Kid_Food_VC);
            }
            return records;
        }

        static private string getText(Item_Type item)
        {
            return item.ToString();
            //return "Kid_Food_Milk";
        }
    }
}


