using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipPanel : MonoBehaviour
{
    // TODO : �÷��̾ ������ ���� �� UI �ݿ�
    // �κ��丮���� ���� Ŭ�� ��, �����ϸ鼭 �κ��丮�� -> ���� ����������
    // 
    public UI_EquipSlot HeadArmorSlot;    
    public UI_EquipSlot BodyArmorSlot;    
    public UI_EquipSlot GloveSlot;    
    public UI_EquipSlot BootsSlot;    
    public UI_EquipSlot WeaponSlot;    
    public UI_EquipSlot PendantSlot;    
    public UI_EquipSlot RingSlot;

    /// <summary>
    /// <para>�÷��̾ ���� ���� �����ۿ� ���� ���� UI�� ����</para>
    /// ID�� �⺻������ 1 �̻��� ����. 0�̸� �������� �������� �ʾҴٴ� ��
    /// </summary>
    private void SetItemToSlot()
    {
        PlayerEquipData playerEquipData = PlayerEquipManager.Instance.GetPlayerEquipFromDB();


        if (playerEquipData.HeadItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.HeadItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
        }
        
    }        
}
