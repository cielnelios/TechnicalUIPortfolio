using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=uTeZz4O12yU
public class SetInventoryCharacterInfoPrefab : MonoBehaviour
{
    private void Start()
    {
        
    }
    //public int characterIndex;

    //private GameManager.CharacterValueStruct _thisCharacterValue;
    //[SerializeField] private GameObject _characterIllust;
    //[SerializeField] private GameObject _itemSlotPanel;
    //[SerializeField] private List<SetInventorySlotPrefab> _itemSlot;

    //private void Start()
    //{
    //    // �Ϸ� ����
    //    _characterIllust.GetComponent<SetCharacterSlotPrefab>().index = characterIndex;
    //    _characterIllust.SetActive(true);

    //    //Explosive, Food, Potion
    //    if (ResourceManager.GetSkillIconImage(51, out Sprite iconSprite0))
    //    {
    //        this._itemSlot[0].GetComponent<Image>().sprite = iconSprite0;
    //        this._itemSlot[0].GetComponent<SetInventorySlotPrefab>().itemTypeOfThisSkillSlot.Add(GameManager.ItemType.Explosive);
    //    }

    //    if (ResourceManager.GetSkillIconImage(19, out Sprite iconSprite1))
    //    {
    //        this._itemSlot[1].GetComponent<Image>().sprite = iconSprite1;
    //        this._itemSlot[1].GetComponent<SetInventorySlotPrefab>().itemTypeOfThisSkillSlot.Add(GameManager.ItemType.Food);
    //    }

    //    if (ResourceManager.GetSkillIconImage(22, out Sprite iconSprite2))
    //    {
    //        this._itemSlot[2].GetComponent<Image>().sprite = iconSprite2;
    //        this._itemSlot[2].GetComponent<SetInventorySlotPrefab>().itemTypeOfThisSkillSlot.Add(GameManager.ItemType.Potion);
    //    }
    //}


    //private void AStart()
    //{
    //    // �Ϸ� ����
    //    _characterIllust.GetComponent<SetCharacterSlotPrefab>().index = characterIndex;
    //    _characterIllust.SetActive(true);

    //    // �� ĳ������ ��ų �ε����� �����ͼ�, �κ��丮 ���� ��ų�̸�, ������ ����
    //    GameManager.GetContentsData(characterIndex, out _thisCharacterValue);

    //    List<GameManager.ItemType>[] ItemTypeInfoOfEachSkill = new List<GameManager.ItemType>[3];
    //    for (int i = 0; i < _thisCharacterValue.SkillList.Count; i++)
    //    {
    //        int sklIdx = _thisCharacterValue.SkillList[i];

    //        bool isSuccess = GameManager.Instance.GetSkillData(sklIdx, out SkillScript value);
    //        if (!isSuccess) continue;

    //        List<GameManager.ItemType> itemTypeOfThisSkillSlot = new List<GameManager.ItemType>();

    //        // �κ� �����̸� ���� ����
    //        if (value.skillType.ToString() == "Equipment" || value.skillType.ToString() == "Potion")
    //        {
    //            foreach (ItemBag.ItemMultiUpContainer ItemMultiUpContainer in value.GetComponent<ItemBag>().ItemMultiUpContainers)
    //            {
    //                itemTypeOfThisSkillSlot.Add(ItemMultiUpContainer.Item);
    //            }
    //            ItemTypeInfoOfEachSkill[i] = itemTypeOfThisSkillSlot;
    //        }
    //        else
    //        {
    //            ItemTypeInfoOfEachSkill[i] = null;
    //        }

    //    }

    //    // �����̸� ��ŸƮ �޼ҵ带 �����ϰ� �ڽ��� ����.
    //    bool isBlank = true;
    //    for (int i = 0; i < ItemTypeInfoOfEachSkill.Length; i++)
    //    {
    //        if (ItemTypeInfoOfEachSkill[i] != null) isBlank = false;
    //    }
    //    if (isBlank) Destroy(this.gameObject);

    //    // �ش� ������ �κ��丮 ���Կ� �־��ְ�, ������ ������ ������ �����, �ű� ���缭 ����� ������
    //    for (int i = 0; i < ItemTypeInfoOfEachSkill.Length; i++)
    //    {
    //        // ������ ��ŵ
    //        if (ItemTypeInfoOfEachSkill[i] == null)
    //        {
    //            // �����
    //            //_itemSlot[i].transform.SetAsLastSibling();
    //            _itemSlot[i].gameObject.SetActive(false);

    //            //������ �����ؼ� �¿�� �������� ��
    //            Vector2 tempDelta = _itemSlotPanel.GetComponent<RectTransform>().sizeDelta;
    //            _itemSlotPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(tempDelta.x - 68, tempDelta.y);

    //            continue;
    //        }

    //        // GameManager.ItemType ����ְ�
    //        _itemSlot[i].itemTypeOfThisSkillSlot.AddRange(ItemTypeInfoOfEachSkill[i]);
    //        // ���ڿ� ������ ��ũ��Ʈ ������ ���� ����
    //        // enum�� ��Ʈ������ �ٲ��ִ� ���� : �׳� �����ۿ��� �̳����� �ٲٴ� ������ �Ѵ�.
    //        //_itemSlot[i].itemTypeOfThisSkillSlotString.AddRange(ItemTypeInfoOfEachSkill[i].ConvertAll(new System.Converter<GameManager.ItemType, string>(enumToString)));
    //    }

    //    // ���ڿ� �ٲٱ�� �ӽ� �Լ�
    //    //string enumToString(GameManager.ItemType thisEnum)
    //    //{
    //    //    return thisEnum.ToString();
    //    //}
    //}
}
