using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    void Start()
    {
        videoPlayer.source = VideoSource.Url;

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "CorpseMedia Studio Intro.mp4");

        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Prepare();
    }


    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video prepared — now playing.");
        vp.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished — loading next scene.");
        SceneManager.LoadScene("Main Menu");
    }
}
