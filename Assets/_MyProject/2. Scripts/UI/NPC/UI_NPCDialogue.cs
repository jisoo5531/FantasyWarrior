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
    [Header("텍스트")]
    public TMP_Text NPC_NameText;
    public TMP_Text DialogueText;
    [Header("대화 선택창")]
    public GameObject DialogSelectContent;
    public GameObject dialogDailyPrefab;
    public GameObject dialogEndPrefab;
    public GameObject dialogQuestPrefab;
    private Button dialogDailyButton;                   // 일상 대화 나누기 버튼         instantiate로 생성 후, 참조
    private Button dialogEndButton;                     // 대화 끝내기(GoodBye) 버튼     instantiate로 생성 후, 참조
    private List<Button> dialogQuestsButton = new();    // 퀘스트 대화 버튼              instantiate로 생성 후, 참조
    [Header("버튼")]
    public Button prevButton;
    public Button nextButton;
    public Button rejectQuestButton;
    public Button acceptQuestButton;
    public Button completeQuestButton;
    [Header("플레이어 UI")]
    public GameObject playerUI;    

    /// <summary>
    /// 이 npc가 주는 퀘스트들의 ID.
    /// </summary>
    private List<int> questID_List = new List<int>();    
    /// <summary>
    /// 그냥 대화를 걸 때의 대화
    /// </summary>
    private List<NPCDialogData> dailyDialogList = new List<NPCDialogData>();
    /// <summary>
    /// 퀘스트를 줄 때 대화
    /// </summary>
    private List<NPCDialogData> questStartDL_List = new List<NPCDialogData>();
    /// <summary>
    /// 퀘스트 완료할 때 대화
    /// </summary>
    private List<NPCDialogData> questEndDL_List = new List<NPCDialogData>();    

    /// <summary>
    /// 대화 넘기기를 했는지
    /// </summary>
    private bool isNextDialog = false;
    /// <summary>
    /// 이전 대화로 돌아가기
    /// </summary>
    private bool isPrevDialog = false;
    /// <summary>
    /// 대화 선택이 끝났는지
    /// </summary>
    private bool isDialogSelectEnd = false;
    /// <summary>
    /// 대화가 끝났는지
    /// </summary>
    private bool isDialogEnd = false;

    /// <summary>
    /// 현재 대화내용 진행도
    /// </summary>
    private int dialogIndex = 0;

    /// <summary>
    /// 현재 진행하고 있는 퀘스트 대화의 퀘스트 ID.
    /// <para>0이면 없는 ID</para>
    /// </summary>
    private int quest_ID = 0;

    private void Awake()
    {
        prevButton.onClick.AddListener(OnClickPrevDialog);
        nextButton.onClick.AddListener(OnClickNextDialog);
        rejectQuestButton.onClick.AddListener(OnClickRejectButton);
        acceptQuestButton.onClick.AddListener(OnClickAcceptButton);
        completeQuestButton.onClick.AddListener(OnClickCompleteQuestButton);
    }
    public void Initialize()
    {
        this.questID_List = NPCManager.Instance.GetQuestIDFromNPC(this.NPC_ID);

        DialogClassify();
        Debug.Log($"{dailyDialogList.Count}, {questStartDL_List.Count}, {questEndDL_List.Count}");

        NPC_NameText.text = NPCManager.Instance.GetNPCName(this.NPC_ID);
    }
    
    private void OnEnable()
    {
        GameManager.inputActions.PlayerActions.DialogProgress.performed += OnPressNextDialog;

        this.quest_ID = 0;
        isNextDialog = false;
        isPrevDialog = false;
        isDialogEnd = false;
        playerUI.gameObject.SetActive(false);                        

        StartDialogue();
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.DialogProgress.performed -= OnPressNextDialog;        

        playerUI.gameObject.SetActive(true);
    }
    /// <summary>
    /// 해당 대화가 일상 / 퀘스트 시작 / 퀘스트 완료 대화인지 분류
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

    #region 대화 진행(시작, 진행 중, 완료)
    /// <summary>
    /// 대화 진행
    /// </summary>
    private void StartDialogue()
    {
        DialogSelectContent.SetActive(true);
        SelectDialogInit();                
    }
    /// <summary>
    /// 대화 선택창 초기화(세팅)
    /// </summary>
    private void SelectDialogInit()
    {
        DialogSelectContent.ContentClear();
        dialogQuestsButton = new();
        Debug.Log($"퀘스트 몇개? : {questID_List.Count}");
        foreach (int questID in questID_List)
        {            
            QuestsData questData = QuestManager.Instance.GetQuestData(questID);            
            UI_DialogSelectElement dialogSelectElement = Instantiate(dialogQuestPrefab, DialogSelectContent.transform).GetComponent<UI_DialogSelectElement>();
            dialogSelectElement.Initialize(questData.Quest_Name);
            dialogQuestsButton.Add(dialogSelectElement.GetComponent<Button>());
        }
        dialogDailyButton = Instantiate(dialogDailyPrefab, DialogSelectContent.transform).GetComponent<Button>();
        dialogEndButton =  Instantiate(dialogEndPrefab, DialogSelectContent.transform).GetComponent<Button>();

        DialogSelectButtonInit();        
    }
    /// <summary>
    /// 대화창 선택 버튼 기능 넣기
    /// </summary>
    private void DialogSelectButtonInit()
    {
        Debug.Log($"버튼 몇개? : {dialogQuestsButton.Count}");
        for (int i = 0; i < dialogQuestsButton.Count; i++)
        {
            int index = i;
            dialogQuestsButton[i].onClick.AddListener(() => OnClickQuestDialog(index));
        }
        dialogDailyButton?.onClick.AddListener(OnClickDailyDialog);
        dialogEndButton?.onClick.AddListener(OnClickEndDialog);
    }
    /// <summary>
    /// 대화 선택 창에서 원하는 대화 선택 후, 진행
    /// </summary>
    /// <returns></returns>
    private void SelectDialog(DialogStatus status, int quest_ID = 0)
    {        
        switch (status)
        {
            case DialogStatus.Daily:
                StartCoroutine(InprogressDialogue(dailyDialogList, DialogStatus.Daily));
                break;
            case DialogStatus.QuestStart:
                StartCoroutine(InprogressDialogue(questStartDL_List, DialogStatus.QuestStart));
                break;
            case DialogStatus.QuestEnd:
                StartCoroutine(InprogressDialogue(questEndDL_List, DialogStatus.QuestEnd));
                break;
            default:
                break;
        }        
    }
    private IEnumerator InprogressDialogue(List<NPCDialogData> dialogList, DialogStatus status)
    {
        dialogIndex = 0;
        Debug.Log($"지금 무슨 상태? : {status}");
        while (true)
        {
            prevButton.gameObject.SetActive(dialogIndex < dialogList.Count - 1);
            nextButton.gameObject.SetActive(dialogIndex < dialogList.Count - 1);

            switch (status)
            {
                case DialogStatus.QuestStart:
                    rejectQuestButton.gameObject.SetActive(dialogIndex >= dialogList.Count - 1);
                    acceptQuestButton.gameObject.SetActive(dialogIndex >= dialogList.Count - 1);
                    break;
                case DialogStatus.QuestEnd:
                    rejectQuestButton.gameObject.SetActive(false);
                    acceptQuestButton.gameObject.SetActive(false);
                    prevButton.gameObject.SetActive(true);
                    completeQuestButton.gameObject.SetActive(dialogIndex >= dialogList.Count - 1);
                    break;
                default:
                    break;
            }
            if (isDialogEnd)
            {
                break;
            }
            if (dialogIndex >= dialogList.Count)
            {                
                if (status == DialogStatus.Daily)
                {
                    break;
                }
            }
            else
            {
                DialogueText.text = dialogList[dialogIndex].dialogText;
            }
            yield return new WaitUntil(() => isNextDialog || isPrevDialog || isDialogEnd);
            yield return new WaitForSeconds(0.1f);
            isPrevDialog = false;
            isNextDialog = false;            
        }
        EndDialogue();
    }
    private void EndDialogue()
    {        
        gameObject.SetActive(false);
    }
    #endregion
    
    /// <summary>
    /// 퀘스트 대화 클릭
    /// <para>몇 번째 대화를 클릭했는지 여부 판단</para>
    /// </summary>
    /// <param name="num"></param>
    private void OnClickQuestDialog(int num)
    {        
        this.quest_ID = questID_List[num];
        List<QuestProgress> userQuestList = QuestManager.Instance.questProgressList;        
        QuestProgress userQuest = userQuestList.Find(x => x.quest_Id == this.quest_ID);        
        if (userQuest == null)
        {
            Debug.Log("혹시 null?");
            // 현재 유저가 해당 퀘스트를 받지 않았다면
            SelectDialog(DialogStatus.QuestStart, questID_List[num]);
        }
        else
        {
            // 현재 유저가 해당 퀘스트를 수행 중이라면
            if (userQuest.IsQuestComplete())
            {
                // 해당 퀘스트가 완료 가능하다면
                SelectDialog(DialogStatus.QuestEnd, questID_List[num]);
            }            
        }        
    }
    
    /// <summary>
    /// 일상 대화 클릭
    /// </summary>
    private void OnClickDailyDialog()
    {
        SelectDialog(DialogStatus.Daily);        
    }
    /// <summary>
    /// 대화 종료 클릭
    /// </summary>
    private void OnClickEndDialog()
    {
        isDialogSelectEnd = true;
    }

    /// <summary>
    /// 이전 버튼 클릭
    /// </summary>
    private void OnClickPrevDialog()
    {
        isPrevDialog = true;
        dialogIndex--;
    }
    /// <summary>
    /// 다음 버튼 클릭
    /// </summary>
    private void OnClickNextDialog()
    {
        isNextDialog = true;
        dialogIndex++;
    }
    /// <summary>
    /// 퀘스트 거절 버튼 클릭
    /// </summary>
    private void OnClickRejectButton()
    {
        Debug.Log("거절 눌렀다.");
        isDialogEnd = true;
        this.quest_ID = 0;
    }
    /// <summary>
    /// 퀘스트 수락 버튼 클릭
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
        isNextDialog = context.ReadValueAsButton();
        dialogIndex++;
    }    
}
