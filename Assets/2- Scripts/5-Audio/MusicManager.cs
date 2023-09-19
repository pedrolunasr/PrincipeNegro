using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager mManager;
    
    public AudioSource aSource;
    public AudioClip[] listAudioClip;
    private int defultSong = 1;
    
    private void Awake()
    {
        if(mManager == null)
        {
            mManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Start()
    {
        aSource = GetComponent<AudioSource>();
        this.PlaySound(defultSong);
    }

    public void PlaySound(int p){
        
        if(aSource.clip != listAudioClip[p])
        {
            aSource.clip = listAudioClip[p];
            aSource.Play();
        }

    }

}
