using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Main_Menu : MonoBehaviour
{

    private List<GameObject> menu_buttons;
    

    void Start() {
        PlayerPrefs.DeleteAll();
        menu_buttons = new List<GameObject>();

        //Assign buttons at runtime? With error checks if needed?
    }

    public void On_Pressed_Easy() {
        PlayerPrefs.SetString("Map_Type", "Easy");
        Load_Game();
    }

    public void On_Pressed_Medium() {
        PlayerPrefs.SetString("Map_Type", "Medium");
        Load_Game();
    }

    public void On_Pressed_Hard() {
        PlayerPrefs.SetString("Map_Type", "Hard");
        Load_Game();
    }

    public void Quit_Application() {
        Application.Quit();
    }

    private void Load_Game() {
        //Load Level
        SceneManager.LoadScene("Main_Game");
        
    }
}
