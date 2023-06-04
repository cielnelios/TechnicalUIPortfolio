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
        //_closeSceneButton.onClick.AddListener(() => this.DelUIScene(thisScene));
        _closeSceneButton.onClick.AddListener(() => this.DelUIScene());
    }

    private void DelUIScene(Scene scene)
    {
        // ���� ���� ����� ����
        SceneManager.UnloadSceneAsync(scene);
    }

    private void DelUIScene()
    {
        //UI Master�� ã�ư��� �ű��� �� ���� �Լ��� ȣ���ϴ� ����.
        //UI Master�� ���� UI�� ������ ������ �ִ�.
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
