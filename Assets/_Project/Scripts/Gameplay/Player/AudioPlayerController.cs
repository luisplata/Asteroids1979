using UnityEngine;

public class AudioPlayerController : MonoBehaviour
{
    public static AudioPlayerController Instance { get; private set; }

    [Header("References")] [SerializeField]
    private AudioSource audioSource;

    [SerializeField] private AudioClip shootClip, playerHitClip, thrustClip, thrustStopClip, rotateClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioPlayerController: AudioSource not assigned.");
            return;
        }

        audioSource.loop = false;
        audioSource.volume = 1f;
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;
        audioSource.PlayOneShot(clip);
    }

    private void Stop()
    {
        if (audioSource == null)
            return;

        audioSource.Stop();
    }

    public void ShootSound()
    {
        PlayOneShot(shootClip);
    }

    public void PlayPlayerHitSound()
    {
        PlayOneShot(playerHitClip);
    }

    public void ThrustStart()
    {
        PlayOneShot(thrustClip);
    }

    public void ThrustStop()
    {
        Stop();
    }

    public void Rotate()
    {
        PlayOneShot(rotateClip);
    }

    public void RotateStop()
    {
        Stop();
    }
}