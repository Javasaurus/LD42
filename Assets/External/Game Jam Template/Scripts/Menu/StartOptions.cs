using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

//MODIFIED BY JAVASAURUS

public class StartOptions : MonoBehaviour
{

    public MenuSettings menuSettingsData;
    public int sceneToStart;                                            //Index number in build settings of scene to load if changeScenes is true
    public bool changeMusicOnStart;										//Choose whether to continue playing menu music or start a new music clip
    public CanvasGroup fadeOutImageCanvasGroup;                         //Canvas group used to fade alpha of image which fades in before changing scenes
    public Image fadeImage;                                             //Reference to image used to fade out before changing scenes

    [HideInInspector] public bool inMainMenu = true;                    //If true, pause button disabled in main menu (Cancel in input manager, default escape key)
    [HideInInspector] public AnimationClip fadeAlphaAnimationClip;      //Animation clip fading out UI elements alpha
    [HideInInspector] public bool fadeCompleted = false;                //Boolean indicating if the fade has been completed (to assure we are not loading levels while still fading)

    private PlayMusic playMusic;                                        //Reference to PlayMusic script
    private float fastFadeIn = .01f;                                    //Very short fade time (10 milliseconds) to start playing music immediately without a click/glitch
    private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels
    private CanvasGroup menuCanvasGroup;


    void Awake()
    {
  
        //Get a reference to ShowPanels attached to UI object
        showPanels = GetComponent<ShowPanels>();

        //Get a reference to PlayMusic attached to UI object
        playMusic = GetComponent<PlayMusic>();

        //Get a reference to the CanvasGroup attached to the main menu so that we can fade it's alpha
        menuCanvasGroup = GetComponent<CanvasGroup>();

        fadeImage.color = menuSettingsData.sceneChangeFadeColor;
    }


    public void StartButtonClicked()
    {
        //If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic
        //To change fade time, change length of animation "FadeToColor"
        if (menuSettingsData.musicLoopToChangeTo != null)
        {
            playMusic.FadeDown(menuSettingsData.menuFadeTime);
        }

        //If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
        if (sceneToStart != 0)
        {
            StartCoroutine(FadeCanvasGroupAlpha(0f, 1f, fadeOutImageCanvasGroup));
            StartCoroutine(LoadNextScene(sceneToStart));
            //Pause button now works if escape is pressed since we are no longer in Main menu.
            inMainMenu = false;

            //Hide the main menu UI element
            showPanels.HideMenu();
        }


    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneTransition;
        SceneManager.sceneLoaded += SceneWasLoaded;
    }

    //Once the level has loaded, check if we want to call PlayLevelMusic
    void SceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        //if changeMusicOnStart is true, call the PlayLevelMusic function of playMusic
        if (menuSettingsData.musicLoopToChangeTo != null)
        {
            playMusic.PlayLevelMusic();
        }
    }

    //Fade in any new scene (transition screens)
    void SceneTransition(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeCanvasGroupAlpha(1f, 0f, fadeOutImageCanvasGroup));
    }


    public IEnumerator FadeCanvasGroupAlpha(float startAlpha, float endAlpha, CanvasGroup canvasGroupToFadeAlpha)
    {
        fadeCompleted = false;
        float elapsedTime = 0f;
        float totalDuration = menuSettingsData.menuFadeTime;
        canvasGroupToFadeAlpha.alpha = startAlpha;
        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);
            canvasGroupToFadeAlpha.alpha = currentAlpha;
            yield return null;
        }
        fadeCompleted = true;
    }

    public IEnumerator LoadNextScene(int sceneIndex)
    {
        //Load the selected scene, by scene index number in build settings
        while (!fadeCompleted)
        {
            yield return null;
        }
        Debug.Log("New scene being loaded. Scene index: " + sceneIndex);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    public void PlayNewMusic()
    {
        //Fade up music nearly instantly without a click 
        playMusic.FadeUp(fastFadeIn);
        //Play second music clip from MenuSettings
        playMusic.PlaySelectedMusic(menuSettingsData.musicLoopToChangeTo);
    }
}
