using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _Instance;
    public static SoundManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj = new GameObject("[Script]: -- SoundManager Instance --");
                _Instance = obj.AddComponent<SoundManager>();
                DontDestroyOnLoad(obj);
            }
            return _Instance;
        }
    }

    [SerializeField]private AudioModelSO audioModelSO;
    public AudioSource sourceMusic;
    public float volumeBGM = 0.5f;
    public float volumeSFX = 0.5f;

    public void Init(AudioModelSO _audioModelSO)
    {
        audioModelSO = _audioModelSO;
        if (sourceMusic != null) Destroy(sourceMusic.gameObject);
    }

    public void PlayAudioSource(string _name)
    {
        if(audioModelSO == null)
        {
            audioModelSO = Resources.Load<AudioModelSO>("AudioModelSO_Game2_Waterpipe");
        }

        AudioModel audioModel = audioModelSO.GetAudioModel(_name);
        AudioClip audioClip = audioModel.audioClip;
        AudioSource source = CreateAudioSource(_name, audioModel.audioType);

        if (audioModel.audioType != AudioType.BGM)
        {
            source.PlayOneShot(audioClip);
            StartCoroutine(DestroyAudioAfterDelay(source.gameObject, audioClip.length));
        }
        //BGM Sound
        else
        {
            sourceMusic = source;
            sourceMusic.clip = audioClip;
            sourceMusic.playOnAwake = true;
            sourceMusic.loop = true;
            sourceMusic.volume = volumeBGM;
            sourceMusic.Play();
        }
    }

    private AudioSource CreateAudioSource(string _name, AudioType _audioType)
    {
        GameObject audio = new GameObject(_name);
        audio.transform.position = Vector3.zero;
        audio.transform.rotation = Quaternion.identity;
        audio.transform.SetParent(this.gameObject.transform);
        AudioSource sourceMusic = audio.AddComponent<AudioSource>();
        if (_audioType == AudioType.BGM) sourceMusic.volume = volumeBGM;
        else sourceMusic.volume = volumeSFX;
        audio.name = _name;
        return audio.GetComponent<AudioSource>();
    }

    private IEnumerator DestroyAudioAfterDelay(GameObject audioObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        // ทำลาย GameObject
        Destroy(audioObject);
    }

    public void SetVolumeBGM(float _volume)
    {
        volumeBGM = _volume;
        if (sourceMusic != null)
        {
            sourceMusic.volume = volumeBGM;
        }
        PlayerPrefs.SetFloat("VolumeBGM", volumeBGM);
    }
    public void SetVolumeSFX(float _volume)
    {
        volumeSFX = _volume;
        PlayerPrefs.SetFloat("VolumeSFX", volumeSFX);
    }
}
