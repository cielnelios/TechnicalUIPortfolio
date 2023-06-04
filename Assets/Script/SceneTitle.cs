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
        // 현재 씬
        Scene thisScene = this.gameObject.scene;

        // UI 타이틀 이름 변경
        _sceneTitleText.text = thisScene.name;

        // X 버튼에 씬 닫기 등록
        _closeSceneButton.onClick.AddListener(() => this.DelUIScene(thisScene));
    }

    private void DelUIScene(Scene scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }
}
