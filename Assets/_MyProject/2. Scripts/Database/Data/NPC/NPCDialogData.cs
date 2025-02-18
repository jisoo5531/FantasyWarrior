using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class NPCDialogData
{
    public int Dialog_ID { get; set; }
    public int NPC_ID { get; set; }
    public int Quest_ID { get; set; }
    public string dialogText { get; set; }
    public int NextDialog_ID { get; set; }
    public int Order { get; set; }
    public DialogStatus Status { get; set; }

    public NPCDialogData(DataRow row) : this
        (
            int.Parse(row["Dialogue_ID"].ToString()),
            int.Parse(row["NPC_ID"].ToString()),
            int.TryParse(row["Quest_ID"].ToString(), out int questid) ? questid : 0,
            row["Text"].ToString(),
            int.TryParse(row["NEXT_DIALOGUE_ID"].ToString(), out int nextDL_ID) ? nextDL_ID : 0,
            int.Parse(row["Dialog_Order"].ToString()),
            (DialogStatus)System.Enum.Parse(typeof(DialogStatus), row["Status"].ToString())            
        )
    { }

    public NPCDialogData(int dialog_ID, int nPC_ID, int quest_ID, string dialogText, int nextDialog_ID, int order, DialogStatus status)
    {
        this.Dialog_ID = dialog_ID;
        this.NPC_ID = nPC_ID;
        this.Quest_ID = quest_ID;
        this.dialogText = dialogText;
        this.NextDialog_ID = nextDialog_ID;
        this.Order = order;
        this.Status = status;
    }
}
public enum DialogStatus
{
    Talk,
    QuestStart,
    QuestEnd
}