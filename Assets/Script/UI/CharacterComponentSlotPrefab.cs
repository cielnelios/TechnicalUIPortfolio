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

    // 포인터 들락날락하면 색상 바뀜
    public void OnPointerEnter(PointerEventData eventData)
    {
        _thisImage.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _thisImage.color = Color.white;
    }

    // 드래그 해서 놓으면 내 밑으로
    public void OnDrop(PointerEventData eventData)
    {
        // 드래그 하는 대상이 있다면
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().position = _thisRectTransform.position;
            eventData.pointerDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(_thisRectTransform.sizeDelta.x, _thisRectTransform.sizeDelta.y);
        }
    }
}
