using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI alive;
    public TextMeshProUGUI killGUI;
    public int kill = 0;
    private AudioManager audioManager;
    public GameObject loseUI;
    public GameObject winUI;
    public AudioSource[] audioSources;
    public GameObject bg;

    TweenUI tweenUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        tweenUI = GetComponent<TweenUI>();

    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
        audioManager.PlayMusic("Theme");
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        GameObject[] _alive = GameObject.FindGameObjectsWithTag("Enemy");
        alive.SetText("Alive: " + _alive.Length.ToString());
        kill = 11 - _alive.Length;
        killGUI.SetText("Kill: " + kill.ToString());
        if (_alive.Length == 0)
        {
            Win();
        }
        Debug.Log(Time.timeScale);
    }

    public void Lose()
    {
        bg.SetActive(true);
        //loseUI.transform.LeanScale(Vector3.one, 2f);
        tweenUI.Tween(loseUI, 2f);
    }

    public void Win()
    {
        bg.SetActive(true);
        tweenUI.Tween(winUI, 2f);
    }

    public void ReloadScene()
    {
        int cur = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(cur);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }


    public void SoundOff()
    {
        foreach (AudioSource source in audioSources)
        {
            source.volume = 0f;
        }
    }

    public void SoundOn()
    {
        foreach (AudioSource source in audioSources)
        {
            source.volume = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SoundChange(bool isOn)
    {
        if (isOn)
        {
            SoundOn();
        }
        else
        {
            SoundOff();
        }
    }
}

