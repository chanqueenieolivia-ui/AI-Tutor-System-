using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OperaTrainingManager : MonoBehaviour
{
    public Animator instructorAnimator;
    public Animator studentAnimator;
    public AudioSource demoAudio;
    public AudioSource studentAudio;
    public Text statusText;
    public Dropdown roleDropdown;
    public Button startDemoButton;
    public Button quitButton;

    private Dictionary<string, float> historyScores = new Dictionary<string, float>();
    private string currentRole = "呂方";
    private string currentStage = "Stage1";

    void Start()
    {
        SetupUI();
        LoadHistory();
    }

    void SetupUI()
    {
        roleDropdown.options.Clear();
        roleDropdown.options.Add(new Dropdown.OptionData("南方"));
        roleDropdown.options.Add(new Dropdown.OptionData("呂方"));
        roleDropdown.onValueChanged.AddListener(delegate { SelectRole(roleDropdown.value); });

        startDemoButton.onClick.AddListener(() => StartDemo());
        quitButton.onClick.AddListener(() => QuitProgram());
    }

    void SelectRole(int index)
    {
        currentRole = roleDropdown.options[index].text;
        statusText.text = "已選擇角色: " + currentRole;
    }

    void StartDemo()
    {
        statusText.text = "導師 Demo 開始: " + currentRole;
        instructorAnimator.Play(currentRole + "_Demo");
        demoAudio.Play();
    }

    void Update()
    {
        if (studentAudio.isPlaying)
        {
            ComparePerformance();
        }
    }

    void ComparePerformance()
    {
        float poseScore = ComparePose();
        float lipScore = CompareLipSync();
        float rhythmScore = CompareTiming();

        float totalScore = (poseScore + lipScore + rhythmScore) / 3f;
        statusText.text = "評分: " + totalScore.ToString("F2");

        SaveHistory(currentRole, totalScore);

        if (poseScore < 0.7f) VoicePrompt("手勢要準啲");
        if (lipScore < 0.8f) VoicePrompt("口型要跟返音樂");
        if (rhythmScore < 0.75f) VoicePrompt("拍子要準啲");
    }

    float ComparePose()
    {
        // 假設比對手臂角度
        float error = Mathf.Abs(
            studentAnimator.GetBoneTransform(HumanBodyBones.RightUpperArm).rotation.eulerAngles.x -
            instructorAnimator.GetBoneTransform(HumanBodyBones.RightUpperArm).rotation.eulerAngles.x
        );
        return Mathf.Clamp01(1f - error / 30f); // 誤差越細分數越高
    }

    float CompareLipSync()
    {
        // 模擬口型比對
        return Random.Range(0.6f, 0.95f);
    }

    float CompareTiming()
    {
        // 模擬節奏比對
        return Random.Range(0.7f, 0.9f);
    }

    void VoicePrompt(string message)
    {
        Debug.Log("提示: " + message);
        // 可以加 Text-to-Speech 輸出
    }

    void SaveHistory(string role, float score)
    {
        historyScores[role] = score;
        PlayerPrefs.SetFloat(role + "_Score", score);
        PlayerPrefs.Save();
    }

    void LoadHistory()
    {
        foreach (string role in new string[] { "南方", "呂方" })
        {
            if (PlayerPrefs.HasKey(role + "_Score"))
            {
                historyScores[role] = PlayerPrefs.GetFloat(role + "_Score");
                statusText.text += "\n歷史分數 (" + role + "): " + historyScores[role];
            }
        }
    }

    void QuitProgram()
    {
        statusText.text = "程式已退出";
        Application.Quit();
    }
}
