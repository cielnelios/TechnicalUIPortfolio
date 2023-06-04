using UnityEngine;
public class ComponentList : MonoBehaviour
{
    [SerializeField] private Transform _uiCanvas;
    [SerializeField] private GameObject _contentContainer;
    [SerializeField] private GameObject _itemPrefab;

    private void Awake()
    {
        _uiCanvas = this.transform.parent;
    }

    private void Start()
    {
        int i = 0;
        foreach (GameManager.EnumComponentStrategy key in GameManager.Instance.IComponentStrategyDictionary.Keys)
        {
            // ������ ��ġ�ϰ�
            GameObject newObject = Instantiate(_itemPrefab, _contentContainer.transform) as GameObject;

            // ���� �־��ش�.
            ComponentItemPrefab componentItemPrefab = newObject.GetComponent<ComponentItemPrefab>();
            componentItemPrefab.index = i++;
            componentItemPrefab.enumComponentStrategy = key;
            componentItemPrefab.uiCanvas = this._uiCanvas;
        }
    }
}
