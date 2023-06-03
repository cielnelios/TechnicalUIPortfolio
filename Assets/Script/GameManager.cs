using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;  //com.unity.nuget.newtonsoft-json 

public class GameManager : MonoBehaviour
{
    ////////////////////////////// �̱���
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            // ���� �Ŵ������� �����ϴ� �����Ϳ� �Լ����� ���⼭ �ʱ�ȭ
            JsonToDic("Json/GameData", ref _dataDictionary);
            JsonToDic("Json/String", ref StringDictionary);
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
    }

    // �̱��� ������ ����ϱ� ���� �ν��Ͻ� ����
    private static GameManager _instance;
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static GameManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    /////////////////////////////////////////////////////////////////////////////////
    /// ������� �̱��濡�� �����ϴ� ������
    /////////////////////////////////////////////////////////////////////////////////

    //Package Manager Window > 
    //Add Package from GIT URL > 
    //com.unity.nuget.newtonsoft-json

    //use using Newtonsoft.Json;
    //https://www.newtonsoft.com/json/help/html/Introduction.htm

    // �������� JSON �Ľ��ؼ� ��ųʸ��� ����
    public class DataDictionary : Dictionary<string, Dictionary<string, Dictionary<string, string>>> { }

    private void JsonToDic(string jsonFilePath, ref DataDictionary JsonToDicDictionary)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(jsonFilePath);
        JsonToDicDictionary = JsonConvert.DeserializeObject<DataDictionary>(textAsset.text);
    }

    ////////////////////////////// ��ũ��Ʈ Parser
    private static DataDictionary _dataDictionary;
    public static int GetStackableCount()
    {
        return _dataDictionary["Stackable"].Keys.Count; 
    }

    // ĳ���Ϳ� ��� ���� ��ųʸ��� ����ü�� ��ȯ�ϴ� ����
    public interface IInterfaceforCategory
    {
        // ī�װ��� ���� ��ҷ� �����ϰ� ��Ʈ������ �����ؾ�, ����ü �Ľ��Ҷ� �ڵ尡 � ��ųʸ��� ã���� �� �� ����
        string Category { get;}
    }

    // �Ҹ�ǰ ���� ����ü
    public struct StackableValueStruct : IInterfaceforCategory
    {
        public string Category { get { return "Stackable"; } }
        public string name;
        public string type;
        public int icon;
        public string explain;

        // ����ü ������ ��ųʸ��� �μ��� ������ ȣ��Ǵ� ������
        public StackableValueStruct(Dictionary<string, string> dictionary)
        {
            // �� �޼ҵ�� get set�� ���� �� �����Ƿ�..
            SetValueFromDictionary(dictionary, "name", out this.name);
            SetValueFromDictionary(dictionary, "type", out this.type);
            SetValueFromDictionary(dictionary, "icon", out this.icon);
            SetValueFromDictionary(dictionary, "explain", out this.explain);
        }
    }

    // ����ü�� ������ �ͼ� ��ųʸ��� ������ ��Ƽ� ������
    public static bool GetContentsData<T>(int index, out T value) where T : struct, IInterfaceforCategory
    {
        // ���� �����ؾ� �ڵ忡�� ī�װ��� �˰� ���� �޾ƿ�.
        // ������ ������ ��Ǹ� ������� Category�� ����ü�� ������ �ִ��� �����Ϸ��� �� �� ����. 
        // �׷��� Category�� �����Ѵٰ� �˸��� ���� InterfaceforCategory�� Category�� �־�ΰ� ��ӹ޴°�.
        
        // Value�� � ����ü�� �ٷ��� ���ϴ� �μ�
        value = default;
        
        bool isSuccess = _dataDictionary[value.Category].TryGetValue(index.ToString(), out Dictionary<string, string> Datas);

        // T�δ� ����ü �����ڿ� ���ڰ��� ���� �� ����.
        // �׷��� �� �޼ҵ带 ����ؼ� datas ��ųʸ��� �����ڿ� �־��ش�.
        if (isSuccess) value = (T)Activator.CreateInstance(typeof(T), new object[] { Datas });

        return isSuccess;
    }
    
    // ��ųʸ��� Value���� ����ȯ�ϴ� �޼ҵ��
    private static void SetValueFromDictionary(Dictionary<string, string> dictionary, string Key, out string value)
    {
        dictionary.TryGetValue(Key, out value);
    }
    private static void SetValueFromDictionary(Dictionary<string, string> dictionary, string Key, out int value)
    {
        dictionary.TryGetValue(Key, out string stringValue);
        value = int.Parse(stringValue);
    }
    private static void SetValueFromDictionary(Dictionary<string, string> dictionary, string Key, out List<int> value)
    {
        dictionary.TryGetValue(Key, out string stringValue);
        value = stringValue.Split(',').Select(int.Parse).ToList(); 
    }
    ////////////////////////////// ��Ʈ�� Parser
    private static DataDictionary StringDictionary;

    public enum StringCategory
    {
        ItemName,
        ItemExplain,
    }

    public static bool StringData(StringCategory category, int index, out string value)
    {
        Dictionary<string, string> Datas = StringDictionary["StringData"][category.ToString()];
        bool isSuccess = Datas.TryGetValue(index.ToString(), out value);

        if (!isSuccess) value = "<color=#FF0000>string N.A</color>";

        return isSuccess;
    }

    public static bool StringData(string stringToken, out string value)
    {
        string[] splitStr = { "::" };
        string[] stringTokenSplit = stringToken.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

        //GameManager.StringCategory category = (GameManager.StringCategory)System.Enum.Parse(typeof(GameManager.StringCategory), stringTokenSplit[0]);
        string category = stringTokenSplit[0];
        string index = stringTokenSplit[1];

        Dictionary<string, string> Datas = StringDictionary["StringData"][category];
        bool isSuccess = Datas.TryGetValue(index, out value);

        if (!isSuccess) value = "<color=#FF0000>string N.A</color>";

        return isSuccess;
    }

    public static void StringDataLength(StringCategory category, out int value)
    {
        Dictionary<string, string> Datas = StringDictionary["StringData"][category.ToString()];

        value = Datas.Count;
    }

    ///////////////////////////////////////// ���� �̳� Ÿ���� ��Ʈ������ ��ȯ���ִ� �κ�
    // ������ Ÿ��
    public enum ItemType
    {
        Sword,
        Gun = 100,
        Rifle,
        HandGun,
        Bow,
        Explosive,
        HeavyWeapon,
        Potion = 200,
        Food,
    }
    public static Dictionary<ItemType, string> StatusItemType;
    public static bool GetStatusItemType(ItemType itemType, out string value)
    {
        bool isSuccess = StatusItemType.TryGetValue(itemType, out value);
        if (!isSuccess) value = "<color=#FF0000>string N.A</color>";
        return isSuccess;
    }

    public static Dictionary<ItemType, int> ItemTypeIconIndex = new Dictionary<ItemType, int>()
    {
        { ItemType.Sword, 56 },
        { ItemType.Gun, 53 },
        { ItemType.Rifle, 25 },
        { ItemType.HandGun, 53 },
        { ItemType.Bow, 52 },
        { ItemType.Explosive, 51 },
        { ItemType.HeavyWeapon, 39 },
        { ItemType.Potion, 22 },
        { ItemType.Food, 24 },
    };
    public static bool GetItemTypeIconIndex(ItemType itemType, out int value)
    {
        bool isSuccess = ItemTypeIconIndex.TryGetValue(itemType, out value);
        if (!isSuccess) value = -1;
        return isSuccess;
    }

    // �� Ÿ�Կ� �����Ǵ� ��Ʈ���� ���� ��ųʸ��� �ۼ�
    private void SetEnumString<T>(StringCategory stringCategory, out Dictionary<T, string> enumString) where T : Enum
    {

// ���ٸ� ����� ���� ������ �ʰ� ó��
#pragma warning disable IDE0039
        // StringCategory�� enum Ÿ�Ե��� �̸� ����ؾ� ��Ʈ������ ã�� �� ����
        // �͸� �޼ҵ�. emum�� �������� ������ ��Ʈ�� ã�� �Լ��� �־ ��Ʈ������ �����ش�.
        Func<T, string> FindString = (enumMember) => {

            // enum�� string���� �Ҷ��� .ToString()������, int�� ������ (int)(object)�� ���δ�. int.parse()�� string�� int�� �Ҷ���..
            int stringIndex = (int)(object)enumMember;
            bool isSuccess = StringData(stringCategory, stringIndex, out string value);

            // �����ϸ� �ٸ� �ؽ�Ʈ�� ä���ְ�, �α� ����.
            if (!isSuccess)
            {
                value = "!Error";
                Debug.Log("Got Error while parsing " + enumMember.ToString() + " in " + stringCategory.ToString());
            }

            return value;
        };
#pragma warning disable IDE0039

        // ������ ��ųʸ�
        enumString = new Dictionary<T, string>();

        // enum�� ����� ��ȸ�ϸ鼭 ��Ʈ���� ã����, �����ִ� ��ųʸ��� ����
        foreach (T enumMember in Enum.GetValues(typeof(T)))
        {
            enumString.Add(enumMember, FindString(enumMember));
        }
    }
    ///////////////////////////////////////// �� �ڷ� ���� ���� ���� ����
    public static Stack<GameObject> SceneStack = new Stack<GameObject>();

    /// ////////////////////////////////////// ��Ÿ ����
    // ž�ٿ� �ޱ� ���� Ÿ��Ʋ
    public static string PreGuiName = "";
}
