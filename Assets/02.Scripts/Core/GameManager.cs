using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleTon<GameManager>
{
    public bool isFullRoom;

    public void SetGameTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
