using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recording
{
    InstrumentBehaviour instrument;
    public float playStart;
    public float playEnd;
    public float volume;
    AudioSource audioSource;

    private Recording()
    {

    }

    public Recording(InstrumentBehaviour instrument)
    {
        this.instrument = instrument;
        this.audioSource = instrument.gameObject.GetComponent<AudioSource>();
        playEnd = -1;
        playStart = -1;
    }

    public void PlayRecording()
    {
        instrument.audioSource.volume = this.volume;
        instrument.audioSource.Play();
        instrument.audioSource.time = playStart;
    }

    public void Stop()
    {
        instrument.audioSource.Stop();

    }

    public void RecordingStart()
    {
        if (audioSource)
        {
            this.playStart = audioSource.time;
        }
    }

    public void RecordingEnd()
    {

        if (audioSource)
            this.playEnd = audioSource.time;
    }

    public void Update()
    {
        if (audioSource && playStart > 0 && playEnd > 0)
        {
            if (audioSource.time > playEnd)
            {
                audioSource.time = playStart;
            }
        }
    }
}