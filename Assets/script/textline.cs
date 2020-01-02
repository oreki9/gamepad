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

    public List<GameObject> TextLine = new List<GameObject>();
    int TextLineNow = 0;
    bool[] verticalKeyBool = { false, false };
    bool[] HorizonKeyBool = { false, false };
    char SelectedChar;


    void Start() {
        keyCodeList = LoadSaveKey();
        foreach(string i in keyCodeList.keyCodeArrayStr)
        {
            keyList.Add(i);
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
    void Update()
    {
        float vertikey = Input.GetAxis("Vertical");
        float Horikey = Input.GetAxis("Horizontal");
        if (keyListNow > 0)
        {
            if ((Mathf.Abs(vertikey) >= 0.5f) || (Mathf.Abs(Horikey) >= 0.5f))
            {
                if ((vertikey >= 0.5f) && (Horikey >= 0.5f))
                {
                    SelectedChar = keyList[keyListNow - 1][1];
                }
                else if ((vertikey <= -0.5f) && (Horikey >= 0.5f))
                {
                    SelectedChar = keyList[keyListNow - 1][3];
                }
                else if ((vertikey >= 0.5f) && (Horikey <= -0.5f))
                {
                    SelectedChar = keyList[keyListNow - 1][7];
                }
                else if ((vertikey <= -0.5f) && (Horikey <= -0.5f))
                {
                    SelectedChar = keyList[keyListNow - 1][5];
                }
                else if (vertikey >= 0.5f)
                {
                    SelectedChar = keyList[keyListNow - 1][0];
                }
                else if (vertikey <= -0.5f)
                {
                    SelectedChar = keyList[keyListNow - 1][4];
                }
                else if (Horikey >= 0.5f)
                {
                    SelectedChar = keyList[keyListNow - 1][2];
                }
                else if (Horikey <= -0.5f)
                {
                    SelectedChar = keyList[keyListNow - 1][6];
                }
            }
        }
        for(int i=0;i<keyCodeList.keyCodeStr.Count;i++)
        {
            keyListListen(SelectedChar, new eigthkey(keyCodeList.keyCodeStr[i], i+1));
        }
    }
    void keyListListen(char SelectedChar, eigthkey selKeyString)
    {
        if (Input.GetKeyDown(StringToKeyCode(selKeyString.keycodestr)))
        {
            keyListNow = selKeyString.codekeyInt;
        }
        if (Input.GetKeyUp(StringToKeyCode(selKeyString.keycodestr)))
        {
            if (SelectedChar != '\0')
            {
                AddTextInLine(SelectedChar);
            }
            keyListNow = 0;
        }
    }
    KeyCode StringToKeyCode(string strkey){
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), strkey);
    }
}
