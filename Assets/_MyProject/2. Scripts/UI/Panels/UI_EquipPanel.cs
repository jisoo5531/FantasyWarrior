using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipPanel : MonoBehaviour
{
    // TODO : 0909 ���� �гο��� ���� Ŭ���ϸ� ��� ���� ��, �κ��丮�� �̵�
    // ���� ��� ���� ��ư�� ������ �ǰԴ� �Ǿ� �ִ�.
    // ��� �����ϸ� ����� �ɷ´�� ���ȿ� �ݿ��ϱ�
    [Header("Slot")]
    public UI_EquipSlot HeadArmorSlot;    
    public UI_EquipSlot BodyArmorSlot;    
    public UI_EquipSlot GloveSlot;    
    public UI_EquipSlot BootsSlot;    
    public UI_EquipSlot WeaponSlot;    
    public UI_EquipSlot PendantSlot;    
    public UI_EquipSlot RingSlot;

        

    private PlayerEquipData playerEquipData;

    private void Awake()
    {        
    }


    private void Start()
    {
        // �÷��̾ �κ��丮 �������� ���� �� ������ �Լ�
        PlayerEquipManager.Instance.OnEquipItem += SetItemToSlot;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetItemToSlot;
        SetItemToSlot();
    }

    /// <summary>
    /// <para>�÷��̾ ���� ���� �����ۿ� ���� ���� UI�� ����</para>
    /// ID�� �⺻������ 1 �̻��� ����. 0�̸� �������� �������� �ʾҴٴ� ��
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
    #region ��� ���� ����
    private void HelmetSlotSetting()
    {
        if (playerEquipData.HeadItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.HeadItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            HeadArmorSlot.Initialize(sprite);
            Debug.Log("�Ӹ� ������.");
        }
        else
        {
            // ��� �������� ���� ���
            Debug.Log("�Ӹ� �������� ����.");
            HeadArmorSlot.Initialize();
        }
    }
    private void ArmorSlotSetting()
    {
        if (playerEquipData.ArmorItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.ArmorItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            BodyArmorSlot.Initialize(sprite);
            Debug.Log("���� ������");
        }
        else
        {
            // ��� �������� ���� ���
            Debug.Log("���� �������� ����.");
            BodyArmorSlot.Initialize();
        }
    }
    private void GloveSlotSetting()
    {
        if (playerEquipData.GloveItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.GloveItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            GloveSlot.Initialize(sprite);
            Debug.Log("�尩 ������");
        }
        else
        {
            // ��� �������� ���� ���
            Debug.Log("�尩 �������� ����.");
            GloveSlot.Initialize();
        }
    }
    private void BootsSlotSetting()
    {
        if (playerEquipData.BootItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.BootItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            BootsSlot.Initialize(sprite);
            Debug.Log("�Ź� ������");
        }
        else
        {
            // ��� �������� ���� ���
            Debug.Log("�Ź� �������� ����.");
            BootsSlot.Initialize();
        }
    }
    private void WeaponSlotSetting()
    {
        if (playerEquipData.WeaponItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.WeaponItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            WeaponSlot.Initialize(sprite);
            Debug.Log("���� ������");
        }
        else
        {
            // ��� �������� ���� ���
            Debug.Log("���� �������� ����.");
            WeaponSlot.Initialize();
        }
    }
    private void PendantSlotSetting()
    {
        if (playerEquipData.Pendant_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.Pendant_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            PendantSlot.Initialize(sprite);
            Debug.Log("����� ������");
        }
        else
        {
            // ��� �������� ���� ���
            Debug.Log("����� �������� ����.");
            PendantSlot.Initialize();
        }
    }
    private void RingSlotSetting()
    {
        if (playerEquipData.Ring_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.Ring_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            RingSlot.Initialize(sprite);
            Debug.Log("���� ������");
        }
        else
        {
            // ��� �������� ���� ���
            Debug.Log("���� �������� ����.");
            RingSlot.Initialize();
        }
    }
    #endregion

    
}
