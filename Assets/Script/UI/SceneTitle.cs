using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTitle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sceneTitleText;
    [SerializeField] private Button _closeSceneButton;

    private void Awake()
    {
        // ���� ��
        Scene thisScene = this.gameObject.scene;

        // UI Ÿ��Ʋ �̸� ����
        this._sceneTitleText.text = thisScene.name;

        // X ��ư�� �� �ݱ� ���
        this._closeSceneButton.onClick.AddListener(() => GameManager.Instance.DelUIScene());
    }
}
