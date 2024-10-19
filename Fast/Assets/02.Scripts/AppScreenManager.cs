using System.Collections.Generic;
using UnityEngine;

public class AppScreenManager : MonoBehaviour
{
    public static AppScreenManager Instance { get; private set; }

    [SerializeField] private List<GameObject> _screens;
    [SerializeField] private GameObject _firstActiveScene;

    private GameObject _currentActiveScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        foreach (var screeen in _screens)
        {
            screeen.SetActive(false);
        }
        _currentActiveScene = _firstActiveScene;
        _currentActiveScene.SetActive(true);
    }


    public void ChangeToScreen(GameObject screen)
    {
        var screenGO = _screens.Find((scr) => scr == screen);

        if (screenGO == null) return;

        _currentActiveScene.SetActive(false);
        _currentActiveScene = screenGO;
        _currentActiveScene.SetActive(true);
    }

    public void ChangeToScreen(string name)
    {
        var screen = _screens.Find((s) => s.name == name);
        ChangeToScreen(screen);
    }
}
