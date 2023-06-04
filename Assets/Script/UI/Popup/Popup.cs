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
        // UI Ÿ��Ʋ �̸� ����
        this._popupTitleText.text = _titleText;

        // X ��ư�� �� �ݱ� ���
        this._closePopupButton.onClick.AddListener(() => this.gameObject.SetActive(false));

        // ĵ�� ��ư�� �� �ݱ� ���
        this._cancelPopupButton.onClick.AddListener(() => this.gameObject.SetActive(false));

        // Ȯ�� ��ư�� �� �ݱ� ���. ������Ʈ ���� ����� CreatePrefabFromPopup���� ���
        this._confirmPopupButton.onClick.AddListener(() => this.gameObject.SetActive(false));
    }
}
