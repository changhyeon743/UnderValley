using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public enum CasinoType { EasyCoin, MiddleCoin }
public enum HeadType { general, android , antenna, tree, chicken, earth, alien, coke, horse, small, pirates, alpaca }
public enum BodyType { general, android, ant, bee, snowman, penguin, coke, stick, snail, larva, tank, alpaca }
public enum Items { none, stone, coal, iron, silver, amethyst, gold, sapphire, ruby, emerald, diamond, fossil ,meteorite }

[System.Serializable]
public class Saveable {
    public float Time = 0;
    public int money = 0;
    public int crystal;
    public int smart = 10;
    public int tired;
    public int hungry;
    public int bankMoney;
    public CompanyRank companyRank;
    public string currentContent;
    public bool DirectionalLight = true;
    public float LightIntensity;
    public XYZClass playerXYZ;
    public HeadType head;
    public BodyType body;
    public float SlimeWeight;
    public Dictionary<int,int> item = new Dictionary<int,int>();
}

[System.Serializable]
public class ItemClass {
    public Items itemType;
    public int itemCount;
}


[System.Serializable]
public class Casino {
    public int betMoney;
    public CasinoType casinoType;
}


[System.Serializable]
public class XYZClass {
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class PlayerSkinClass {
    public HumanType bodyType;
    public XYZClass xyz;
    public Mesh mesh;
    public Texture texture;
    public bool isExists = true;
}

[System.Serializable]
public class PlayerHeadClass{
    public HeadType headType;
    public PlayerSkinClass playerHeadInfo;
}

[System.Serializable]
public class PlayerBodyClass {
    public BodyType bodyType;
    public List<PlayerSkinClass> playerBodyInfo;
}

public class DataController : MonoBehaviour {

	public static DataController Instance;
	public Saveable saveable;
    public Casino casino;
    public List<PlayerHeadClass> playerhead;
    public List<PlayerBodyClass> playerbody;

	void Awake() {
		if (Instance == null) {
			DontDestroyOnLoad(this);
			Instance = this;
		}
        StartCoroutine(FirstLoad());
	}

    IEnumerator FirstLoad() {
        yield return new WaitForSeconds(0.1f);
        if (File.Exists(Global.filePath)) {
            DataController.Instance.saveable = DataController.LoadDatas(Global.filePath);
            Global.gameController.global.PlayerObj.transform.position = new Vector3(DataController.Instance.saveable.playerXYZ.x,DataController.Instance.saveable.playerXYZ.y,DataController.Instance.saveable.playerXYZ.z);
        }
    }


	 public static void SaveDatas(Saveable save, string filePath) { 
        BinaryFormatter formatter = new BinaryFormatter(); 
        FileStream stream = new FileStream(filePath, FileMode.Create); 
        formatter.Serialize(stream, save); 
        stream.Close(); 
        Debug.Log("Saved");
    }

    public static Saveable LoadDatas(string filePath) { 
        BinaryFormatter formatter = new BinaryFormatter(); 
        FileStream stream = new FileStream(filePath, FileMode.Open); 
        Saveable data = (Saveable)formatter.Deserialize(stream); 
        stream.Close(); 
        Debug.Log("Loaded");
        return data;
        
    }
}
