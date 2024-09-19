using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class NPCDialogData
{
    public int Dialog_ID { get; set; }
    public int NPC_ID { get; set; }
    public string dialogText { get; set; }
    public int NextDialog_ID { get; set; }

    public NPCDialogData(DataRow row) : this
        (
            int.Parse(row["Dialogue_ID"].ToString()),
            int.Parse(row["NPC_ID"].ToString()),
            row["Text"].ToString(),
            int.Parse(row["NEXT_DIALOGUE_ID"].ToString())
        )
    { }

    public NPCDialogData(int dialog_ID, int nPC_ID, string dialogText, int nextDialog_ID)
    {
        this.Dialog_ID = dialog_ID;
        this.NPC_ID = nPC_ID;
        this.dialogText = dialogText;
        this.NextDialog_ID = nextDialog_ID;
    }
}
