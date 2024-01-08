using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    ////////////////////////////// �̱���
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            // ���⼭ �ʱ�ȭ
            this.MakeDictionary();
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

    /////////////////////////////////////////////////////////////////////////////////
    /// ���� UI ����� ����
    /////////////////////////////////////////////////////////////////////////////////

    private Stack<Scene> _uiList = new Stack<Scene>();
    // ���� ���ÿ� �߰�
    public void AddUIScene(Scene scene)
    {
        this._uiList.Push(scene);
        //Debug.Log("add " + this._uiList.Peek().name);
    }

    // ���� ���ÿ��� �̾Ƽ� ����. ������ ���̹Ƿ� ���� �������� �� UI��
    public void DelUIScene()
    {
        if (this._uiList.Count > 0)
        {
            Scene UISceneToBeDeleted = this._uiList.Pop();
            //Debug.Log("del " + UISceneToBeDeleted.name);
            //SceneManager.UnloadSceneAsync(UISceneToBeDeleted);    // ���� ���ִ� �ڵ�

            // UI �����͸� ���⵵�� �ϴ� ��ü ����. �� �������� UI ���� ������ serialize�ϱ⿡�� ���ŷӴ�.
            //MoveScene.MoveSceneMethod()�� �߰� ������ �Ѽ�Ʈ��
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

    // player input ����
    public void DelUIScene(InputAction.CallbackContext context)
    {
        this.DelUIScene();
    }

    /////////////////////////////////////////////////////////////////////////////////
    /// ������Ʈ�� ������Ʈ�� �����ϴ� ����
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

    // Awake���� ����
    private void MakeDictionary()
    {
        // ��ųʸ��� ���� �ν��Ͻ��� �߰��Ѵ�.
        // �� ������ �̹� ���� �Ŵ����� ������Ʈ�� �پ���, ���� ������ ������ ������Ʈ�鿡�Դ� ������ �������� ����.
        IComponentStrategyDictionary.Add(EnumComponentStrategy.setColorRed, this.gameObject.AddComponent<SetColorRed>());
        IComponentStrategyDictionary.Add(EnumComponentStrategy.setSpin, this.gameObject.AddComponent<SetSpin>());
        IComponentStrategyDictionary.Add(EnumComponentStrategy.SetTall, this.gameObject.AddComponent<SetTall>());
        IComponentStrategyDictionary.Add(EnumComponentStrategy.SetFat, this.gameObject.AddComponent<SetFat>());
    }

    // ĳ���� ���Կ��� ĳ���Ͱ� ���� ���� ����� ����ϴ� ��ųʸ��� ������, ������ ���¸� ��ųʸ������� ���ִ� �Լ�
    public void RemoveKeyInChracterSlot(IComponentStrategy myClass, CharacterComponentInfoPrefab characterComponentInfoPrefab)
    {
        // ĳ���� ������ �Լ��� �����´�.
        Dictionary<GameObject, IComponentStrategy> strategyDic = characterComponentInfoPrefab.StrategyInSlotDictionary;

        // �Լ�����
        foreach (KeyValuePair<GameObject, IComponentStrategy> strategy in strategyDic)
        {
            if (strategy.Value == null)
            {
                // ��üũ ȸ��
                continue;
            }
            //��ųʸ����� ���� ���� ����� ã�Ƽ� Ű������ ����
            else if (myClass.GetType() == strategy.Value.GetType())
            {
                strategyDic.Remove(strategy.Key);
                break;
            }
        }
    }
}

// ���µ��� �����ϴ� �������̽�
public interface IComponentStrategy
{
    void EnterStrategy(GameObject targetObject, CharacterComponentInfoPrefab characterComponentInfoPrefab);
    void ExitStrategy();
}

// ���¸� �����ϰ�, �� ������ ������ �����ϱ� ���� ������ �� ������Ʈ�� �ٴ� ������ context �Լ�
public class ComponentStrategySet
{
    // ���� ���õ� ���¸� ��� ����
    public IComponentStrategy CurrentComponentStrategy { get; private set; }

    // ���¸� ��ġ�ϴ� ������ ���¸� ������ ���� �κ�
    public ComponentStrategySet(GameManager.EnumComponentStrategy enumComponentStrategy, GameObject targetObject, CharacterComponentInfoPrefab characterComponentInfoPrefab)
    {
        // enum���� ���¸� �����´�.
        GameManager.Instance.IComponentStrategyDictionary.TryGetValue(enumComponentStrategy, out IComponentStrategy componentStrategy);

        // enum���� �� ���� Ŭ���� ������Ʈ�� �������ְ�,
        System.Type type = componentStrategy.GetType();
        targetObject.AddComponent(type);

        // ���õ� ���� Ŭ������ ���ؽ�Ʈ���� ����ϰ�, �� ���� ������Ʈ�� �����Ҷ� �˷��ִ� �뵵�� ���.
        targetObject.TryGetComponent(type, out Component component);
        CurrentComponentStrategy = (IComponentStrategy)component;

        // ������Ʈ�� enum���� �� ���� Ŭ������ �������ְ�, ���� Ŭ�������� � ���� ������Ʈ�� ���ȴ��� �����Ѵ�.
        CurrentComponentStrategy.EnterStrategy(targetObject, characterComponentInfoPrefab);
    }

    // ������Ʈ �ִ� ĳ���� �����տ� ��ǥ ����, �ѹ���, �׸��� �׸��� ���·� Ȯ�� ������ ���������� ����
}

public class SetColorRed : MonoBehaviour, IComponentStrategy
{
    private GameObject _targetObject;
    private MeshRenderer _thisComponent;
    private CharacterComponentInfoPrefab _characterComponentInfoPrefab;
    public void EnterStrategy(GameObject targetObject, CharacterComponentInfoPrefab characterComponentInfoPrefab)
    {
        if (!targetObject.TryGetComponent<MeshRenderer>(out _thisComponent))
        {
            _thisComponent = targetObject.AddComponent<MeshRenderer>();
        }
        _targetObject = targetObject;
        _characterComponentInfoPrefab = characterComponentInfoPrefab;
        _thisComponent.material.color = Color.red;
    }

    public void ExitStrategy()
    {
        // ��ųʸ� ó��
        GameManager.Instance.RemoveKeyInChracterSlot(this, _characterComponentInfoPrefab);

        _thisComponent.material.color = Color.white;
        Destroy(this);
    }
}

public class SetTall : MonoBehaviour, IComponentStrategy
{
    private float _tall = 3;

    private GameObject _targetObject;
    private Transform _thisComponent;
    private CharacterComponentInfoPrefab _characterComponentInfoPrefab;
    public void EnterStrategy(GameObject targetObject, CharacterComponentInfoPrefab characterComponentInfoPrefab)
    {
        if (!targetObject.TryGetComponent<Transform>(out _thisComponent))
        {
            _thisComponent = targetObject.AddComponent<Transform>();
        }
        _targetObject = targetObject;
        _characterComponentInfoPrefab = characterComponentInfoPrefab;
        _thisComponent.localScale = new Vector3(1, _tall, 1);
    }

    public void ExitStrategy()
    {
        // ��ųʸ� ó��
        GameManager.Instance.RemoveKeyInChracterSlot(this, _characterComponentInfoPrefab);

        _thisComponent.localScale = Vector3.one;
        Destroy(this);
    }
}

public class SetFat : MonoBehaviour, IComponentStrategy
{
    private float _fat = 3;

    private GameObject _targetObject;
    private Transform _thisComponent;
    private CharacterComponentInfoPrefab _characterComponentInfoPrefab;
    public void EnterStrategy(GameObject targetObject, CharacterComponentInfoPrefab characterComponentInfoPrefab)
    {
        if (!targetObject.TryGetComponent<Transform>(out _thisComponent))
        {
            _thisComponent = targetObject.AddComponent<Transform>();
        }
        _targetObject = targetObject;
        _characterComponentInfoPrefab = characterComponentInfoPrefab;
        _thisComponent.localScale = new Vector3(_fat, 1, _fat);
    }

    public void ExitStrategy()
    {
        // ��ųʸ� ó��
        GameManager.Instance.RemoveKeyInChracterSlot(this, _characterComponentInfoPrefab);

        _thisComponent.localScale = Vector3.one;
        Destroy(this);
    }
}

public class SetSpin : MonoBehaviour, IComponentStrategy
{
    private void Awake()
    {
        // FixedUpdate�� ���� ����
        this.enabled = false;
    }
    private float _spinSpeed = 30;

    private GameObject _targetObject;
    private Rigidbody _thisComponent;
    private CharacterComponentInfoPrefab _characterComponentInfoPrefab;
    public void EnterStrategy(GameObject targetObject, CharacterComponentInfoPrefab characterComponentInfoPrefab)
    {
        if (!targetObject.TryGetComponent<Rigidbody>(out _thisComponent))
        {
            _thisComponent = targetObject.AddComponent<Rigidbody>();
        }
        _targetObject = targetObject;
        _characterComponentInfoPrefab = characterComponentInfoPrefab;
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
        // ��ųʸ� ó��
        GameManager.Instance.RemoveKeyInChracterSlot(this, _characterComponentInfoPrefab);

        _thisComponent.isKinematic = false;
        Destroy(this);
    }
}
