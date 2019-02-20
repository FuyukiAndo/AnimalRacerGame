using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {

    public void SetVolume(float volume)
    {
        Debug.Log("Denna slider gör ingenting eftersom FMOD behöver implementeras");
    }

    public void SetQualityLevel(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
