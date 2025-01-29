using UnityEngine;

using SerapKeremGameTools._Game._AudioSystem;

public class AudioTest : MonoBehaviour
{
    [SerializeField]
    private string[] audioName;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlayAudio(audioName[0]);
        }
    }

}

