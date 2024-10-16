using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemList
{
    //static ItemList instance = this;
    static List<Item> items = new List<Item>();

    static ItemList()
    {
        Init();
    }

    public static void Init()
    {
        items.Add(new ItemTheSadOnion { Id = 0, Title = "The Sad Onion",  Description = "Tears up" });
        items.Add(new Item { Id = 1, Title = "Cricket's head", Description = "DMG up" });
        items.Add(new ActiveItem { Id = 2, Title = "Yum Heart",      Description = "Resuable regeneration" });
        Debug.Log(items);
    }

    public static Item GetItemById(int _id)
    {
        return items.Find(x => x.Id == _id);
    }
}

public class ItemReader : MonoBehaviour
{
    [SerializeField] List<Item> Items = new List<Item>();
    ActiveItem MainActiveItem = null;
    Player P;

    public void AddItem(Item _thing)
    {
        Items.Add(_thing);
        print(_thing.Title + "|---|" + _thing.Description);
        WhenPlayerGetItem();
    }
    private void Awake()
    {
        P = GetComponent<Player>();

    }

    public void WhenPlayerGetItem()
    {
        Items[Items.Count - 1].WhenRecieved(P);
    }

    public void WhenPlayerCarryingItem()
    {
        foreach (var item in Items) {
            if (item.SpecialEffect == SpecialEffects.Carrying) {
                item.OnCarrying();
            }
        }
    }

    public void WhenActiveItemUsed()
    {
        if (MainActiveItem != null) {
            MainActiveItem.OnActiveItemUsed(P);
        }
    }
}
