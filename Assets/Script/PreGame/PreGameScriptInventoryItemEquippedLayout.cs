using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // ���⼭ ������ �������̽��� ���ӽ����̽�

// https://www.youtube.com/watch?v=uTeZz4O12yU : �̺�Ʈ �ý���

public class PreGameScriptInventoryItemEquippedLayout : MonoBehaviour//,
    //IPointerEnterHandler, IPointerExitHandler,
    //IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, 
    //IBeginDragHandler, IDragHandler, IEndDragHandler, 
    //IDropHandler    //onDrop method�� ���� ��� ���
{
    private GameObject _itemEquippedLayout;
    [SerializeField] private GameObject _characterInventoryInfoPrefab;

    private void Awake()
    {
        //_itemEquippedLayout = this.gameObject;
    }

    private void Start()
    {
        //// ������ �� �θ� ������Ʈ ã��
        //FunctionManager.GetContentsFromLayout(_itemEquippedLayout, out GameObject ContentObject);
        
        //for (int i = 0; i < GameManager.GetCharacterIndexList().Count; i++)
        //{
        //    int j = i + 1;

        //    // ������ ��ġ�ϰ�
        //    GameObject newObject = Instantiate(_characterInventoryInfoPrefab, ContentObject.transform) as GameObject;

        //    // ���� �־��ش�.
        //    SetInventoryCharacterInfoPrefab inventoryItemPrefab = newObject.GetComponent<SetInventoryCharacterInfoPrefab>();
        //    inventoryItemPrefab.characterIndex = j;
        //}
    }

    // UI�� �̺�Ʈ Ʈ���� ���̷��� ���
    // https://daru-daru.tistory.com/55 : �̺�Ʈ Ʈ���� ����
    //EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

    //EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
    //entry_PointerDown.eventID = EventTriggerType.PointerDown;
    //entry_PointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
    //eventTrigger.triggers.Add(entry_PointerDown);

    //// ������ �̵�
    //public void OnPointerEnter(PointerEventData eventData)
    //{

    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{

    //}

    //// ������ Ŭ��
    //public void OnPointerClick(PointerEventData eventData)
    //{

    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{

    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{

    //}

    //// �巡��
    //public void OnBeginDrag(PointerEventData eventData)
    //{

    //}

    //public void OnDrag(PointerEventData eventData)
    //{

    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{

    //}

    //// �巡�� �� ���
    //public void OnDrop(PointerEventData eventData)
    //{

    //}
}
