using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class ComponentItemPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index;
    public GameManager.EnumComponentStrategy enumComponentStrategy;
    [SerializeField] private TextMeshProUGUI _text;

    public Transform uiCanvas;
    private Transform _transformOfParentsBefore;
    private RectTransform _thisRectTransform;
    private CanvasGroup _thisCanvasGroup;
    private ComponentStrategySet _componentStrategySet;

    private void Awake()
    {
        //_iconImage, inventoryUICanvas : ������ Instantiate�ϸ鼭 �־���
        this._thisRectTransform = this.GetComponent<RectTransform>();
        this._thisCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        // �����Ǵ� �������� �̸� �ٲ��ְ�
        string newName = enumComponentStrategy.ToString();

        this.gameObject.name = newName;
        this._text.text = newName;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���� �θ� ����ϰ� (ĵ���� ���Ϳ�)
        this._transformOfParentsBefore = this.transform.parent;

        // �ֻ�ܿ� �α� ���� ĵ�ٽ� �������� �ű�� ���� ���������� ���� ����
        this.transform.SetParent(uiCanvas);
        this.transform.SetAsLastSibling();

        // ������ ó���Ѵ��� ����ĳ���� �ߴ�
        this._thisCanvasGroup.alpha = 0.6f;
        this._thisCanvasGroup.blocksRaycasts = false;
    }
    
    // �巡���߿� ���콺 �巡�� ��ġ�� �� �������� �ű��.
    public void OnDrag(PointerEventData eventData)
    {
        _thisRectTransform.position = eventData.position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        // ����Ʈ���� ����Դµ�
        if (this._transformOfParentsBefore.CompareTag("ComponentList"))
        {
            //ĳ���� ������ �ƴϸ� ����
            if (!this.transform.parent.CompareTag("CharacterComponentSlot"))
            {
                this.transform.SetParent(_transformOfParentsBefore);
                this._thisRectTransform.position = this._transformOfParentsBefore.GetComponent<RectTransform>().position;
                this.transform.SetSiblingIndex(index);  // ���� ��ġ��
            }
            // ĳ���� �������� ����Դٸ� ������Ʈ�� ���δ�.
            else if (this.transform.parent.CompareTag("CharacterComponentSlot"))
            {
                GameObject targetCharacter = this.transform.parent.gameObject.GetComponent<CharacterComponentSlotPrefab>().characterComponentInfoPrefab.targetCharacter;
                _componentStrategySet = new ComponentStrategySet(enumComponentStrategy, targetCharacter);
            }
        }
        // ĳ���� ���Կ��� ����Դµ�
        else if (this._transformOfParentsBefore.CompareTag("CharacterComponentSlot")) 
        {
            // ������ �ٸ� ĳ���� �����̸�
            if ((this.transform.parent.CompareTag("CharacterComponentSlot"))&&(this.transform.parent == this._transformOfParentsBefore))
            {
                // ���� ���ο��Լ� ���Ұ�
                if (_componentStrategySet != null) _componentStrategySet.CurrentComponentStrategy.ExitStrategy();
                // �ٽ� ������Ʈ�� ���δ�.
                GameObject targetCharacter = this.transform.parent.gameObject.GetComponent<CharacterComponentSlotPrefab>().characterComponentInfoPrefab.targetCharacter;
                _componentStrategySet = new ComponentStrategySet(enumComponentStrategy, targetCharacter);
            }
            // ĳ���� ���Կ��� ĳ���� ���� �ƴ� ������ ġ���
            else if (!this.transform.parent.CompareTag("CharacterComponentSlot"))
            {
                // ���� ���ο��Լ� ���Ұ�
                if (_componentStrategySet != null) _componentStrategySet.CurrentComponentStrategy.ExitStrategy();
                // �ڽ��� �ı��Ѵ�.
                Destroy(this.gameObject);
            }
        }
        
        // ��� ���̵� ���� ó�� ������ ����ĳ���� �簳
        this._thisCanvasGroup.alpha = 1;
        this._thisCanvasGroup.blocksRaycasts = true;
    }
}
