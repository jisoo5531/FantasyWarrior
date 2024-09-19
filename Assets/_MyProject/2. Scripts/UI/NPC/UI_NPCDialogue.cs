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
    [Header("버튼")]
    public Button prevButton;
    public Button nextButton;
    public Button rejectQuestButton;
    public Button acceptQuestButton;
    [Header("플레이어 UI")]
    public GameObject playerUI;    

    private int quest_ID;
    private List<NPCDialogData> dialogList = new List<NPCDialogData>();

    private bool isNextDialog = false;
    private bool isPrevDialog = false;
    private bool isDialogEnd = false;

    /// <summary>
    /// 현재 대화내용 진행도
    /// </summary>
    private int dialogIndex = 0;
    private void Awake()
    {
        prevButton.onClick.AddListener(OnClickPrevDialog);
        nextButton.onClick.AddListener(OnClickNextDialog);
        rejectQuestButton.onClick.AddListener(OnClickRejectButton);
        acceptQuestButton.onClick.AddListener(OnClickAcceptButton);
    }
    private void OnEnable()
    {
        GameManager.inputActions.PlayerActions.DialogProgress.performed += OnPressNextDialog;        
        this.quest_ID = NPCManager.Instance.GetQuestIDFromNPC(this.NPC_ID);
        this.dialogList = NPCManager.Instance.GetDialogList(this.NPC_ID);
        NPC_NameText.text = NPCManager.Instance.GetNPCName(this.NPC_ID);
        
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
    /// 대화 진행
    /// </summary>
    private void StartDialogue()
    {        
        StartCoroutine(InprogressDialogue());
    }
    IEnumerator InprogressDialogue()
    {
        dialogIndex = 0;
        while (true)
        {
            prevButton.gameObject.SetActive(dialogIndex < dialogList.Count - 1);
            nextButton.gameObject.SetActive(dialogIndex < dialogList.Count - 1);
            rejectQuestButton.gameObject.SetActive(dialogIndex >= dialogList.Count - 1);
            acceptQuestButton.gameObject.SetActive(dialogIndex >= dialogList.Count - 1);
            
            if (isDialogEnd)
            {
                break;
            }
            if (dialogIndex < dialogList.Count)
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
        isDialogEnd = true;
    }
    /// <summary>
    /// 퀘스트 수락 버튼 클릭
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
