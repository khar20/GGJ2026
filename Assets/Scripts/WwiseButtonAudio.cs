using UnityEngine;

public class WwiseButtonAudio : MonoBehaviour
{
    public AK.Wwise.Event clickEvent;

    public void PlayClickSound()
    {
        clickEvent.Post(gameObject);
    }
}