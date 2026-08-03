using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{
    [SerializeField] internal AudioListener m_Player_Listener;
    [SerializeField] internal AudioSource m_BG_Audio;
    [SerializeField] internal AudioSource m_Click_Audio;
    [SerializeField] internal AudioSource m_Win_Audio;
    [SerializeField] internal AudioSource m_Bonus_Audio;
    [SerializeField] internal AudioSource m_FreeSpin_Audio;
    [SerializeField] internal AudioSource m_Spin_Audio;

    private List<AudioSource> allSources = new List<AudioSource>();
    private readonly Dictionary<AudioSource, bool> preFocusMuteState = new Dictionary<AudioSource, bool>();
    private bool isForceMuted = false;

    internal void InitialAudioSetup()
    {
        if (m_BG_Audio) m_BG_Audio.Play();

        allSources.Clear();
        if (m_BG_Audio) allSources.Add(m_BG_Audio);
        if (m_Click_Audio) allSources.Add(m_Click_Audio);
        if (m_Win_Audio) allSources.Add(m_Win_Audio);
        if (m_Bonus_Audio) allSources.Add(m_Bonus_Audio);
        if (m_FreeSpin_Audio) allSources.Add(m_FreeSpin_Audio);
        if (m_Spin_Audio) allSources.Add(m_Spin_Audio);
    }

    // Focus-driven — called from BOTH OnFocusChanged (WebGL/JS path) and OnApplicationFocus below.
    internal void SetMuteAll(bool forceMute)
    {
        if (forceMute == isForceMuted) return;
        isForceMuted = forceMute;

        foreach (var source in allSources)
        {
            if (source == null) continue;
            if (forceMute)
            {
                preFocusMuteState[source] = source.mute;
                source.mute = true;
            }
            else
            {
                source.mute = preFocusMuteState.TryGetValue(source, out bool prevMuted) ? prevMuted : source.mute;
            }
        }
    }

    // Native/editor focus path — calls the SAME method the WebGL OnFocusChanged path calls.
    private void OnApplicationFocus(bool focus)
    {
        SetMuteAll(!focus);
    }

    internal void ToggleMute(bool toggle, string type = "all")
    {
        switch (type)
        {
            // case "bg":
            //     m_BG_Audio.mute = toggleBG;
            //     break;
            case "button":
                m_Click_Audio.mute = toggle;
                m_Spin_Audio.mute = toggle;
                break;
            case "wl":
                m_Win_Audio.mute = toggle;
                m_Bonus_Audio.mute = toggle;
                m_FreeSpin_Audio.mute = toggle;
                break;
            case "all":
               // m_BG_Audio.mute = toggle;
                m_Click_Audio.mute = toggle;
                m_Win_Audio.mute = toggle;
                m_Bonus_Audio.mute = toggle;
                m_FreeSpin_Audio.mute = toggle;
                m_Spin_Audio.mute = toggle;
                break;
        }
    }

    internal void ToggleBG_Mute(bool toggle)
    {
        m_BG_Audio.mute = toggle;
    }

}
