using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private string map_diff;
    private int bomb_count, max_tiles;
    private float bomb_freq = 0.17f;

    //Spawn parameters

    public float x_pos, y_pos;
    public int tile_id;

    private bool coin_flip;
    
    void Start() {
        map_diff = PlayerPrefs.GetString("Map_Type", "None");
        coin_flip = false;
        Set_Map_Size();
        Set_Bomb_Count();

        GameManager.Instance.Set_Max_To_Clear(bomb_count);
        Create_Map();
        GameManager.Instance.Set_Resolution();
    }

    private void Set_Map_Size() {

        if(map_diff == "Easy") {
            GameManager.Instance.map_width = 8;
            GameManager.Instance.map_height = 8;
        }
        if(map_diff == "Medium") {
            GameManager.Instance.map_width = 18;
            GameManager.Instance.map_height = 18;
        }
        if(map_diff == "Hard") {
            GameManager.Instance.map_width = 24;
            GameManager.Instance.map_height = 24;
        }
        
        if(map_diff == "None") {

            Debug.LogError("PlayerPrefs was not set correctly!");
        }
    }

    private void Set_Bomb_Count() {
        bomb_count = (int)((GameManager.Instance.map_width * GameManager.Instance.map_height) * bomb_freq);
        GameManager.Instance.Set_Flag_UI(bomb_count);
    }

    private void Create_Map() {
        for(int i=0; i<bomb_count; i++) {
            int temp_int;
            do 
            {
                temp_int = Random.Range(0, GameManager.Instance.map_width * GameManager.Instance.map_height);
            } while(GameManager.Instance.bomb_IDs.Contains(temp_int));

           GameManager.Instance.bomb_IDs.Add(temp_int);
        }

        for(int i=0; i<(GameManager.Instance.map_width*GameManager.Instance.map_height); i++) {
            //MAGIC NUMBERS!!!
            GameObject tile = Instantiate(Resources.Load<GameObject>("Tile"), new Vector3(x_pos,y_pos), Quaternion.identity, gameObject.transform);

            //How to decide colour? COIN FLIP!

            Color32 new_colour = (coin_flip)? new Color32(22, 255, 0, 255) : new Color32(22, 179, 0, 255);
            coin_flip = !coin_flip;

            tile.GetComponent<SpriteRenderer>().color = new_colour;
            tile.GetComponent<Tile>().Set_Tile_ID(tile_id);
            tile.GetComponent<Tile>().Set_Default_Colour(new_colour);

            //Check if its bomb?
            if(GameManager.Instance.bomb_IDs.Contains(tile_id)) {
                tile.GetComponent<Tile>().Set_Is_Bomb(true);
            }

            GameManager.Instance.tiles.Add(tile);
            //Move spawn location
            tile_id++;
            x_pos += 0.75f;
            if(tile_id != 0 && tile_id % GameManager.Instance.map_width == 0) {
                x_pos = 0;
                y_pos += 0.75f;

                //Checker pattern!
                coin_flip = !coin_flip;
            }
        }
    }

}
