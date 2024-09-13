using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    // Game Settings
    private CanvasGroup sceneFade;
    private float musicVolume, sfxVolume;
    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; SaveData(); } }
    public float SfxVolume { get { return sfxVolume; } set { sfxVolume = value; SaveData(); } }
    private TextMeshProUGUI deathText;

    // Player Save Data
    private int fewestDeaths = 999, deaths, level;
    public int FewestDeaths { get { return fewestDeaths; } set { fewestDeaths = value; } }
    public int Deaths { get { return deaths; } set { deaths = value; } }
    public int Level { get { return level; } set { level = value; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Reset();
        LoadData();
        UpdateDeaths();
        StartCoroutine(SceneFadeOut());
    }

    private void Reset()
    {
        deaths = 0;
        level = 1;
    }

    public void NewGame()
    {
        Reset();
        SaveData();
        LoadLevel(level);
    }

    private void LoadData()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);

        SaveData save = SaveSystem.LoadPlayer();
        if (save == null) return;
        fewestDeaths = save.fewestDeaths;
        deaths = save.deaths;
        level = save.level;
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SfxVolume", sfxVolume);
        PlayerPrefs.Save();
        
        SaveSystem.SavePlayer(this);
    }

    private IEnumerator SceneFadeIn()
    {
        sceneFade = FindObjectOfType<CanvasGroup>();
        sceneFade.gameObject.SetActive(true);
        sceneFade.alpha = 0f;
        while (sceneFade.alpha < 1f)
        {
            if (Time.deltaTime * 2f <= 1f - sceneFade.alpha)
                sceneFade.alpha += Time.deltaTime * 2f;
            else
                sceneFade.alpha = 1f;
            yield return null;
        }
    }

    private IEnumerator SceneFadeOut()
    {
        sceneFade = FindObjectOfType<CanvasGroup>();
        sceneFade.gameObject.SetActive(true);
        sceneFade.alpha = 1f;
        while (sceneFade.alpha > 0f)
        {
            if (Time.deltaTime * 2f <= sceneFade.alpha)
                sceneFade.alpha -= Time.deltaTime * 2f;
            else
                sceneFade.alpha = 0f;
            yield return null;
        }
    }

    public void IncrementDeaths()
    {
        deaths++;
        UpdateDeaths();
    }

    public void UpdateDeaths()
    {
        deathText = GameObject.Find("Deaths").GetComponent<TextMeshProUGUI>();
        deathText.text = SceneManager.GetActiveScene().buildIndex == 0 ? "Best: " + fewestDeaths : "Deaths: " + deaths;
    }

    public void LevelComplete()
    {
        if (level != 20)
        {
            level++;
            LoadLevel(level);
        }
        else
        {
            if (deaths < fewestDeaths) fewestDeaths = deaths;
            Reset();
            LoadLevel(0);
        }
    }

    public void LoadLevel(int index)
    {
        SaveData();
        StartCoroutine(AsyncLoadLevel(index));
    }

    private IEnumerator AsyncLoadLevel(int index)
    {
        StartCoroutine(SceneFadeIn());
        yield return new WaitForSeconds(0.5f);
        var asyncLoadLevel = SceneManager.LoadSceneAsync(index);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        UpdateDeaths();
        StartCoroutine(SceneFadeOut());
    }

    public IEnumerator Quit()
    {
        Debug.Log("Quitting...");
        StartCoroutine(SceneFadeIn());
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
