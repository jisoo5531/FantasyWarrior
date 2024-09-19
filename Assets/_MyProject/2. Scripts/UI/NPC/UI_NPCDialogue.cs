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
    public GameObject DialogSelctContent;
    public GameObject dialogDailyPrefab;
    public GameObject dialogEndPrefab;
    public GameObject dialogQuestPrefab;
    [Header("��ư")]
    public Button prevButton;
    public Button nextButton;
    public Button rejectQuestButton;
    public Button acceptQuestButton;
    [Header("�÷��̾� UI")]
    public GameObject playerUI;    

    private List<int> quests_ID = new List<int>();    
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

    private bool isNextDialog = false;
    private bool isPrevDialog = false;
    private bool isDialogEnd = false;

    /// <summary>
    /// ���� ��ȭ���� ���൵
    /// </summary>
    private int dialogIndex = 0;
    private void Awake()
    {
        prevButton.onClick.AddListener(OnClickPrevDialog);
        nextButton.onClick.AddListener(OnClickNextDialog);
        rejectQuestButton.onClick.AddListener(OnClickRejectButton);
        acceptQuestButton.onClick.AddListener(OnClickAcceptButton);
        
    }
    public void Initialize()
    {
        this.quests_ID = NPCManager.Instance.GetQuestIDFromNPC(this.NPC_ID);

        DialogClassify();
        Debug.Log($"{dailyDialogList.Count}, {questStartDL_List.Count}, {questEndDL_List.Count}");

        NPC_NameText.text = NPCManager.Instance.GetNPCName(this.NPC_ID);
    }
    
    private void OnEnable()
    {
        GameManager.inputActions.PlayerActions.DialogProgress.performed += OnPressNextDialog;                        
        
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
        DialogSelctContent.SetActive(true);
        StartCoroutine(SelectDialog());
        
    }
    /// <summary>
    /// ��ȭ ���� â���� ���ϴ� ��ȭ ���� ��, ����
    /// </summary>
    /// <returns></returns>
    IEnumerator SelectDialog()
    {
        while (true)
        {

        }
        StartCoroutine(InprogressDialogue());
    }
    IEnumerator InprogressDialogue()
    {
        dialogIndex = 0;
        while (true)
        {
            prevButton.gameObject.SetActive(dialogIndex < questStartDL_List.Count - 1);
            nextButton.gameObject.SetActive(dialogIndex < questStartDL_List.Count - 1);
            rejectQuestButton.gameObject.SetActive(dialogIndex >= questStartDL_List.Count - 1);
            acceptQuestButton.gameObject.SetActive(dialogIndex >= questStartDL_List.Count - 1);
            
            if (isDialogEnd)
            {
                break;
            }
            if (dialogIndex < questStartDL_List.Count)
            {
                DialogueText.text = questStartDL_List[dialogIndex].dialogText;
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
    /// �ϻ� ��ȭ Ŭ��
    /// </summary>
    private void OnClickDailyDialog()
    {

    }
    /// <summary>
    /// ��ȭ ���� Ŭ��
    /// </summary>
    private void OnClickEndDialog()
    {

    }

    /// <summary>
    /// ���� ��ư Ŭ��
    /// </summary>
    private void OnClickPrevDialog()
    {
        isPrevDialog = true;
        dialogIndex--;
    }
    /// <summary>
    /// ���� ��ư Ŭ��
    /// </summary>
    private void OnClickNextDialog()
    {
        isNextDialog = true;
        dialogIndex++;
    }
    /// <summary>
    /// ����Ʈ ���� ��ư Ŭ��
    /// </summary>
    private void OnClickRejectButton()
    {        
        isDialogEnd = true;
    }
    /// <summary>
    /// ����Ʈ ���� ��ư Ŭ��
    /// </summary>
    private void OnClickAcceptButton()
    {        
        isDialogEnd = true;
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
