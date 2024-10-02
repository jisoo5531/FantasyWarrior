using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShopSellItemSlot : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemQuantityText;

    private InventoryData invenItem;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickSellItemSlot);
    }
    private void Start()
    {
        InventoryManager.Instance.OnSubtractItem += UpdateQuantityText;
        InventoryManager.Instance.OnDeleteItem += EventSlotClear;
    }
    public void Initialize(InventoryData invenItem, Sprite itemSprite)
    {
        this.invenItem = invenItem;
        itemIcon.ImageTransparent(1);
        itemIcon.sprite = itemSprite;
        itemQuantityText.gameObject.SetActive(true);
        itemQuantityText.text = invenItem.Quantity.ToString();
    }
    private void EventSlotClear(ItemData item)
    {
        itemIcon.ImageTransparent(0);
        itemQuantityText.gameObject.SetActive(false);
    }
    public void SlotClear()
    {
        itemIcon.ImageTransparent(0);
        itemQuantityText.gameObject.SetActive(false);
    }
    private void UpdateQuantityText(ItemData item)
    {
        if (invenItem == null || this.invenItem.Item_ID != item.Item_ID)
        {
            return;
        }        

        itemQuantityText.text = InventoryManager.Instance.GetInventoryItem(item.Item_ID).Quantity.ToString();


    }
    /// <summary>
    /// 클릭하면 해당 아이템을 팔 것인지를 보여줄 UI 활성화
    /// </summary>
    private void OnClickSellItemSlot()
    {
        UI_ShopPanel shopPanel = FindObjectOfType<PanelManager>(true).ShopPanel;
        UI_ShopSellItemInfo sellItemInfo = shopPanel.sellItemInfo;
        sellItemInfo.Initialize(invenItem.Item_ID);
        sellItemInfo.gameObject.SetActive(true);
    }
}
