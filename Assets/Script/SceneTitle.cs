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
        _sceneTitleText.text = thisScene.name;

        // X ��ư�� �� �ݱ� ���
        _closeSceneButton.onClick.AddListener(() => this.DelUIScene(thisScene));
    }

    private void DelUIScene(Scene scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }
}
