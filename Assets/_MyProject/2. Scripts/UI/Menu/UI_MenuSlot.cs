using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_MenuSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string currentMenuName;
    public GameObject activePanel;
    public TMP_Text labelText;    

    private void Awake()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnClickActiveAction);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        labelText.text = currentMenuName;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        labelText.text = string.Empty;
    }

    private void OnClickActiveAction()
    {
        if (activePanel == null)
        {
            // 게임 종료
            ApplicationQuit.Quit();
        }
        else
        {
            activePanel.SetActive(true);
            PanelManager.Instance.CloseOtherPanels(activePanel);
            PanelManager.Instance.UpdateUIState();
        }        
        transform.parent.parent.gameObject.SetActive(false);
    }
}
