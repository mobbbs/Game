using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    [SerializeField] private Vector2 velocity;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            rb.velocity = velocity;
        }
    }
    public void SetUpItem(ItemData itemData, Vector2 velocity)
    {
        this.itemData = itemData;
        this.velocity = velocity;
        rb.velocity = velocity;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "item object - " + itemData.name;
    }

    public void PickUpItem()
    {
        if (itemData.itemType == ItemType.Equipment)
        {
            if (Inventory.Instance.CanAddItem())
            {
                Inventory.Instance.AddItem(itemData);
            }
            else
            {
                rb.velocity = new Vector2(0, 7);
            }
        }
        else
        {
            Destroy(gameObject);
            Inventory.Instance.AddItem(itemData);
        }
    }
}
