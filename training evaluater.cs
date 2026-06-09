public class TrainingEvaluator : MonoBehaviour
{
    public Animator instructorAnimator;
    public Animator studentAnimator;
    public AudioSource demoAudio;
    public AudioSource studentAudio;

    void Update()
    {
        ComparePose();
        CompareLipSync();
        CompareTiming();
    }

    void ComparePose()
    {
        float angleError = CalculateJointError(studentAnimator, instructorAnimator);
        if (angleError > 5f) VoicePrompt("手要再高啲");
    }

    void CompareLipSync()
    {
        float lipAccuracy = LipSyncAnalyzer.Compare(studentAudio, demoAudio);
        if (lipAccuracy < 0.8f) VoicePrompt("口型要準啲");
    }

    void CompareTiming()
    {
        float timingError = RhythmAnalyzer.Compare(studentAudio, demoAudio);
        if (timingError > 0.2f) VoicePrompt("慢啲，跟返拍子");
    }

    void VoicePrompt(string message)
    {
        Debug.Log("提示: " + message);
        // 可以加 Text-to-Speech 輸出
    }

    float CalculateJointError(Animator student, Animator instructor)
    {
        // 假設比對手臂角度
        return Mathf.Abs(student.GetBoneTransform(HumanBodyBones.RightUpperArm).rotation.eulerAngles.x -
                         instructor.GetBoneTransform(HumanBodyBones.RightUpperArm).rotation.eulerAngles.x);
    }
}
