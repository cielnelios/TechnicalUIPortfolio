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
        //_closeSceneButton.onClick.AddListener(() => this.DelUIScene(thisScene));
        _closeSceneButton.onClick.AddListener(() => this.DelUIScene());
    }

    private void DelUIScene(Scene scene)
    {
        // 직접 씬을 지우는 구현
        SceneManager.UnloadSceneAsync(scene);
    }

    private void DelUIScene()
    {
        //UI Master를 찾아가서 거기의 씬 삭제 함수를 호출하는 구현.
        //UI Master가 열린 UI의 스택을 가지고 있다.
        GameObject[] nextSceneRootObject = SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects();
        foreach (GameObject RootGameObject in nextSceneRootObject)
        {
            if (RootGameObject.CompareTag("UIMaster"))
            {
                RootGameObject.GetComponent<UIMasterIndex>().DelUIScene();
            }
        }
    }
}
