using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;  //com.unity.nuget.newtonsoft-json 

public class GameManager : MonoBehaviour
{
    ////////////////////////////// 싱글톤
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            // 게임 매니저에서 관리하는 데이터용 함수들을 여기서 초기화
            JsonToDic("Json/GameData", ref _dataDictionary);
            JsonToDic("Json/String", ref StringDictionary);
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static GameManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
    /// 여기부터 싱글톤에서 관리하는 데이터
    /////////////////////////////////////////////////////////////////////////////////

    //Package Manager Window > 
    //Add Package from GIT URL > 
    //com.unity.nuget.newtonsoft-json

    //use using Newtonsoft.Json;
    //https://www.newtonsoft.com/json/help/html/Introduction.htm

    // 뉴턴으로 JSON 파싱해서 딕셔너리로 저장
    public class DataDictionary : Dictionary<string, Dictionary<string, Dictionary<string, string>>> { }

    private void JsonToDic(string jsonFilePath, ref DataDictionary JsonToDicDictionary)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(jsonFilePath);
        JsonToDicDictionary = JsonConvert.DeserializeObject<DataDictionary>(textAsset.text);
    }

    ////////////////////////////// 스크립트 Parser
    private static DataDictionary _dataDictionary;
    public static int GetStackableCount()
    {
        return _dataDictionary["Stackable"].Keys.Count; 
    }

    // 캐릭터와 장비 정보 딕셔너리를 구조체로 반환하는 구현
    public interface IInterfaceforCategory
    {
        // 카테고리는 공통 요소로 고정하고 스트링으로 설정해야, 구조체 파싱할때 코드가 어떤 딕셔너리를 찾을지 알 수 있음
        string Category { get;}
    }

    // 소모품 정보 구조체
    public struct StackableValueStruct : IInterfaceforCategory
    {
        public string Category { get { return "Stackable"; } }
        public string name;
        public string type;
        public int icon;
        public string explain;

        // 구조체 생성시 딕셔너리를 인수로 넣으면 호출되는 생성자
        public StackableValueStruct(Dictionary<string, string> dictionary)
        {
            // 이 메소드는 get set에 넣을 수 없으므로..
            SetValueFromDictionary(dictionary, "name", out this.name);
            SetValueFromDictionary(dictionary, "type", out this.type);
            SetValueFromDictionary(dictionary, "icon", out this.icon);
            SetValueFromDictionary(dictionary, "explain", out this.explain);
        }
    }

    // 구조체를 가지고 와서 딕셔너리의 내용을 담아서 내보냄
    public static bool GetContentsData<T>(int index, out T value) where T : struct, IInterfaceforCategory
    {
        // 먼저 선언해야 코드에서 카테고리를 알고 값을 받아옴.
        // 하지만 선언한 사실만 가지고는 Category를 구조체가 가지고 있는지 컴파일러가 알 수 없다. 
        // 그래서 Category가 존재한다고 알리기 위해 InterfaceforCategory에 Category를 넣어두고 상속받는것.
        
        // Value는 어떤 구조체를 다룰지 정하는 인수
        value = default;
        
        bool isSuccess = _dataDictionary[value.Category].TryGetValue(index.ToString(), out Dictionary<string, string> Datas);

        // T로는 구조체 생성자에 인자값을 넣을 수 없다.
        // 그래서 이 메소드를 사용해서 datas 딕셔너리를 생성자에 넣어준다.
        if (isSuccess) value = (T)Activator.CreateInstance(typeof(T), new object[] { Datas });

        return isSuccess;
    }
    
    // 딕셔너리의 Value값을 형변환하는 메소드들
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
    ////////////////////////////// 스트링 Parser
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

    ///////////////////////////////////////// 각종 이넘 타입을 스트링으로 전환해주는 부분
    // 아이템 타입
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

    // 각 타입에 대응되는 스트링을 가진 딕셔너리를 작성
    private void SetEnumString<T>(StringCategory stringCategory, out Dictionary<T, string> enumString) where T : Enum
    {

// 람다를 쓰라는 오류 나오지 않게 처리
#pragma warning disable IDE0039
        // StringCategory에 enum 타입들을 미리 등록해야 스트링에서 찾을 수 있음
        // 익명 메소드. emum의 구성원을 받으면 스트링 찾는 함수에 넣어서 스트링으로 돌려준다.
        Func<T, string> FindString = (enumMember) => {

            // enum은 string으로 할때는 .ToString()이지만, int로 갈때는 (int)(object)를 붙인다. int.parse()는 string을 int로 할때만..
            int stringIndex = (int)(object)enumMember;
            bool isSuccess = StringData(stringCategory, stringIndex, out string value);

            // 실패하면 다른 텍스트로 채워넣고, 로그 띄운다.
            if (!isSuccess)
            {
                value = "!Error";
                Debug.Log("Got Error while parsing " + enumMember.ToString() + " in " + stringCategory.ToString());
            }

            return value;
        };
#pragma warning disable IDE0039

        // 돌려줄 딕셔너리
        enumString = new Dictionary<T, string>();

        // enum의 멤버를 순회하면서 스트링을 찾은뒤, 돌려주는 딕셔너리에 넣음
        foreach (T enumMember in Enum.GetValues(typeof(T)))
        {
            enumString.Add(enumMember, FindString(enumMember));
        }
    }
    ///////////////////////////////////////// 씬 뒤로 가기 구현 위한 스택
    public static Stack<GameObject> SceneStack = new Stack<GameObject>();

    /// ////////////////////////////////////// 기타 편의
    // 탑바에 달기 위한 타이틀
    public static string PreGuiName = "";
}
