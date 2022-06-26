using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    Transform itemSlotContainer;
    Transform itemSlotTemplate;
    TPSMovement player;

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");

        //if (itemSlotContainer != null) Debug.Log("itemslotcontainer null de�il");
        //if (itemSlotContainer == null) Debug.Log("itemslotcontainer null ");
        //if (itemSlotTemplate != null) Debug.Log("itemslottemplate null de�il");
        //if (itemSlotTemplate == null) Debug.Log("itemslottemplate null ");
    }
    public void SetPlayer(TPSMovement player)
    {
        this.player = player;
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach(Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 60f;
        foreach (Item item in inventory.GetItemList())
        {
            //buran�n i�ini �ok da anlamad�m bak�caz
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {
                inventory.UseItem(item);
            
            };
            Debug.Log("sa� t�k fonksiyonundan �nce");
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {//her rect transform i�in farkl� bi mouseRightClickFunc var. ��yle bi �rnek verelim. ilk item cactus olsun. kakt�s�n oldu�u rect transforma girecek.
                                                                                         //MouseRightClickini belirliycek. lambda ifadesi this ve item i kaps�yor. kapsad��� item Cactus olacak. 
                Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };// d�zeltmek i�in bunu ekledik
                //inventory.RemoveItem(item);
                inventory.RemoveItem(item);
                ItemWorld.DropItem(new Vector3(player.transform.position.x, player.transform.position.y + 0.09f, player.transform.position.z), duplicateItem);
            };
            Debug.Log("sa� t�k fonksiyonundan sonra");
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            Text amountText = itemSlotRectTransform.Find("amountText").GetComponent<Text>();
            if (item.amount > 1)
                amountText.text = item.amount.ToString();
            else
                amountText.text = " ";
            x++;
            if (x > 3)
            {
                x = 0;
                y--;
            }
        }
    }
}
