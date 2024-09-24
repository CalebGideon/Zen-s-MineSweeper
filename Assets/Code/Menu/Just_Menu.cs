using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Just_Menu : MonoBehaviour
{
    public void Menu() {

        if(GameManager.Instance.CLICK_DISABLED == false) {
            //Update backendless or new API that I need to learn? ScoreBoard?
            SceneManager.LoadScene("Main_Menu");
        }
    }
}
