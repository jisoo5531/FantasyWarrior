using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class NPCTalkQuestData
{
    /// <summary>
    /// ����� �Ǵ� NPC
    /// <para>��, ���� ���Ϸ� ���� �ϴ� NPC ID</para>
    /// </summary>
    public int NPC_ID { get; set; }
    public int Quest_ID { get; set; }

    public NPCTalkQuestData(DataRow row) : this
        (
            int.Parse(row["NPC_ID"].ToString()),
            int.Parse(row["Quest_ID"].ToString())
        )
    { }
    public NPCTalkQuestData(int nPC_ID, int quest_ID)
    {
        this.NPC_ID = nPC_ID;
        this.Quest_ID = quest_ID;
    }
}
