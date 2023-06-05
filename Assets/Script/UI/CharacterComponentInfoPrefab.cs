using System.Collections.Generic;
using UnityEngine;

public class CharacterComponentInfoPrefab : MonoBehaviour
{
    public GameObject targetCharacter;
    
    [SerializeField] private GameObject _contentContainer;
    [SerializeField] private GameObject _itemPrefab;

    public Dictionary<GameObject, IComponentStrategy> StrategyInSlotDictionary// { get; private set; }
        = new Dictionary<GameObject, IComponentStrategy>();

    private void Start()
    {
        this.CreateSlot();
    }

    public void CreateSlot()
    {
        // ������Ʈ ���� ������ ��ġ�ϰ�
        GameObject newObject = Instantiate(_itemPrefab, _contentContainer.transform) as GameObject;
        // ���Կ� �� ĳ���� �������� �����
        newObject.GetComponent<CharacterComponentSlotPrefab>().characterComponentInfoPrefab = this;
        // ������ �߰��ؼ� �� ���Կ� ��ġ�� ������ ���� �ľ��� �� �ֵ��� �Ѵ�.
        StrategyInSlotDictionary.Add(newObject,null);
    }

}
