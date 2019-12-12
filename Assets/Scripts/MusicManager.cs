using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	private AudioSource mAudioSrc;
    public bool end;
    public float MaxTimeout;
    private float timeout = 0;
    private bool isPaused;
	// Start is called before the first frame update
	void Start()
    {
		mAudioSrc = transform.Find("LilaMusic").GetComponent<AudioSource>();
        mAudioSrc.Play();
        mAudioSrc.Pause();
        isPaused = true;
        timeout = MaxTimeout;
        end = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (end)
        {
            mAudioSrc.Stop();
        } else
        {
            if (GameObject.Find("musicSymbol(Clone)") != null)
            {
                if (isPaused)
                {
                    mAudioSrc.UnPause();
                    isPaused = false;
                }

            }
            else if (!isPaused)
            {
                timeout -= Time.deltaTime;
                if (timeout < 0)
                {
                    mAudioSrc.Pause();
                    isPaused = true;
                    timeout = MaxTimeout;
                }
            }
        }
        
    }
}
