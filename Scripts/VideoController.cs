using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Optional: Play video at the start
        videoPlayer.Play();
    }

    public void PlayCutscene()
    {
        videoPlayer.Play();
    }

    public void StopCutscene()
    {
        videoPlayer.Stop();
    }
}