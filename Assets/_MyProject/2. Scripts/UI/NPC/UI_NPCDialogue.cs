using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UI_NPCDialogue : MonoBehaviour
{
    [Header("NPC ID")]
    public int NPC_ID;
    [Header("�ؽ�Ʈ")]
    public TMP_Text NPC_NameText;
    public TMP_Text DialogueText;
    [Header("��ȭ ����â")]
    public GameObject DialogSelectContent;
    public GameObject dialogDailyPrefab;
    public GameObject dialogEndPrefab;
    public GameObject dialogQuestPrefab;
    private Button dialogDailyButton;                   // �ϻ� ��ȭ ������ ��ư         instantiate�� ���� ��, ����
    private Button dialogEndButton;                     // ��ȭ ������(GoodBye) ��ư     instantiate�� ���� ��, ����
    private List<Button> dialogQuestsButton = new();    // ����Ʈ ��ȭ ��ư              instantiate�� ���� ��, ����
    [Header("��ư")]
    public Button prevButton;
    public Button nextButton;
    public Button rejectQuestButton;
    public Button acceptQuestButton;
    public Button completeQuestButton;
    [Header("�÷��̾� UI")]
    public GameObject playerUI;

    /// <summary>
    /// �� npc�� �ִ� ����Ʈ���� ID.
    /// </summary>
    private List<int> questID_List;
    /// <summary>
    /// �׳� ��ȭ�� �� ���� ��ȭ
    /// </summary>
    private List<NPCDialogData> dailyDialogList = new List<NPCDialogData>();
    /// <summary>
    /// ����Ʈ�� �� �� ��ȭ
    /// </summary>
    private List<NPCDialogData> questStartDL_List = new List<NPCDialogData>();
    /// <summary>
    /// ����Ʈ �Ϸ��� �� ��ȭ
    /// </summary>
    private List<NPCDialogData> questEndDL_List = new List<NPCDialogData>();

    /// <summary>
    /// ��ȭ �ѱ�⸦ �ߴ���
    /// </summary>
    private bool isNextDialog = false;
    /// <summary>
    /// ���� ��ȭ�� ���ư���
    /// </summary>
    private bool isPrevDialog = false;
    /// <summary>
    /// ��ȭ ������ ��������
    /// </summary>
    private bool isDialogSelectEnd = false;
    /// <summary>
    /// ��ȭ�� ��������
    /// </summary>
    private bool isDialogEnd = false;

    /// <summary>
    /// ���� ��ȭ���� ���൵
    /// </summary>
    private int dialogIndex = 0;

    /// <summary>
    /// ���� �����ϰ� �ִ� ����Ʈ ��ȭ�� ����Ʈ ID.
    /// <para>0�̸� ���� ID</para>
    /// </summary>
    private int quest_ID = 0;

    private void Awake()
    {
        InitializeButtonListeners();
    }
    private void InitializeButtonListeners()
    {
        prevButton.onClick.AddListener(OnClickPrevDialog);
        nextButton.onClick.AddListener(OnClickNextDialog);
        rejectQuestButton.onClick.AddListener(OnClickRejectButton);
        acceptQuestButton.onClick.AddListener(OnClickAcceptButton);
        completeQuestButton.onClick.AddListener(OnClickCompleteQuestButton);
    }
    public void Initialize()
    {
        ResetDialogState();
        DialogClassify();
        Debug.Log($"{dailyDialogList.Count}, {questStartDL_List.Count}, {questEndDL_List.Count}");

        NPC_NameText.text = NPCManager.Instance.GetNPCName(this.NPC_ID);
    }

    private void OnEnable()
    {
        GameManager.inputActions.PlayerActions.DialogProgress.performed += OnPressNextDialog;
        ResetDialogState();
        StartDialogue();
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.DialogProgress.performed -= OnPressNextDialog;

        playerUI.SetActive(true);
    }
    private void ResetDialogState()
    {
        questID_List = NPCManager.Instance.GetQuestIDFromNPC(this.NPC_ID);

        isNextDialog = false;
        isPrevDialog = false;
        isDialogEnd = false;
        playerUI.SetActive(false);
        quest_ID = 0;
    }
    /// <summary>
    /// �ش� ��ȭ�� �ϻ� / ����Ʈ ���� / ����Ʈ �Ϸ� ��ȭ���� �з�
    /// </summary>
    private void DialogClassify()
    {
        List<NPCDialogData> dialogList = NPCManager.Instance.GetDialogList(this.NPC_ID);        
        foreach (var dialog in dialogList)
        {
            switch (dialog.Status)
            {
                case DialogStatus.Daily:
                    dailyDialogList.Add(dialog);
                    break;
                case DialogStatus.QuestStart:
                    questStartDL_List.Add(dialog);
                    break;
                case DialogStatus.QuestEnd:
                    questEndDL_List.Add(dialog);
                    break;
                default:
                    break;
            }
        }
    }

    #region ��ȭ ����(����, ���� ��, �Ϸ�)
    /// <summary>
    /// ��ȭ ����
    /// </summary>
    private void StartDialogue()
    {
        DialogSelectContent.SetActive(true);
        SelectDialogInit();
    }
    /// <summary>
    /// ��ȭ ����â �ʱ�ȭ(����)
    /// </summary>
    private void SelectDialogInit()
    {
        DialogSelectContent.ContentClear();
        dialogQuestsButton.Clear();

        InitializeQuestButtons();

        InitializeDialogButtons();
    }
    private void InitializeQuestButtons()
    {
        #region �� NPC�� ����Ʈ Ȯ��
        foreach (int questID in questID_List)
        {
            QuestsData questData = QuestManager.Instance.GetQuestData(questID);
            UI_DialogSelectElement dialogSelectElement = Instantiate(dialogQuestPrefab, DialogSelectContent.transform).GetComponent<UI_DialogSelectElement>();
            dialogSelectElement.Initialize(questData.Quest_Name);
            dialogQuestsButton.Add(dialogSelectElement.GetComponent<Button>());
        }
        #endregion
        #region ���� ����Ʈ Ȯ��
        // ���� ����Ʈ�� �� ���� ������ ����Ʈ���� �� npc���� �Դٸ�
        foreach (NPCTalkQuestData tempNPCQuest in NPCManager.Instance.talkNpcQuestList)
        {
            if (tempNPCQuest.NPC_ID != this.NPC_ID)
            {
                continue;
            }
            QuestsData questData = QuestManager.Instance.GetQuestData(tempNPCQuest.Quest_ID);
            UI_DialogSelectElement dialogSelectElement = Instantiate(dialogQuestPrefab, DialogSelectContent.transform).GetComponent<UI_DialogSelectElement>();
            dialogSelectElement.Initialize(questData.Quest_Name);
            dialogQuestsButton.Add(dialogSelectElement.GetComponent<Button>());
        }
        #endregion
    }
    /// <summary>
    /// ��ȭâ ���� ��ư ��� �ֱ�
    /// </summary>
    private void InitializeDialogButtons()
    {
        dialogDailyButton = Instantiate(dialogDailyPrefab, DialogSelectContent.transform).GetComponent<Button>();
        dialogEndButton = Instantiate(dialogEndPrefab, DialogSelectContent.transform).GetComponent<Button>();

        for (int i = 0; i < dialogQuestsButton.Count; i++)
        {
            int index = i;
            dialogQuestsButton[i].onClick.AddListener(() => OnClickQuestDialog(index));
        }
        dialogDailyButton?.onClick.AddListener(OnClickDailyDialog);
        dialogEndButton?.onClick.AddListener(OnClickEndDialog);
    }
    /// <summary>
    /// ��ȭ ���� â���� ���ϴ� ��ȭ ���� ��, ����
    /// </summary>
    /// <returns></returns>
    private void SelectDialog(DialogStatus status, int quest_ID = 0)
    {        
        if (quest_ID == 0) return;

        List<NPCDialogData> dialogList = new List<NPCDialogData>();
        
        switch (status)
        {
            case DialogStatus.Daily:
                dialogList = dailyDialogList.FindAll(x => x.Quest_ID == quest_ID);                
                StartCoroutine(InprogressDialogue(dialogList, DialogStatus.Daily));
                break;
            case DialogStatus.QuestStart:
                dialogList = questStartDL_List.FindAll(x => x.Quest_ID == quest_ID);
                Debug.Log($"�굵 Ȯ�� {dialogList.Count}");
                Debug.Log($"�굵 Ȯ�� {questStartDL_List.Count}");
                StartCoroutine(InprogressDialogue(dialogList, DialogStatus.QuestStart));
                break;
            case DialogStatus.QuestEnd:
                dialogList = questEndDL_List.FindAll(x => x.Quest_ID == quest_ID);
                StartCoroutine(InprogressDialogue(dialogList, DialogStatus.QuestEnd));
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// ��ȭ ���뿡 ���缭 ��ȭ ���� ����
    /// </summary>
    /// <param name="dialogList">������ ��ȭ</param>
    /// <param name="status">�� ��ȭ�� ����Ʈ ���� / �� / �ϻ� ��ȭ����</param>
    /// <returns></returns>
    private IEnumerator InprogressDialogue(List<NPCDialogData> dialogList, DialogStatus status)
    {
        dialogIndex = 0;
            Debug.Log($"Ȯ�� : {dialogList.Count}");
        while (dialogIndex < dialogList.Count)
        {
            prevButton.gameObject.SetActive(dialogIndex > 0);
            nextButton.gameObject.SetActive(dialogIndex < dialogList.Count - 1);

            if (status == DialogStatus.QuestStart)
            {
                rejectQuestButton.gameObject.SetActive(dialogIndex >= dialogList.Count - 1);
                acceptQuestButton.gameObject.SetActive(dialogIndex >= dialogList.Count - 1);
            }
            else if (status == DialogStatus.QuestEnd)
            {
                completeQuestButton.gameObject.SetActive(dialogIndex >= dialogList.Count - 1);
            }

            DialogueText.text = dialogList[dialogIndex].dialogText;

            yield return new WaitUntil(() => isNextDialog || isPrevDialog || isDialogEnd);
            yield return new WaitForSeconds(0.1f);

            if (isNextDialog) dialogIndex++;
            if (isPrevDialog) dialogIndex--;

            isPrevDialog = false;
            isNextDialog = false;

            if (isDialogEnd)
            {
                Debug.Log("��ȭ ������.");
                break;
            }
        }
        EndDialogue();
    }
    private void EndDialogue()
    {
        rejectQuestButton.gameObject.SetActive(false);
        acceptQuestButton.gameObject.SetActive(false);
        completeQuestButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    #endregion

    /// <summary>
    /// ����Ʈ ��ȭ Ŭ��
    /// <para>�� ��° ��ȭ�� Ŭ���ߴ��� ���� �Ǵ�</para>
    /// </summary>
    /// <param name="num"></param>
    private void OnClickQuestDialog(int num)
    {
        Debug.Log(num + "��°");
        if (num < questID_List.Count)
        {
            this.quest_ID = questID_List[num];

            List<QuestProgress> userQuestList = QuestManager.Instance.questProgressList;
            QuestProgress userQuest = userQuestList.Find(x => x.quest_Id == this.quest_ID);
            if (userQuest == null)
            {
                Debug.Log("Ȥ�� null?");
                // ���� ������ �ش� ����Ʈ�� ���� �ʾҴٸ�
                SelectDialog(DialogStatus.QuestStart, questID_List[num]);
            }
            else if (userQuest.IsQuestComplete())
            {
                // ���� ������ �ش� ����Ʈ�� ���� ���̶��
                // �ش� ����Ʈ�� �Ϸ� �����ϴٸ�
                SelectDialog(DialogStatus.QuestEnd, questID_List[num]);
            }
        }
        else
        {
            List<NPCTalkQuestData> talkList = NPCManager.Instance.talkNpcQuestList;
            // �� NPC���� �� ��ȭ ����Ʈ �ִ��� Ȯ��
            NPCTalkQuestData talkQuest = talkList.Find(x => x.NPC_ID == this.NPC_ID);

            if (talkQuest != null)
            {
                // �ش�Ǵ� ����Ʈ�� ������
                this.quest_ID = talkQuest.Quest_ID;
                // �ش� ����Ʈ�� �Ϸ� ��ȭ ����.
                SelectDialog(DialogStatus.QuestEnd, talkQuest.Quest_ID);
            }
        }
    }

    #region Click / Press Listener

    /// <summary>
    /// �ϻ� ��ȭ Ŭ��
    /// </summary>
    private void OnClickDailyDialog()
    {
        SelectDialog(DialogStatus.Daily);
    }
    /// <summary>
    /// ��ȭ ���� Goodbye Ŭ��
    /// </summary>
    private void OnClickEndDialog()
    {
        isDialogSelectEnd = true;
    }

    /// <summary>
    /// ���� ��ư Ŭ��
    /// </summary>
    private void OnClickPrevDialog()
    {
        if (dialogIndex <= 0) return;
        isPrevDialog = true;        
    }
    /// <summary>
    /// ���� ��ư Ŭ��
    /// </summary>
    private void OnClickNextDialog()
    {
        isNextDialog = true;        
    }
    /// <summary>
    /// ����Ʈ ���� ��ư Ŭ��
    /// </summary>
    private void OnClickRejectButton()
    {
        Debug.Log("���� ������.");
        isDialogEnd = true;
        this.quest_ID = 0;
    }
    /// <summary>
    /// ����Ʈ ���� ��ư Ŭ��
    /// </summary>
    private void OnClickAcceptButton()
    {
        isDialogEnd = true;
        if (this.quest_ID != 0)
        {
            QuestManager.Instance.AcceptQuest(this.quest_ID);
        }
    }
    private void OnClickCompleteQuestButton()
    {
        isDialogEnd = true;
        if (this.quest_ID != 0)
        {
            QuestManager.Instance.QuestComplete(this.quest_ID);
        }
    }
    /// <summary>
    /// Enter or Space    
    /// </summary>
    /// <param name="context"></param>
    private void OnPressNextDialog(InputAction.CallbackContext context)
    {
        isNextDialog = true;                
    }
    #endregion
}
