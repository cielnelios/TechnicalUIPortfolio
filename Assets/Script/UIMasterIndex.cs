using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIMasterIndex : MonoBehaviour
{
    // 열린 UI 목록을 관리
    private Stack<Scene> _uiList = new Stack<Scene>();

    // 씬을 스택에 추가
    public void AddUIScene(Scene scene)
    {
        this._uiList.Push(scene);
        //Debug.Log("add " + this._uiList.Peek().name);
    }

    // 씬을 스택에서 뽑아서 닫음. 마지막 씬이므로 가장 마지막에 연 UI임
    public void DelUIScene()
    {
        if (this._uiList.Count > 0)
        {
            Scene UISceneToBeDeleted = this._uiList.Pop();
            SceneManager.UnloadSceneAsync(UISceneToBeDeleted);
        }
    }

    public void DelUIScene(InputAction.CallbackContext context)
    {
        this.DelUIScene();
    }
}
