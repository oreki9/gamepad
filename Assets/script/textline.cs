using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class textline : MonoBehaviour
{
    List<string> keyList = new List<string>();
    int keyListNow = 0;
    SaveKey keyCodeList;
    int colNow = 0;

    public List<GameObject> TextLine = new List<GameObject>();
    int TextLineNow = 0;
    bool[] verticalKeyBool = { false, false };
    bool[] HorizonKeyBool = { false, false };
    char SelectedChar;

    bool afterAnalog = false;
    string StringPredict = "";
    bool afterPress = false;
    void Start() {
        keyCodeList = LoadSaveKey();
        foreach(int[] i in keyCodeList.keyCodeArrayStr)
        {
            string NewStr = "";
            foreach (int o in i)
            {
                NewStr += (char)o;
            }
            keyList.Add(NewStr);
        }
        SelectedChar = '\0';
        foreach (GameObject i in TextLine) {
            i.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 30);
        }
    }
    public SaveKey LoadSaveKey()
    {
        SaveKey save = null;
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            save = (SaveKey)bf.Deserialize(file);
            file.Close();
        }
        return save;
    }
    void AddTextInLine(char Newchr)
    {
        string StrInLine = TextLine[TextLineNow].GetComponent<Text>().text;
        TextLine[TextLineNow].GetComponent<Text>().text = StrInLine + Newchr;
    }
    void ChanceTextInLine(string NewStr,char Newchr,int col)
    {
        string NewStr2 = NewStr;
        NewStr2 = NewStr2.Insert(col, Newchr.ToString());
        TextLine[TextLineNow].GetComponent<Text>().text = NewStr2;
    }
    void Update()
    {
        float vertikey = Input.GetAxis("Vertical");
        float Horikey = Input.GetAxis("Horizontal");
        if ((Mathf.Abs(vertikey) >= 0.5f) || (Mathf.Abs(Horikey) >= 0.5f)){
            if (!afterAnalog)
            {
                StringPredict = TextLine[TextLineNow].GetComponent<Text>().text;
            }
            afterAnalog = true;
            if ((vertikey >= 0.5f) && (Horikey >= 0.5f)){
                SelectedChar = keyList[keyListNow][1];
            }
            else if ((vertikey <= -0.5f) && (Horikey >= 0.5f)){
                SelectedChar = keyList[keyListNow][3];
            }
            else if ((vertikey >= 0.5f) && (Horikey <= -0.5f)){
                SelectedChar = keyList[keyListNow][7];
            }
            else if ((vertikey <= -0.5f) && (Horikey <= -0.5f)){
                SelectedChar = keyList[keyListNow][5];
            }
            else if (vertikey >= 0.5f){
                SelectedChar = keyList[keyListNow][0];
            }
            else if (vertikey <= -0.5f){
                SelectedChar = keyList[keyListNow][4];
            }
            else if (Horikey >= 0.5f){
                if (!afterPress)
                {
                    colNow += 1;
                }
                SelectedChar = keyList[keyListNow][2];
            }
            else if (Horikey <= -0.5f)
            {
                if (!afterPress)
                {
                    colNow -= 1;
                }
                SelectedChar = keyList[keyListNow][6];
            }
            if (colNow < 0)
            {
                colNow = 0;
            }
            if (colNow > StringPredict.Length)
            {
                colNow = StringPredict.Length;
            }
            if (afterPress)
            {
                ChanceTextInLine(StringPredict, SelectedChar, colNow);
            }
        }
        else
        {
            if (afterAnalog)
            {
                colNow += 1;
                afterAnalog = false;
            }
        }
        for (int i=0;i<keyCodeList.keyCodeStr.Count;i++)
        {
            keyListListen(SelectedChar, new eigthkey(keyCodeList.keyCodeStr[i], i));
        }
    }
    void keyListListen(char SelectedChar, eigthkey selKeyString)
    {
        if (Input.GetKeyDown(StringToKeyCode(selKeyString.keycodestr)))
        {
            afterPress = true;
            keyListNow = selKeyString.codekeyInt;
        }
        if (Input.GetKeyUp(StringToKeyCode(selKeyString.keycodestr)))
        {
            afterAnalog = false;
            colNow += 1;
            afterPress = false;
            keyListNow = 0;
        }
    }
    KeyCode StringToKeyCode(string strkey){
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), strkey);
    }
}
