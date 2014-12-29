using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager
{

    private static string RESOURCE_AUDIO_ROOT = "Tearoom6/Audio/";
    private static Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    /// <summary>
    /// Initialize by the specified resourceAudioRoot.
    /// </summary>
    /// <param name="resourceAudioRoot">Resource audio root dir.</param>
    public static void Initialize(string resourceAudioRoot)
    {
        RESOURCE_AUDIO_ROOT = resourceAudioRoot;
    }

    /// <summary>
    /// Play audio once.
    /// </summary>
    /// <param name="audioSource">Audio source.</param>
    /// <param name="fileName">File name.</param>
    /// <param name="volume">Volume.</param>
    public static void PlayOneShot(AudioSource audioSource, string fileName, float volume = 1.0f)
    {
        if (!audioClips.ContainsKey(fileName))
        {
            LoadAudioClip(fileName);
        }
        AudioClip audioClip = audioClips[fileName];
        audioSource.PlayOneShot(audioClip, volume);
    }

    /// <summary>
    /// Loads the audio clip.
    /// </summary>
    /// <returns>The audio clip.</returns>
    /// <param name="fileName">File name.</param>
    private static AudioClip LoadAudioClip(string fileName)
    {
        AudioClip audioClip = Resources.Load(RESOURCE_AUDIO_ROOT + fileName) as AudioClip;
        if (audioClip != null)
        {
            audioClips.Add(fileName, audioClip);
            Logger.Info(audioClip.name + " AudioClip is loaded.");
        }
        return audioClip;
    }

}
