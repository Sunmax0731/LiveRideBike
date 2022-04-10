using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> TabList;
    [SerializeField] private GameObject TabObject;
    [SerializeField] private int TabIndex = 0;

    public void EnableTab(int tabIndex)
    {
        TabList.ForEach(tab => tab.SetActive(false));
        if (tabIndex == -1)
        {
            EnableUI();
            return;
        }
        TabIndex = tabIndex;
        TabList[TabIndex].SetActive(true);
    }
    private void EnableUI()
    {
        TabObject.SetActive(!TabObject.activeSelf);
        if (TabObject.activeSelf) TabList[TabIndex].SetActive(true);
    }
}
