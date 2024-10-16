using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemObject : MonoBehaviour
{
    public int Id;
    [SerializeField] Item item;
    private void Awake()
    {
        item = ItemList.GetItemById(Id);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            item = ItemList.GetItemById(Id);
            other.GetComponent<ItemReader>().AddItem(item);
            Destroy(gameObject);
        }
    }
}