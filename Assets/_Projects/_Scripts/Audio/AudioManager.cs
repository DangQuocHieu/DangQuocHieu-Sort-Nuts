using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : PersistentSingleton<AudioManager>, IMessageHandle
{
    [Header("Audio Mixers")]
    [SerializeField] AudioMixer _audioMixer;

    [Header("SFX Audio")]
    [SerializeField] private Audio[] _sfxAudios;

    private Dictionary<AudioName, AudioSource> _sfxAudioSourcePool = new Dictionary<AudioName, AudioSource>();
    [Header("Music Audio")]
    [SerializeField] private Audio[] _musicAudios;
    private AudioSource _musicSource;

    private float previousPlayTime = 0f;
    [SerializeField] private float sfxCooldown = 0.08f;

    protected override void Awake()
    {
        base.Awake();
        InitializeAudioSourcePool();
        ApplyAudioMixer();
    }
    void OnEnable()
    {
        MessageManager.AddSubscriber(GameMessageType.OnNutMoved, this);
        MessageManager.AddSubscriber(GameMessageType.OnBoltCompleted, this);
        MessageManager.AddSubscriber(GameMessageType.OnLevelCompleted, this);
    }

    void OnDisable()
    {
        MessageManager.RemoveSubscriber(GameMessageType.OnNutMoved, this);
        MessageManager.RemoveSubscriber(GameMessageType.OnBoltCompleted, this);
        MessageManager.RemoveSubscriber(GameMessageType.OnLevelCompleted, this);
    }


    void InitializeAudioSourcePool()
    {
        _sfxAudioSourcePool = new Dictionary<AudioName, AudioSource>();
        foreach (var audio in _sfxAudios)
        {
            AudioSource AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSource.clip = audio.Clip;
            AudioSource.loop = false;
            AudioSource.volume = audio.Volume;
            AudioSource.pitch = audio.Pitch;
            AudioSource.priority = audio.Priority;
            _sfxAudioSourcePool.Add(audio.AudioName, AudioSource);
        }
        _musicSource = gameObject.AddComponent<AudioSource>();
    }

    void ApplyAudioMixer()
    {
        if (_audioMixer == null) return;
        foreach (var source in _sfxAudioSourcePool.Values)
        {
            source.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("SFX")[0];
        }
        _musicSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Music")[0];
    }

    public void PlayMusic(AudioName name, float fadeDuration = 1f)
    {
        foreach (var audio in _musicAudios)
        {
            if (audio.AudioName == name)
            {
                if (_musicSource.clip == audio.Clip && _musicSource.isPlaying)
                    return;

                if (_musicSource.isPlaying)
                    _musicSource.Stop();

                _musicSource.clip = audio.Clip;
                _musicSource.volume = 0f; // Bắt đầu từ 0
                _musicSource.pitch = audio.Pitch;
                _musicSource.loop = true;
                _musicSource.priority = audio.Priority;

                AudioListener.pause = false;
                _musicSource.Play();

                // Fade to volume trong thời gian fadeDuration
                _musicSource.DOFade(audio.Volume, fadeDuration);
                return;
            }
        }
    }


    public void PauseMusic(object[] datas)
    {
        _musicSource.Pause();
    }

    public void ContinuePlayMusic(object[] datas)
    {
        _musicSource.UnPause();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }



    public void PlaySFX(AudioName name)
    {
        if (!_sfxAudioSourcePool.ContainsKey(name)) return;

        AudioSource source = _sfxAudioSourcePool[name];
        if (source.isPlaying) source.Stop();

        source.PlayOneShot(source.clip);
    }


    public void PlaySFXPerFrame(AudioName name)
    {
        AudioSource source = _sfxAudioSourcePool[name];
        if (source.isPlaying) return;
        source.PlayOneShot(source.clip);
    }

    public void StopSFX(AudioName name)
    {
        if (_sfxAudioSourcePool.ContainsKey(name))
        {
            _sfxAudioSourcePool[name].Stop();
        }
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case GameMessageType.OnNutMoved:
                PlaySFX(AudioName.PopSFX);
                break;

            case GameMessageType.OnBoltCompleted:
                PlaySFX(AudioName.BotCompleteSFX);
                break;

            case GameMessageType.OnLevelCompleted:
                PlaySFX(AudioName.LevelCompleteSFX);
                break;
        }
    }

}