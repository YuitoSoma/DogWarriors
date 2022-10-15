using UnityEngine;
using UnityEngine.Audio;

public class AnimationEventStepPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = CreateAudioSource();
    }

    public void Play(string eventName)
    {
        audioSource.Play();
    }

    private AudioSource CreateAudioSource()
    {
        var audioGameObject = new GameObject();
        audioGameObject.name = "AnimationEventSEPlayer";
        audioGameObject.transform.SetParent(gameObject.transform);

        var audioSource = audioGameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;

        return audioSource;
    }
}