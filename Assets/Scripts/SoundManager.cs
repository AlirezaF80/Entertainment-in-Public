using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure only one instance of SoundManager exists
        if (instance == null)
        {
            instance = this;
            // Get the AudioSource component attached to this GameObject
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    // Play a sound clip
    public void PlaySound(AudioClip soundClip)
    {
        if (soundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
        else
        {
            Debug.LogWarning("Sound clip or audio source is null.");
        }
    }
}
