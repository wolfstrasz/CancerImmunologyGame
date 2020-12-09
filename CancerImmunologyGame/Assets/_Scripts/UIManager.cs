using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private PlayerController pc; // SHOULD BE REMOVED FROM HERE

    [SerializeField]
    private GameObject playerObj;
    [SerializeField]
    private GameObject dendriticCell;
    [SerializeField]
    private GameObject popup;
    [SerializeField]
    private GameObject trailOfChemokines;

    [SerializeField]
    private List<TutorialObject> tutorialObjects = new List<TutorialObject>();
    private int tutIndex = 0;

    [SerializeField]
    private GameObject TutorialPanel;
    [SerializeField]
    private TMP_Text TutorialTxt;
    [SerializeField]
    private GameObject SkipTxt;

    private bool isDisplayingTutorialText = false;

    [SerializeField]
    public List<CancerCell> allCancerCells = new List<CancerCell>();
    [SerializeField]
    public GameObject WinScreen = null;


    public Image ImmunotherapyIcon = null;
    public Color ImmunotherapyCanActivateColour = Color.cyan;
    public Color ImmunotherapyCannotActivateColour = Color.white;
    public Button ImmunotherapyButton = null;


    public GameObject MainMenuPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && tutIndex == tutorialObjects.Count)
        {
            RemoveTutorialText();
        }
        if (isPaused) return;


        if (first_exhausting_mid_range && !first_therapy)
        {
            StartTherapyTutorial();
        }
        else if ((first_exhausting_mid_range && first_therapy) && !second_therapy)
        {
            StartTherapyTutorial2();
        }

        if (isDisplayingTutorialText && TutorialPanel.activeSelf )
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("PRESSING SPACE!");
                isDisplayingTutorialText = false;
                isPaused = false;
                NextTutorial();
            }
        }

        if (allCancerCells.Count <= 0)
        {
            WinScreen.SetActive(true);
            isPaused = true;
        }
    }

    void DisplayTutorialText ( string txt)
    {
        pc.areControlsEnabled = false;
        TutorialTxt.text = txt;
        TutorialPanel.SetActive(true);
        // Enalbe tut control
    }

    public void RemoveTutorialText()
    {
        TutorialPanel.SetActive(false);
        SkipTxt.SetActive(false);
        // Enable player controls
        pc.areControlsEnabled = true;
        UIManager.Instance.isPaused = false;

    }

    public void JustRemoveTutorialText()
    {
        TutorialPanel.SetActive(false);
        SkipTxt.SetActive(false);
    }


    // TUTORIAL STUFF NEEDS OWN CLASS
    public void NextTutorial()
    {
        if (isPaused) return;

        if (tutIndex < tutorialObjects.Count)
        {
            Debug.Log("START TUTORIAL OBJ: " + tutIndex);

            var tutObj = tutorialObjects[tutIndex];

            if (tutObj.shouldDisplayText)
            {
                DisplayTutorialText(tutObj.txt);
            }

            if (tutObj.skipButton)
            {
                isDisplayingTutorialText = true;
                SkipTxt.SetActive(true);
            }

            tutorialObjects[tutIndex].Activate();
            tutIndex++;
            if (tutIndex == tutorialObjects.Count)
                ActivatePlayerPanel();

        }
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NextTutorial();
    }

    public void Cinema(float time)
    {
        StartCoroutine(Wait(time));
    }

    public void FocusOnDendriticCell()
    {
        JustRemoveTutorialText();
        Debug.Log("FOCUS ON DENDRITIC CELL");
        SmoothCamera.Instance.currentTarget = dendriticCell;
        SmoothCamera.Instance.isCameraFocused = false;
        SmoothCamera.Instance.nextTut = false;
    }

    public void FocusOnPlayer()
    {
        Debug.Log("FOCUS ON Player CELL");
        SmoothCamera.Instance.currentTarget = playerObj;
        SmoothCamera.Instance.isCameraFocused = false;
        SmoothCamera.Instance.nextTut = false;
    }

    public void SpawnTutorialPopUpAtDendriticCell()
    {
        RemoveTutorialText();
        Instantiate(popup, dendriticCell.transform.position, Quaternion.identity);
    }

    public void SpawnTrailOfChemokines()
    {
        //RemoveTutorialText();
        trailOfChemokines.SetActive(true);
    }


    public void StartImmunotherapy()
    {
        Debug.Log("Button pressed");
        bool success = GlobalGameData.Instance.TriggerPowerUp();
        if (success)
        {
            UIManager.Instance.isPaused = false;
            pc.StartPowerUp();
        }
    }



    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool isPaused = true;

    ///////////////////////////////////// 
    /// NEW TUTORIAL INFO
    /// 

    bool first_health_taken = false;
    bool first_exhausting_mid_range = false;
    bool first_therapy = false;
    bool first_bump = false;
    bool second_therapy = false;

    public GameObject TherapyPanel = null;
    public GameObject PlayerPanel = null;
    public RegulatorySense rc = null;

    public void ActivatePlayerPanel()
    {
        PlayerPanel.SetActive(true);
    }

    public void ActivateTherapyPanel()
    {
        TherapyPanel.SetActive(true);
    }

    public void StartExhaustTutorial()
    {
        if (first_exhausting_mid_range) return;
        first_exhausting_mid_range = true;
        isPaused = true;

        DisplayTutorialText("Oh dear! Interacting with cancer cells makes immune cells like you become tired and exhausted" + 
            " (look at your brown increasing exhaustion bar)." 
            + " This will make you move slower, a danger in the hypoxic cancer enviroment.");
        isDisplayingTutorialText = true;
        SkipTxt.SetActive(true);
    }

    public void StartTherapyTutorial()
    {
        isPaused = true;
        first_therapy = true;

        DisplayTutorialText("However, do not fear! Cancer researchers have developed cancer treatments known as immunotherapies");
        isDisplayingTutorialText = true;
        SkipTxt.SetActive(true);
    }

    public void StartTherapyTutorial2()
    {
        isPaused = true;
        second_therapy = true;
        ActivateTherapyPanel();

        DisplayTutorialText("These are therapies which reverse immune cell exhaustion, " 
            + " giving immune cells like you the extra boost they need to get rid of cancer. Give it a try and see! (press the circle button)");
        isDisplayingTutorialText = true;
        SkipTxt.SetActive(true);
    }

    public void StartHealthTutorial()
    {
        if (first_health_taken) return;
        first_health_taken = true;
        isPaused = true;

        DisplayTutorialText("As cancers grow they use all the oxygen and blood supply in their surrounding, generating an enviroment that is hypoxic," 
            + "meaning without oxygen. This damages immune cells like you, so beware! Don't let your health bar (red) drop to zero");

        isDisplayingTutorialText = true;
        SkipTxt.SetActive(true);
    }

    public void StartRegulatoryCellTutorial()
    {
        if (first_bump) return;
        first_bump = true;
        isPaused = true;
        StartCoroutine(ExecuteRCTutorial());
    }

    IEnumerator ExecuteRCTutorial()
    {
        yield return new WaitForSeconds(0.4f);
        rc.InitialiseRC();

        yield return new WaitForSeconds(0.4f);

        DisplayTutorialText("Cancer cells can disguise as healthy cells to the regulatory cells." +
            " This though guy is a regulatory cell and it prevents TK cells, like you, from damaging healthy cells");

        isDisplayingTutorialText = true;
        SkipTxt.SetActive(true);
    }

}
