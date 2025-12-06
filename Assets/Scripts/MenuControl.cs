using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public GameObject TutorialPanel;  // The panel that contains both ComboPage and IconPage
    public GameObject ComboPage;      // The Combo page inside TutorialPanel
    public GameObject IconPage;       // The Icon page inside TutorialPanel

    private bool isComboPageActive = true;  // Tracks which page is currently active

    void Start()
    {
        closePanel();
    }
    public void closePanel()
    {
        TutorialPanel.SetActive(false);
        ComboPage.SetActive(false);
        IconPage.SetActive(false);
    }
    // This method will be called when the TutorialPanel is activated
    public void ActivateTutorial()
    {
        TutorialPanel.SetActive(true);  // Activate the tutorial panel
        ShowComboPage();  // Show the ComboPage by default
    }

    // Switch between ComboPage and IconPage
    public void SwitchPages()
    {
        if (isComboPageActive)
        {
            ShowIconPage();  // Switch to IconPage
        }
        else
        {
            ShowComboPage();  // Switch to ComboPage
        }
    }

    // Show the ComboPage and hide the IconPage
    private void ShowComboPage()
    {
        ComboPage.SetActive(true);
        IconPage.SetActive(false);
        isComboPageActive = true;  // Update the flag
    }

    // Show the IconPage and hide the ComboPage
    private void ShowIconPage()
    {
        ComboPage.SetActive(false);
        IconPage.SetActive(true);
        isComboPageActive = false;  // Update the flag
    }
}
