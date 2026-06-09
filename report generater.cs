using UnityEngine;
using System.IO;

public class ReportGenerator : MonoBehaviour
{
    public string studentName = "學員A";
    public string role = "呂方";
    public string stage = "Stage2";
    public float score = 82.5f;

    public void GenerateReport()
    {
        var report = new
        {
            studentName = studentName,
            role = role,
            stage = stage,
            score = score,
            errors = new[] {
                new { movement = "右手角度", error = "偏差 12°", comment = "手要再高啲" },
                new { movement = "口型同步", error = "LipSync 75%", comment = "口型要準啲" },
                new { movement = "節奏", error = "快咗 0.3s", comment = "慢啲，跟返拍子" }
            },
            suggestions = new[] {
                "用氣穩定高音",
                "面部表情投入",
                "重心要平衡"
            },
            history = new {
                Stage1 = 78.0f,
                Stage2 = 82.5f
            },
            upgradeLevel = "Level 2"
        };

        string json = JsonUtility.ToJson(report, true);
        string path = Application.persistentDataPath + "/TrainingReport.json";
        File.WriteAllText(path, json);

        Debug.Log("報告已生成: " + path);
    }
}
