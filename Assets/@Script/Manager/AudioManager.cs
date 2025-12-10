using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip lineClearClip;
    [SerializeField]
    private AudioClip bestRecordClip;
    [SerializeField]
    private AudioClip placeBlockClip;
    [SerializeField]
    private AudioClip clickClip;
    [SerializeField]
    private AudioClip[] comboClips;

    private AudioSource audio;

    public static AudioManager Instance { private set; get; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audio = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }

    public void PlayPlaceBlockClip()
    {
        audio.PlayOneShot(placeBlockClip);
    }

    public void PlayLineClearClip()
    {
        audio.PlayOneShot(lineClearClip);
    }

    public void PlayBestRecordClip()
    {
        audio.PlayOneShot(bestRecordClip);
    }

    public void PlayComboClip(int index)
    {
        audio.PlayOneShot(comboClips[Mathf.Clamp(index,0, comboClips.Length-1)]);
    }

    public void PlayClickClip()
    {
        audio.PlayOneShot(clickClip);
    }
}
