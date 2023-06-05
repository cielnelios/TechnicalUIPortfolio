using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    ////////////////////////////// 싱글톤
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            this.MakeDictionary();
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

    /////////////////////////////////////////////////////////////////////////////////
    /// 열린 UI 목록을 관리
    /////////////////////////////////////////////////////////////////////////////////

    private Stack<Scene> _uiList = new Stack<Scene>();
    // 씬을 스택에 추가
    public void AddUIScene(Scene scene)
    {
        this._uiList.Push(scene);
        //Debug.Log("add " + this._uiList.Peek().name);
    }

    // 씬을 스택에서 뽑아서 닫음. 마지막 씬이므로 가장 마지막에 연 UI임
    public void DelUIScene()
    {
        if (this._uiList.Count > 0)
        {
            Scene UISceneToBeDeleted = this._uiList.Pop();
            //Debug.Log("del " + UISceneToBeDeleted.name);
            //SceneManager.UnloadSceneAsync(UISceneToBeDeleted);    // 씬을 없애는 코드

            // UI 마스터만 숨기도록 하는 대체 구현. 이 포폴에서 UI 생성 내역을 serialize하기에는 번거롭다.
            //MoveScene.MoveSceneMethod()의 추가 구현도 한세트임
            foreach (GameObject gameObject in UISceneToBeDeleted.GetRootGameObjects())
            {
                //Debug.Log(gameObject.name);
                if (gameObject.CompareTag("UIMaster"))
                {
                    gameObject.SetActive(false);
                    break;  
                }
            }
        }
    }

    // player input 대응
    public void DelUIScene(InputAction.CallbackContext context)
    {
        this.DelUIScene();
    }

    /////////////////////////////////////////////////////////////////////////////////
    /// 오브젝트에 컴포넌트를 장착하는 구현
    /////////////////////////////////////////////////////////////////////////////////

    public enum EnumComponentStrategy
    {
        setColorRed,
        setSpin,
        SetTall,
        SetFat,
    }

    public Dictionary<EnumComponentStrategy, IComponentStrategy> IComponentStrategyDictionary
        = new Dictionary<EnumComponentStrategy, IComponentStrategy>();

    // Awake에서 실행
    private void MakeDictionary()
    {
        // 딕셔너리에 패턴을 추가한다.
        // 이 패턴은 이미 매니저에 붙은 인스턴스이며, 실제로 배정되지 않음. context가 설정할거임
        IComponentStrategyDictionary.Add(EnumComponentStrategy.setColorRed, this.gameObject.AddComponent<SetColorRed>());
        IComponentStrategyDictionary.Add(EnumComponentStrategy.setSpin, this.gameObject.AddComponent<SetSpin>());
        IComponentStrategyDictionary.Add(EnumComponentStrategy.SetTall, this.gameObject.AddComponent<SetTall>());
        IComponentStrategyDictionary.Add(EnumComponentStrategy.SetFat, this.gameObject.AddComponent<SetFat>());
    }
}

// 전략들이 공유하는 인터페이스
public interface IComponentStrategy
{
    void EnterStrategy(GameObject targetObject);
    void ExitStrategy();
}

// 전략을 배정하고, 그 배정된 내역을 관리하기 위해 각각의 고객 오브젝트에 붙는 집사
public class ComponentStrategySet
{
    // 현재 선택된 전략을 담는 변수
    public IComponentStrategy CurrentComponentStrategy { get; private set; }

    // 전략을 배치하는 곳에서 전략을 실제로 고르는 부분
    public ComponentStrategySet(GameManager.EnumComponentStrategy enumComponentStrategy, GameObject targetObject)
    {
        // enum으로 전략을 가져온다.
        GameManager.Instance.IComponentStrategyDictionary.TryGetValue(enumComponentStrategy, out IComponentStrategy componentStrategy);

        // enum으로 고른 전략 클래스 컴포넌트를 배정해주고,
        System.Type type = componentStrategy.GetType();
        targetObject.AddComponent(type);

        // 선택된 전략 클래스를 컨텍스트에서 기록하고, 고객 게임 오브젝트가 질의할때 알려주는 용도로 사용.
        targetObject.TryGetComponent(type, out Component component);
        CurrentComponentStrategy = (IComponentStrategy)component;

        // 오브젝트가 enum으로 고른 전략 클래스를 배정해주고, 전략 클래스에게 어떤 게임 오브젝트가 물렸는지 전달한다.
        CurrentComponentStrategy.EnterStrategy(targetObject);
    }

    // 컴포넌트 넣는 캐릭터 프리팹에 좌표 정보, 넘버링, 그리고 그리드 형태로 확장 가능한 디자인으로 수정
}

public class SetColorRed : MonoBehaviour, IComponentStrategy
{
    private MeshRenderer _thisComponent;
    public void EnterStrategy(GameObject targetObject)
    {
        if (!targetObject.TryGetComponent<MeshRenderer>(out _thisComponent))
        {
            _thisComponent = targetObject.AddComponent<MeshRenderer>();
        }

        _thisComponent.material.color = Color.red;
    }

    public void ExitStrategy()
    {
        Destroy(this);
        _thisComponent.material.color = Color.white;
    }
}

public class SetTall : MonoBehaviour, IComponentStrategy
{
    private float _tall = 3;

    private Transform _thisComponent;
    public void EnterStrategy(GameObject targetObject)
    {
        if (!targetObject.TryGetComponent<Transform>(out _thisComponent))
        {
            _thisComponent = targetObject.AddComponent<Transform>();
        }

        _thisComponent.localScale = new Vector3(1, _tall, 1);
    }

    public void ExitStrategy()
    {
        Destroy(this);
        _thisComponent.localScale = Vector3.one;
    }
}

public class SetFat : MonoBehaviour, IComponentStrategy
{
    private float _fat = 3;

    private Transform _thisComponent;
    public void EnterStrategy(GameObject targetObject)
    {
        if (!targetObject.TryGetComponent<Transform>(out _thisComponent))
        {
            _thisComponent = targetObject.AddComponent<Transform>();
        }

        _thisComponent.localScale = new Vector3(_fat, 1, _fat);
    }

    public void ExitStrategy()
    {
        Destroy(this);
        _thisComponent.localScale = Vector3.one;
    }
}

public class SetSpin : MonoBehaviour, IComponentStrategy
{
    private void Awake()
    {
        // FixedUpdate를 막기 위해
        this.enabled = false;
    }
    private float _spinSpeed = 30;

    private Rigidbody _thisComponent;
    public void EnterStrategy(GameObject targetObject)
    {
        if (!targetObject.TryGetComponent<Rigidbody>(out _thisComponent))
        {
            _thisComponent = targetObject.AddComponent<Rigidbody>();
        }
        _thisComponent.isKinematic = true;

        this.enabled = true;
    }

    private void FixedUpdate()
    {
        Vector3 turn = Vector3.up * _spinSpeed * Time.deltaTime;
        _thisComponent.rotation *= Quaternion.Euler(turn);
    }

    public void ExitStrategy()
    {
        Destroy(this);
        _thisComponent.isKinematic = false;
    }
}
