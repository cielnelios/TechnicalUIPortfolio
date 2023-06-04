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
    //    // 일러 설정
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
    //    // 일러 설정
    //    _characterIllust.GetComponent<SetCharacterSlotPrefab>().index = characterIndex;
    //    _characterIllust.SetActive(true);

    //    // 이 캐릭터의 스킬 인덱스를 가져와서, 인벤토리 관련 스킬이면, 정보를 저장
    //    GameManager.GetContentsData(characterIndex, out _thisCharacterValue);

    //    List<GameManager.ItemType>[] ItemTypeInfoOfEachSkill = new List<GameManager.ItemType>[3];
    //    for (int i = 0; i < _thisCharacterValue.SkillList.Count; i++)
    //    {
    //        int sklIdx = _thisCharacterValue.SkillList[i];

    //        bool isSuccess = GameManager.Instance.GetSkillData(sklIdx, out SkillScript value);
    //        if (!isSuccess) continue;

    //        List<GameManager.ItemType> itemTypeOfThisSkillSlot = new List<GameManager.ItemType>();

    //        // 인벤 관련이면 정보 저장
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

    //    // 공백이면 스타트 메소드를 종료하고 자신을 비운다.
    //    bool isBlank = true;
    //    for (int i = 0; i < ItemTypeInfoOfEachSkill.Length; i++)
    //    {
    //        if (ItemTypeInfoOfEachSkill[i] != null) isBlank = false;
    //    }
    //    if (isBlank) Destroy(this.gameObject);

    //    // 해당 정보를 인벤토리 슬롯에 넣어주고, 정보가 없으면 슬롯을 숨기고, 거기 맞춰서 사이즈도 재조정
    //    for (int i = 0; i < ItemTypeInfoOfEachSkill.Length; i++)
    //    {
    //        // 없으면 스킵
    //        if (ItemTypeInfoOfEachSkill[i] == null)
    //        {
    //            // 숨기고
    //            //_itemSlot[i].transform.SetAsLastSibling();
    //            _itemSlot[i].gameObject.SetActive(false);

    //            //사이즈 조정해서 좌우로 안퍼지게 함
    //            Vector2 tempDelta = _itemSlotPanel.GetComponent<RectTransform>().sizeDelta;
    //            _itemSlotPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(tempDelta.x - 68, tempDelta.y);

    //            continue;
    //        }

    //        // GameManager.ItemType 담아주고
    //        _itemSlot[i].itemTypeOfThisSkillSlot.AddRange(ItemTypeInfoOfEachSkill[i]);
    //        // 문자열 버전도 스크립트 때문에 따로 저장
    //        // enum을 스트링으로 바꿔주는 구현 : 그냥 아이템에서 이넘으로 바꾸는 식으로 한다.
    //        //_itemSlot[i].itemTypeOfThisSkillSlotString.AddRange(ItemTypeInfoOfEachSkill[i].ConvertAll(new System.Converter<GameManager.ItemType, string>(enumToString)));
    //    }

    //    // 문자열 바꾸기용 임시 함수
    //    //string enumToString(GameManager.ItemType thisEnum)
    //    //{
    //    //    return thisEnum.ToString();
    //    //}
    //}
}
