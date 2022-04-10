using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuPresenter : MonoBehaviour
{
    [SerializeField] private List<Button> TabButtonList;

    [Header("Component")]
    [SerializeField] private MainMenuManager _MainMenuMangaer;

    void Start()
    {
        //タブボタンのクリックイベント
        foreach (var (item, index) in TabButtonList.Select((item, index) => (item, index)))
        {
            item.onClick.AsObservable().Subscribe(_ => _MainMenuMangaer.EnableTab(index)).AddTo(this);
        }
        _MainMenuMangaer.EnableTab(-1);

        //ESCキーでメニューのオンオフ
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => _MainMenuMangaer.EnableTab(-1)).AddTo(this);
    }
}