using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{


    public AudioClip clickSound;
    public AudioClip refreshSound;
    public AudioClip purchaseSound;
    public AudioClip bossApproachSound;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

    }
    public void PlayClickSound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    public void PlayRefreshSound()
    {
        if (refreshSound != null)
            audioSource.PlayOneShot(refreshSound);
    }

    public void PlayPurchaseSound()
    {
        if (purchaseSound != null)
            audioSource.PlayOneShot(purchaseSound);
    }

    public void PlayBossApproachSound()
    {
        if (bossApproachSound != null)
            audioSource.PlayOneShot(bossApproachSound);


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
