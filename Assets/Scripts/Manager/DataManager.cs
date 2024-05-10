using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : SingletonDontDestroy<DataManager>
{
    // 1. ĳ���� ����

    // ���� ������
    [SerializeField] private Data_Character characterStat;

    // ���� �����͸� ���� ��ųʸ�
    Dictionary<string, Data_Character.Param> characterStatData = new Dictionary<string, Data_Character.Param>();

    // ���� �����͸� ��ųʸ��� �ִ� �޼ҵ�
    void CharacterData()
    {
        for (int i = 0; i < characterStat.sheets[0].list.Count; ++i)
        {
            characterStatData.Add(characterStat.sheets[0].list[i].Name, characterStat.sheets[0].list[i]);
        }
    }

    // ���� �����͸� ���� �������� �޼ҵ�
    public Data_Character.Param GetCharacterData(string name)
    {
        return characterStatData.TryGetValue(name, out var result) ? result : null;
    }

    //======================================================================================

    // 2. ������ ����

    // ���� ������
    [SerializeField] private Data_Item itemStat;

    // ���� �����͸� ���� ��ųʸ�
    Dictionary<int, Data_Item.Param> itemStatData = new Dictionary<int, Data_Item.Param>();

    // ���� �����͸� ��ųʸ��� �ִ� �޼ҵ�
    void WeaponData()
    {
        for (int i = 0; i < itemStat.sheets[0].list.Count; ++i)
        {
            itemStatData.Add(itemStat.sheets[0].list[i].ID, itemStat.sheets[0].list[i]);
        }
    }

    // ID�� �̸��� �������� �޼ҵ�
    public Data_Item.Param GetWeaponData(int ID)
    {
        return itemStatData.TryGetValue(ID, out var result) ? result : null;
    }

    public Data_Item.Param GetRandomItemDataParams()
    {
        //��ȭ�� ��� �ҷ����� ���ϰ� + �Ҹ�ǰ�� 10000����� �ٲ�
        var unenhancedOrConsumableItems = itemStatData.Values.Where(item => (item.ID % 1000 == 0 && item.ItemType != "Consumable") || (item.ID >= 10000 && item.ItemType == "Consumable")).ToList();

        if (unenhancedOrConsumableItems.Count == 0)
        {
            Debug.Log("No unenhanced or consumable items available for random selection.");
            return null;
        }

        return unenhancedOrConsumableItems.OrderBy(o => Guid.NewGuid()).FirstOrDefault();
    }

    //=========================================================================================

    // 3. ������ �ܾ� ����

    // ���� ������
    [SerializeField] private Data_Lacalize lacalize;

    // ���� �����͸� ���� ��ųʸ�
    Dictionary<int, Data_Lacalize.Param> lacalizeData = new Dictionary<int, Data_Lacalize.Param>();

    void LacalizeData()
    {
        for (int i = 0; i < lacalize.sheets[0].list.Count; ++i)
        {
            lacalizeData.Add(lacalize.sheets[0].list[i].ID, lacalize.sheets[0].list[i]);
        }
    }

    public Data_Lacalize.Param GetLacalizeData(int ID)
    {
        return lacalizeData.TryGetValue(ID, out var result) ? result : null;
    }

    //=========================================================================================.

    // 4. UI �ܾ� ����

    [SerializeField] private Data_LacalizeWord word;

    Dictionary<string, Data_LacalizeWord.Param> wordData = new Dictionary<string, Data_LacalizeWord.Param>();

    void WordData()
    {
        for (int i = 0; i < word.sheets[0].list.Count; ++i)
        {
            wordData.Add(word.sheets[0].list[i].Type, word.sheets[0].list[i]);
        }
    }

    public string GetWordData(string type)
    {
        if (!wordData.ContainsKey(type))
        {
            Debug.LogError("�����͸� �ҷ��� �� �����ϴ� - " + type);
            return wordData["None"].ToString();
        }
        return wordData[type].Kor;
    }

    //=========================================================================================.

    // 5. ���� ����
    [SerializeField] private Data_Shop shop;

    Dictionary<int, Data_Shop.Param> shopData = new Dictionary<int, Data_Shop.Param>();

    void ShopData()
    {
        for (int i = 0; i < shop.sheets[0].list.Count; ++i)
        {
            shopData.Add(shop.sheets[0].list[i].ID, shop.sheets[0].list[i]);
        }
    }

    public List<Data_Shop.Param> GetAllShopItems()
    {
        return new List<Data_Shop.Param>(shopData.Values);
    }

    public Data_Shop.Param GetShopData(int ID)
    {
        return shopData.TryGetValue(ID, out var result) ? result : null;
    }

    protected override void DoAwake()
    {
        CharacterData();
        WeaponData();
        LacalizeData();
        WordData();
        ShopData();
    }
}
