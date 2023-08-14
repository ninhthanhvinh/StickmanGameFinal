using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;
    [SerializeField] private GameObject _loaderUI;
    [SerializeField] private Image _progressBar;
    // Start is called before the first frame update
    void Start()
    {
        LoadScene("PlayStage");
    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderUI.SetActive(true);

        do
        {
            await Task.Delay(100);
            _progressBar.fillAmount = scene.progress;
        }
        while (scene.progress < 0.9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
    }
}
