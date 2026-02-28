using UnityEngine;

public class BossEventRelay : MonoBehaviour
{
    [SerializeField] private BossController controller;

    private void Awake()
    {
        if (controller == null)
            controller = GetComponentInParent<BossController>();
    }

    public void OnIntroFinished() => controller?.OnIntroFinished();

    public void IntroStartInvuln() => controller?.IntroStartInvuln();
    public void IntroEndInvuln() => controller?.IntroEndInvuln();

    public void BlockStart() => controller?.BlockStart();
    public void BlockEnd() => controller?.BlockEnd();

    public void Phase2Start() => controller?.Phase2Start();
    public void Phase2End() => controller?.Phase2End();

    public void DownHitboxOn() => controller?.DownHitboxOn();
    public void DownHitboxOff() => controller?.DownHitboxOff();

    public void LeftHitboxOn() => controller?.LeftHitboxOn();
    public void LeftHitboxOff() => controller?.LeftHitboxOff();

    public void RightHitboxOn() => controller?.RightHitboxOn();
    public void RightHitboxOff() => controller?.RightHitboxOff();
}