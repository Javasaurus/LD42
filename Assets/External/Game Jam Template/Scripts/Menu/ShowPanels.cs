using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ShowPanels : MonoBehaviour
{

    public UnityEngine.GameObject optionsPanel;                         //Store a reference to the Game Object OptionsPanel 
    public UnityEngine.GameObject optionsTint;                          //Store a reference to the Game Object OptionsTint 
    public UnityEngine.GameObject menuPanel;                            //Store a reference to the Game Object MenuPanel 
    public UnityEngine.GameObject leaderboardPanel;                     //Store a reference to the Game Object LeaderboardPanel 
    public UnityEngine.GameObject pausePanel;                           //Store a reference to the Game Object PausePanel 

    private UnityEngine.GameObject activePanel;
    private MenuObject activePanelMenuObject;
    private EventSystem eventSystem;



    private void SetSelection(UnityEngine.GameObject panelToSetSelected)
    {

        activePanel = panelToSetSelected;
        activePanelMenuObject = activePanel.GetComponent<MenuObject>();
        if (activePanelMenuObject != null)
        {
            activePanelMenuObject.SetFirstSelected();
        }
    }

    public void Start()
    {
        SetSelection(menuPanel);
    }

    public void ShowPanel(UnityEngine.GameObject panel)
    {
        panel.SetActive(true);
        optionsTint.SetActive(true);
        menuPanel.SetActive(false);
        SetSelection(panel);
    }

    public void HidePanel(UnityEngine.GameObject panel)
    {
        menuPanel.SetActive(true);
        panel.SetActive(false);
        optionsTint.SetActive(false);
    }

    //Call this function to activate and display the main menu panel during the main menu
    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        SetSelection(menuPanel);
    }

    //Call this function to deactivate and hide the main menu panel during the main menu
    public void HideMenu()
    {
        menuPanel.SetActive(false);
    }

    //Call this function to activate and display the Pause panel during game play
    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        optionsTint.SetActive(true);
        SetSelection(pausePanel);
    }

    //Call this function to deactivate and hide the Pause panel during game play
    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
        optionsTint.SetActive(false);

    }
}
