using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 여기서 구현할 인터페이스의 네임스페이스

// https://www.youtube.com/watch?v=uTeZz4O12yU : 이벤트 시스템

public class PreGameScriptInventoryItemEquippedLayout : MonoBehaviour//,
    //IPointerEnterHandler, IPointerExitHandler,
    //IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, 
    //IBeginDragHandler, IDragHandler, IEndDragHandler, 
    //IDropHandler    //onDrop method를 가진 드랍 대상
{
    private GameObject _itemEquippedLayout;
    [SerializeField] private GameObject _characterInventoryInfoPrefab;

    private void Awake()
    {
        //_itemEquippedLayout = this.gameObject;
    }

    private void Start()
    {
        //// 프리팹 들어갈 부모 오브젝트 찾고
        //FunctionManager.GetContentsFromLayout(_itemEquippedLayout, out GameObject ContentObject);
        
        //for (int i = 0; i < GameManager.GetCharacterIndexList().Count; i++)
        //{
        //    int j = i + 1;

        //    // 프리팹 배치하고
        //    GameObject newObject = Instantiate(_characterInventoryInfoPrefab, ContentObject.transform) as GameObject;

        //    // 값들 넣어준다.
        //    SetInventoryCharacterInfoPrefab inventoryItemPrefab = newObject.GetComponent<SetInventoryCharacterInfoPrefab>();
        //    inventoryItemPrefab.characterIndex = j;
        //}
    }

    // UI에 이벤트 트리거 붙이려는 경우
    // https://daru-daru.tistory.com/55 : 이벤트 트리거 참고
    //EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

    //EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
    //entry_PointerDown.eventID = EventTriggerType.PointerDown;
    //entry_PointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
    //eventTrigger.triggers.Add(entry_PointerDown);

    //// 포인터 이동
    //public void OnPointerEnter(PointerEventData eventData)
    //{

    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{

    //}

    //// 포인터 클릭
    //public void OnPointerClick(PointerEventData eventData)
    //{

    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{

    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{

    //}

    //// 드래그
    //public void OnBeginDrag(PointerEventData eventData)
    //{

    //}

    //public void OnDrag(PointerEventData eventData)
    //{

    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{

    //}

    //// 드래그 된 대상
    //public void OnDrop(PointerEventData eventData)
    //{

    //}
}
