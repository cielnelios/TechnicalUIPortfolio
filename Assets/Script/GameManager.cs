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
        // ��ųʸ��� ������ �߰��Ѵ�.
        // �� ������ �̹� �Ŵ����� ���� �ν��Ͻ��̸�, ������ �������� ����. context�� �����Ұ���
        IComponentStrategyDictionary.Add(EnumComponentStrategy.setColorRed, this.gameObject.AddComponent<SetColorRed>());
        IComponentStrategyDictionary.Add(EnumComponentStrategy.setSpin, this.gameObject.AddComponent<SetSpin>());
        IComponentStrategyDictionary.Add(EnumComponentStrategy.SetTall, this.gameObject.AddComponent<SetTall>());
        IComponentStrategyDictionary.Add(EnumComponentStrategy.SetFat, this.gameObject.AddComponent<SetFat>());
    }
}

// �������� �����ϴ� �������̽�
public interface IComponentStrategy
{
    void EnterStrategy(GameObject targetObject);
    void ExitStrategy();
}

// ������ �����ϰ�, �� ������ ������ �����ϱ� ���� ������ �� ������Ʈ�� �ٴ� ����
public class ComponentStrategySet
{
    // ���� ���õ� ������ ��� ����
    public IComponentStrategy CurrentComponentStrategy { get; private set; }

    // ������ ��ġ�ϴ� ������ ������ ������ ���� �κ�
    public ComponentStrategySet(GameManager.EnumComponentStrategy enumComponentStrategy, GameObject targetObject)
    {
        // enum���� ������ �����´�.
        GameManager.Instance.IComponentStrategyDictionary.TryGetValue(enumComponentStrategy, out IComponentStrategy componentStrategy);

        // enum���� �� ���� Ŭ���� ������Ʈ�� �������ְ�,
        System.Type type = componentStrategy.GetType();
        targetObject.AddComponent(type);

        // ���õ� ���� Ŭ������ ���ؽ�Ʈ���� ����ϰ�, �� ���� ������Ʈ�� �����Ҷ� �˷��ִ� �뵵�� ���.
        targetObject.TryGetComponent(type, out Component component);
        CurrentComponentStrategy = (IComponentStrategy)component;

        // ������Ʈ�� enum���� �� ���� Ŭ������ �������ְ�, ���� Ŭ�������� � ���� ������Ʈ�� ���ȴ��� �����Ѵ�.
        CurrentComponentStrategy.EnterStrategy(targetObject);
    }

    // ������Ʈ �ִ� ĳ���� �����տ� ��ǥ ����, �ѹ���, �׸��� �׸��� ���·� Ȯ�� ������ ���������� ����
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
        // FixedUpdate�� ���� ����
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
