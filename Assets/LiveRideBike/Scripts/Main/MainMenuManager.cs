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

    void Start()
    {
    }
    public void EnableTab(int tabIndex)
    {
        TabList.ForEach(tab => tab.SetActive(false));
        if (tabIndex == -1)
        {
            Debug.Log(TabObject.activeSelf);
            TabObject.SetActive(!TabObject.activeSelf);
            return;
        }
        TabList[tabIndex].SetActive(true);
    }
}
