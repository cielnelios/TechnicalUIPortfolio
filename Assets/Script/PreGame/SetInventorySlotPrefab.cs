using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetInventorySlotPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private RectTransform _thisRectTransform;
    private Image _thisImage;
    //public List<GameManager.ItemType> itemTypeOfThisSkillSlot = new List<GameManager.ItemType>();

    private void Awake()
    {
        this._thisRectTransform = this.GetComponent<RectTransform>();
        this._thisImage = this.GetComponent<Image>();
        this.gameObject.tag = "InventoryItemSlot";
    }

    // ������ ��������ϸ� ���� �ٲ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        _thisImage.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _thisImage.color = Color.white;
    }

    // �巡�� �ؼ� ������ �� ������
    public void OnDrop(PointerEventData eventData)
    {
        // �巡�� �ϴ� ����� �ִٸ�
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().position = _thisRectTransform.position;
        }
    }
}
