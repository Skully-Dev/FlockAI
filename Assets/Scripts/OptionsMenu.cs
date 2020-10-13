using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    private bool menuActive = false; //current menu status
    [SerializeField]
    [Tooltip("Reference parent of menu UI")]
    private GameObject menu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //if escape is pressed
        {
            if (menuActive) //if menu currently active
            {
                Resume(); //deactivate menu
            }
            else
            {
                Options(); //otherwise activate menu
            }
        }
    }

    public void Resume() //called on esc or Resume button on click event
    {
        menu.SetActive(false); //hides menu UI
        menuActive = false; //sets menu active state bool
    }

    public void Options()
    {
        menu.SetActive(true); //shows menu UI
        menuActive = true; //sets menu active state bool
    }

    public void Restart()//called with restart button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//reloads the scene
    }

    public void QuitSimulation() //called with quit button
    {
        Debug.Log("Quitting application.");

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode(); //If using unity, run this code, exit play mode
#endif
        Application.Quit(); //if not unity editor, quits app, i.e. once published, this will quit.
    }
}