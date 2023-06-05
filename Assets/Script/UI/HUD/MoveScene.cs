using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private LoadSceneMode mode;
    // LoadSceneMode.Single: ���� ���� �����ϰ� ���ο� ���� �ε��մϴ�.
    // LoadSceneMode.Additive: ���� ���� ������ ä ���ο� ���� �ε��մϴ�.

    private void Awake()
    {
        // ��ư�� �� ���� ����� ���
        Button thisButton = this.gameObject.GetComponent<Button>();
        thisButton.onClick.AddListener( () => this.MoveSceneMethod(nextSceneName) );
    }

    private void MoveSceneMethod(string nextScene)
    {
        // ���� ������ ���� �ε��ϰ�, ESC�� ���� �� �ְ� LodedSceneListClass�� ���ÿ� ���
        if (!SceneManager.GetSceneByName(nextScene).isLoaded)
        {
            SceneManager.LoadScene(nextScene, mode);
            GameManager.Instance.AddUIScene(SceneManager.GetSceneByName(nextScene));
        }

        // ���� �ִٸ� : �� �������� UI ���� ������ serialize�ϱ⿡�� ���ŷӴ�.
        //GameManager.DelUIScene()�� ������ �� ��Ʈ��
        else
        {
            // UI �����͸� ã�Ƽ�
            foreach (GameObject gameObject in SceneManager.GetSceneByName(nextScene).GetRootGameObjects())
            {
                if (gameObject.CompareTag("UIMaster"))
                {
                    // ��Ȱ��ȭ��� ���ش�.
                    if (gameObject.activeSelf == false)
                    {
                        gameObject.SetActive(true);
                        GameManager.Instance.AddUIScene(SceneManager.GetSceneByName(nextScene));
                    }

                    // Ȱ��ȭ�� �������� �ȴ�.
                    break;
                }
            }
        }
    }

}
