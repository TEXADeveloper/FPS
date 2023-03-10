using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private int amount;
    private AudioSource[] sources;
    private int selected = 0;

    void Start()
    {
        sources = new AudioSource[amount];
        for (int i = 0 ; i < amount ; i++)
            sources[i] = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
            return;
        sources[selected].PlayOneShot(clip);
        selected++;
        if (selected >= amount)
            selected = 0;
    }
}
