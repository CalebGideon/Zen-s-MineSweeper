using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance {get; private set;}

    void Awake () {
    // force singleton instance
        if (Instance == null) { Instance = this; }
        else { Destroy (gameObject); }
    }

    public GameObject game_over_ui;

    public List<GameObject> tiles = new List<GameObject>();
    public HashSet<int> bomb_IDs = new HashSet<int>();

    public bool CLICK_DISABLED;
    public List<Sprite> bomb_status;

    public int map_width, map_height;

    public TextMeshProUGUI flag_count_ui;
    public int flag_count;

    //Major settings
    private string map_difficulty; 
    private int cleared_tiles, max_to_clear;
    private bool first_click;
    private int revealed_Count;

    void Start() {
        first_click = true;
        revealed_Count = 0;
    }

    public void Set_Flag_UI(int new_val) {
        flag_count += new_val;
        flag_count_ui.text = flag_count.ToString();
    }

    public void Set_First_Click(bool first_click) {
        this.first_click = first_click;
    }

    public void Set_Max_To_Clear(int bomb_count) {
        max_to_clear = ((map_width * map_height) - bomb_count);
    }

    public void Set_Revealed_Count(int revealed_Count) {
        this.revealed_Count = revealed_Count;
    }

    public bool Get_First_Click() {
        return first_click;
    }

    public int Get_Revealed_Count() {
        return revealed_Count;
    }

    public void Check_Game_Over(string status) {

        if(status == "Bomb") {
            Debug.Log("YOU HAVE LOST THE GAME!!!");
            StartCoroutine(Mass_Explode());
            //(3) Popup for lose back to menu
            
        }
        if(status == "Cleared") {
            Debug.Log(cleared_tiles + " " + max_to_clear);
            cleared_tiles += 1;
            if(cleared_tiles == max_to_clear) {
                CLICK_DISABLED = true;
                Game_Over_Popup(1);
                Debug.Log("YOU'VE WON THE GAME!!!");
            }
        }
        
    }

    IEnumerator Mass_Explode() {
        CLICK_DISABLED = true;
        foreach(GameObject tile in tiles) {
            Tile script = tile.GetComponent<Tile>();
            script.Screw_Flag(false);
            if(script.Get_Is_Bomb() == true) {
                script.bomb.SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
        }
        Game_Over_Popup(0);
    }

    public void Set_Resolution() {
        GetComponent<Set_Resolution>().Set_Res();
    }

    private void Game_Over_Popup(int status) {

        game_over_ui.SetActive(true);
        TextMeshProUGUI text = GameObject.Find("Over_Text").GetComponent<TextMeshProUGUI>();

        if(status == 0) {
            text.text = "You Died!";
        }
        else if(status == 1) {
            text.text = "You Win!";
        }
        
        else {
            //ERROR
        }
    }

}
