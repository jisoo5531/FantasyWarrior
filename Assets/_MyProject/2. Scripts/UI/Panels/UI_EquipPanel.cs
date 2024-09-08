using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipPanel : MonoBehaviour
{
    // TODO : 0909. 아이템 슬롯에 커서를 올리면 아이템 정보가 나오게끔. 현재 투구만 임시로 테스트로. 다른 부위도 해주기

    [Header("Slot")]
    public UI_EquipSlot HeadArmorSlot;    
    public UI_EquipSlot BodyArmorSlot;    
    public UI_EquipSlot GloveSlot;    
    public UI_EquipSlot BootsSlot;    
    public UI_EquipSlot WeaponSlot;    
    public UI_EquipSlot PendantSlot;    
    public UI_EquipSlot RingSlot;

    [Header("Item Info")]
    public UI_ItemInfo HeadArmorInfo;
        

    private PlayerEquipData playerEquipData;

    private void Awake()
    {        

    }


    private void Start()
    {
        // 플레이어가 인벤토리 아이템을 장착 시 실행할 함수
        PlayerEquipManager.Instance.OnEquipItem += SetItemToSlot;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetItemToSlot;
        SetItemToSlot();
    }

    /// <summary>
    /// <para>플레이어가 장착 중인 아이템에 맞춰 슬롯 UI에 세팅</para>
    /// ID는 기본적으로 1 이상의 정수. 0이면 아이템을 장착하지 않았다는 것
    /// </summary>
    private void SetItemToSlot()
    {
        playerEquipData = PlayerEquipManager.Instance.GetPlayerEquipFromDB();

        HelmetSlotSetting();
        ArmorSlotSetting();
        GloveSlotSetting();
        BootsSlotSetting();
        WeaponSlotSetting();
        PendantSlotSetting();
        RingSlotSetting();
    }
    #region 장비 슬롯 세팅
    private void HelmetSlotSetting()
    {
        if (playerEquipData.HeadItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.HeadItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            HeadArmorSlot.Initialize(playerEquipData.HeadItem_ID, sprite, HeadArmorInfo);       
            
        }
        else
        {
            // 장비를 장착하지 않은 경우            
            HeadArmorSlot.Initialize();
        }
    }
    private void ArmorSlotSetting()
    {
        if (playerEquipData.ArmorItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.ArmorItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            BodyArmorSlot.Initialize(playerEquipData.ArmorItem_ID, sprite);            
        }
        else
        {
            // 장비를 장착하지 않은 경우            
            BodyArmorSlot.Initialize();
        }
    }
    private void GloveSlotSetting()
    {
        if (playerEquipData.GloveItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.GloveItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            GloveSlot.Initialize(playerEquipData.GloveItem_ID, sprite);            
        }
        else
        {
            // 장비를 장착하지 않은 경우            
            GloveSlot.Initialize();
        }
    }
    private void BootsSlotSetting()
    {
        if (playerEquipData.BootItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.BootItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            BootsSlot.Initialize(playerEquipData.BootItem_ID, sprite);            
        }
        else
        {
            // 장비를 장착하지 않은 경우            
            BootsSlot.Initialize();
        }
    }
    private void WeaponSlotSetting()
    {
        if (playerEquipData.WeaponItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.WeaponItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            WeaponSlot.Initialize(playerEquipData.WeaponItem_ID, sprite);            
        }
        else
        {
            // 장비를 장착하지 않은 경우            
            WeaponSlot.Initialize();
        }
    }
    private void PendantSlotSetting()
    {
        if (playerEquipData.Pendant_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.Pendant_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            PendantSlot.Initialize(playerEquipData.Pendant_ID, sprite);            
        }
        else
        {
            // 장비를 장착하지 않은 경우            
            PendantSlot.Initialize();
        }
    }
    private void RingSlotSetting()
    {
        if (playerEquipData.Ring_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.Ring_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            RingSlot.Initialize(playerEquipData.Ring_ID, sprite);            
        }
        else
        {
            // 장비를 장착하지 않은 경우            
            RingSlot.Initialize();
        }
    }
    #endregion

    
}
