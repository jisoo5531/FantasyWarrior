using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTest : MonsterUnit
{
    // TOOD : �ӽ� �÷��̾� Ÿ�� ����, ���߿� MonsterUnit�� �÷��̾� �ּ� ������ ���
    public GameObject testPlayer;
    
    int rewardExp = 1000;
    protected override void Initialize()
    {        
        monsterID = 2;        
        // TODO : �ӽ�
        this.monsterData = new MonsterData(2, "��� ��", 2000, 2000, 200, 10, 5, 3, 100, 100, 1);

        M_StateMachine = new BossStateMachine(this);          
        M_StateMachine.Initialize(M_StateMachine.idleState);
        base.Initialize();
    }
    private void Update()
    {
        if (damagable.isStunned)
        {
            Debug.Log("�� �����̴�. �� ������");
            M_StateMachine.StateTransition(M_StateMachine.idleState);
            
            return;
        }
        if (damagable.Hp > 0)
        {
            M_StateMachine.Excute();
        }
    }
    protected override void OnHpChange(int damage)
    {
        base.OnHpChange(damage);
    }

    // TODO : ���Ͱ� �׾��� �� ������, ����ġ, ��� ��� ���� ��������
    // TODO : ������ �����ϰ�?
    protected override void OnDeath()
    {
        nav.isStopped = true;
        ItemData dropItem = ItemManager.Instance.GetItemData(8);

        GameObject rewardItemOBJ = Resources.Load<GameObject>($"Items/{dropItem.Item_Name}");
        GameObject itemObj = Instantiate(rewardItemOBJ, transform.position + new Vector3(0, 0.5f, 0), rewardItemOBJ.transform.rotation);
        Item rewardItem = itemObj.GetComponent<Item>();
        rewardItem?.Initialize(ItemManager.Instance.GetItemData(monsterData.Item_Reward));
        UserStatManager.Instance.UpdateExp(rewardExp);
        //item.GetComponent<Rigidbody>().AddForce(Vector3.up * 20f, ForceMode.Impulse);

        base.OnDeath();
    }
}
