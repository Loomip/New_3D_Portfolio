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

    // "School" ������ ��ȯ
    public void SceneStart()
    {
        LoadSceneManager.LoadScene(nextScene);
    }

    // ���ø����̼� ����
    public void Clear()
    {
        Application.Quit();
    }

    // �̽������� Ű�� ������ �ɼ� �޴��� �ݽ��ϴ�.
    void Start()
    {
        txt_Title.text = DataManager.instance.GetWordData("Title");
        txt_Start.text = DataManager.instance.GetWordData("Start");
        txt_Exit.text = DataManager.instance.GetWordData("Exit");

        //SoundManager.instance.PlayBgm(e_Bgm.TitleSound);
    }
}
