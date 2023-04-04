using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscussionPanelOpen : MonoBehaviour
{
    private void OnEnable()
    {
        BaseUIManager.Instance.BaseNewClueEffectsCheck();
        BaseUIManager.Instance.BaseNewPuzzleEffectsCheck();
    }
}
