using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterComponentSlotPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private RectTransform _thisRectTransform;
    private Image _thisImage;
    public CharacterComponentInfoPrefab characterComponentInfoPrefab;

    private void Awake()
    {
        this._thisRectTransform = this.GetComponent<RectTransform>();
        this._thisImage = this.GetComponent<Image>();
        this.gameObject.tag = "CharacterComponentSlot";
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
            eventData.pointerDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(_thisRectTransform.sizeDelta.x, _thisRectTransform.sizeDelta.y);
        }
    }
}