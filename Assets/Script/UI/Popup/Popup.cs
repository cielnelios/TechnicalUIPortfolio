using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    public string _titleText;
    [SerializeField] private TextMeshProUGUI _popupTitleText;
    [SerializeField] private Button _closePopupButton;
    [SerializeField] private Button _confirmPopupButton;
    [SerializeField] private Button _cancelPopupButton;

    private void Awake()
    {
        // UI 타이틀 이름 변경
        this._popupTitleText.text = _titleText;

        // X 버튼에 씬 닫기 등록
        this._closePopupButton.onClick.AddListener(() => this.gameObject.SetActive(false));

        // 캔슬 버튼에 씬 닫기 등록
        this._cancelPopupButton.onClick.AddListener(() => this.gameObject.SetActive(false));

        // 확인 버튼에 씬 닫기 등록. 오브젝트 생성 기능은 CreatePrefabFromPopup에서 등록
        this._confirmPopupButton.onClick.AddListener(() => this.gameObject.SetActive(false));
    }
}
