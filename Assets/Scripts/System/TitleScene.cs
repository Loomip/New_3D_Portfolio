using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    [Header("�ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI txt_Title;
    [SerializeField] private TextMeshProUGUI txt_Start;
    [SerializeField] private TextMeshProUGUI txt_Option;
    [SerializeField] private TextMeshProUGUI txt_Exit;

    [Header("������")]
    [SerializeField] private string nextScene;

    [Header("�Ŵ����ӿ�����Ʈ")]
    [SerializeField] private GameObject option;

    // "School" ������ ��ȯ
    public void SceneStart()
    {
        // �Ѿ�� â�� ���������� ����
        option.SetActive(false);
        // ���������� �Ѿ
        LoadSceneManager.LoadScene(nextScene);
    }

    // ���ø����̼� ����
    public void Clear()
    {
        Application.Quit();
    }

    public void OptionButten()
    {
        option.SetActive(true);
    }

    public void ExitButten()
    {
        option.SetActive(false);
    }

    // �̽������� Ű�� ������ �ɼ� �޴��� �ݽ��ϴ�.
    void Start()
    {
        txt_Title.text = DataManager.instance.GetWordData("Title");
        txt_Start.text = DataManager.instance.GetWordData("Start");
        txt_Option.text = DataManager.instance.GetWordData("Option");
        txt_Exit.text = DataManager.instance.GetWordData("Exit");

        SoundManager.instance.PlayBgm(e_Bgm.Title);

        option.SetActive(false);
    }
}
