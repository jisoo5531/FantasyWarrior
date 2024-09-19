using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_DialogSelectElement : MonoBehaviour
{
    public TMP_Text dialogNameLabel;

    public void Initialize(string nameLabel)
    {
        dialogNameLabel.text = nameLabel;
    }
}
