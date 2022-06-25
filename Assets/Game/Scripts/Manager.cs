using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public void NextLevel()
    {
        LevelManager.Instance.NextLevel();
        LevelManager.Instance.ui.UpdateLevelText();
    }

    public void RestartLevel()
    {
        LevelManager.Instance.RestartLevel();
    }
}
