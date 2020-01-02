using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class KeySetting : MonoBehaviour
{
    public bool newKey = false;
    public List<GameObject> ButtonCodeList;
    GameObject ButtonCodeNow;
    void Start()
    {
        ButtonCodeNow = ButtonCodeList[0];
        for(int i = 0; i < ButtonCodeList.Count; i++)
        {
            GameObject AddEventGO = ButtonCodeList[i];
            AddEventGO.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { NewKeySwitch(AddEventGO); });
        }
    }
    void OnGUI(){
        if (newKey)
        {
            if (Input.anyKeyDown)
            {
                Event e = Event.current;
                string GetkeyCode = e.keyCode.ToString();
                if (GetkeyCode != "None")
                {
                    if (e.isKey)
                    {
                        ButtonCodeNow.transform.Find("Button").transform.GetChild(0).GetComponent<Text>().text = GetkeyCode;
                    }
                }
            }
        }
	}
    public void NewKeySwitch(GameObject newBtnCodeNow)
    {
        if (newKey)
        {
            newKey = false;
        }
        else
        {
            ButtonCodeNow = newBtnCodeNow;
            newKey = true;
        }
    }
    SaveKey CreateSaveKey()
    {
        SaveKey save = new SaveKey();
        foreach(GameObject i in ButtonCodeList)
        {
            save.keyCodeStr.Add(i.transform.Find("Button").transform.GetChild(0).GetComponent<Text>().text);
            save.keyCodeArrayStr.Add(i.transform.Find("Text").GetComponent<Text>().text);
        }
        return save;
    }
    public void SetSaveKey()
    {
        SaveKey save = CreateSaveKey();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
    }
}
