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
        //_iconImage, inventoryUICanvas : 생성시 Instantiate하면서 넣어줌
        this._thisRectTransform = this.GetComponent<RectTransform>();
        this._thisCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        // 대응되는 전략으로 이름 바꿔주고
        string newName = enumComponentStrategy.ToString();

        this.gameObject.name = newName;
        this._text.text = newName;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 이전 부모 기록하고 (캔슬시 복귀용)
        this._transformOfParentsBefore = this.transform.parent;

        // 최상단에 두기 위해 캔바스 직속으로 옮기고 제일 마지막으로 순서 변경
        this.transform.SetParent(uiCanvas);
        this.transform.SetAsLastSibling();

        // 반투명 처리한다음 레이캐스팅 중단
        this._thisCanvasGroup.alpha = 0.6f;
        this._thisCanvasGroup.blocksRaycasts = false;
    }
    
    // 드래그중에 마우스 드래그 위치로 이 아이템을 옮긴다.
    public void OnDrag(PointerEventData eventData)
    {
        _thisRectTransform.position = eventData.position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        // 리스트에서 끌어왔는데
        if (this._transformOfParentsBefore.CompareTag("ComponentList"))
        {
            //캐릭터 슬롯이 아니면 복귀
            if (!this.transform.parent.CompareTag("CharacterComponentSlot"))
            {
                this.transform.SetParent(_transformOfParentsBefore);
                this._thisRectTransform.position = this._transformOfParentsBefore.GetComponent<RectTransform>().position;
                this.transform.SetSiblingIndex(index);  // 원래 위치로
            }
            // 캐릭터 슬롯으로 끌어왔다면 컴포넌트를 붙인다.
            else if (this.transform.parent.CompareTag("CharacterComponentSlot"))
            {
                CharacterComponentInfoPrefab characterComponentInfoPrefab = this.transform.parent.gameObject.GetComponent<CharacterComponentSlotPrefab>().characterComponentInfoPrefab;
                GameObject targetCharacter = characterComponentInfoPrefab.targetCharacter;

                // strategySet으로 전략 클래스를 만들고 컴포넌트를 장착해서 동작
                _componentStrategySet = new ComponentStrategySet(enumComponentStrategy, targetCharacter);

                // 캐릭터쪽 스크립트에 슬롯에 장착된 전략 정보를 넘김
                characterComponentInfoPrefab.StrategyInSlotDictionary[this.transform.parent.gameObject] = _componentStrategySet.CurrentComponentStrategy;

                // 이후 복사본을 만들어서
                GameObject copiedObject = Instantiate(this.gameObject);

                // 다시 컴포넌트 리스트에 넣어준다.
                copiedObject.transform.SetParent(_transformOfParentsBefore);
                copiedObject.GetComponent<RectTransform>().position = this._transformOfParentsBefore.GetComponent<RectTransform>().position;
                copiedObject.transform.SetSiblingIndex(index);  // 원래 위치로

                // 복사하면서 딸려온 처리도 롤백
                copiedObject.GetComponent<CanvasGroup>().alpha = 1;
                copiedObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

                // Slot 증가
                characterComponentInfoPrefab.CreateSlot();
            }
        }
        // 캐릭터 슬롯에서 끌어왔는데
        else if (this._transformOfParentsBefore.CompareTag("CharacterComponentSlot")) 
        {
            // 기존과 다른 캐릭터 슬롯이면
            if ((this.transform.parent.CompareTag("CharacterComponentSlot"))&&(this.transform.parent != this._transformOfParentsBefore))
            {
                // 이전 주인에게서 빼았고, 비게 된 이전 슬롯을 없앤다.
                if (_componentStrategySet != null) _componentStrategySet.CurrentComponentStrategy.ExitStrategy();
                Destroy(this._transformOfParentsBefore.gameObject);

                // 다시 컴포넌트를 붙인다.
                CharacterComponentInfoPrefab characterComponentInfoPrefab = this.transform.parent.gameObject.GetComponent<CharacterComponentSlotPrefab>().characterComponentInfoPrefab;
                GameObject targetCharacter = characterComponentInfoPrefab.targetCharacter;
                _componentStrategySet = new ComponentStrategySet(enumComponentStrategy, targetCharacter);

                // Slot 증가
                characterComponentInfoPrefab.CreateSlot();
            }
            // 캐릭터 슬롯에서 캐릭터 슬롯 아닌 곳으로 치우면
            else if (!this.transform.parent.CompareTag("CharacterComponentSlot"))
            {
                // 이전 주인에게서 빼았고, 비게 된 이전 슬롯을 없앤다.
                if (_componentStrategySet != null) _componentStrategySet.CurrentComponentStrategy.ExitStrategy();
                Destroy(this._transformOfParentsBefore.gameObject);
                // 자신도 파괴한다.
                Destroy(this.gameObject);
                return;
            }
        }
        
        // 어느 쪽이든 투명 처리 끝내고 레이캐스팅 재개
        this._thisCanvasGroup.alpha = 1;
        this._thisCanvasGroup.blocksRaycasts = true;
    }
}
