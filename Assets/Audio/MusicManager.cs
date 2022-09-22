using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme;
    public AudioClip menuTheme;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(menuTheme, 2);
    }
}
