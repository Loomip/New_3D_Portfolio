using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_Title;
    [SerializeField] private TextMeshProUGUI txt_Start;
    [SerializeField] private TextMeshProUGUI txt_Exit;
    [SerializeField] private string nextScene;

    // "School" 씬으로 전환
    public void SceneStart()
    {
        LoadSceneManager.LoadScene(nextScene);
    }

    // 애플리케이션 종료
    public void Clear()
    {
        Application.Quit();
    }

    // 이스케이프 키를 누르면 옵션 메뉴를 닫습니다.
    void Start()
    {
        txt_Title.text = DataManager.instance.GetWordData("Title");
        txt_Start.text = DataManager.instance.GetWordData("Start");
        txt_Exit.text = DataManager.instance.GetWordData("Exit");

        //SoundManager.instance.PlayBgm(e_Bgm.TitleSound);
    }
}
