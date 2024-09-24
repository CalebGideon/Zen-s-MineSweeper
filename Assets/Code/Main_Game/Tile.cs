using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private int tile_id;
    [SerializeField]
    private bool is_bomb;
    [SerializeField]
    private bool is_cleared;
    [SerializeField]
    private bool is_flagged;

    private Color32 default_colour;

    public GameObject flag;
    public GameObject bomb;
    private AudioSource audio;
    private ParticleSystem particle;

    void Start() {
        audio = GetComponent<AudioSource>();
        particle = GetComponent<ParticleSystem>();
    }

    public int Get_Tile_ID() {
        return tile_id;
    }

    public bool Get_Is_Bomb() {
        return is_bomb;
    }

    public bool Get_Is_Cleared() {
        return is_cleared;
    }

    public bool Get_Is_Flagged() {
        return is_flagged;
    }

    public void Set_Tile_ID(int tile_id) {
        this.tile_id = tile_id;
    }

    public void Set_Is_Bomb(bool is_bomb) {
        this.is_bomb = is_bomb;
    }

    public void Set_Is_Cleared(bool is_cleared) {
        this.is_cleared = is_cleared;
    }

    public void Set_Is_Flagged(bool is_flagged) {
        this.is_flagged = is_flagged;
        Toggle_Flag();
    }

    public void Screw_Flag(bool is_flagged) {
        flag.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Set_Default_Colour(Color32 default_colour) {
        this.default_colour = default_colour;
    }

    private void Toggle_Flag() {    
        flag.GetComponent<SpriteRenderer>().enabled = is_flagged;
        if(is_flagged == true) {
            GameManager.Instance.Set_Flag_UI(-1);
        }
        else {
            GameManager.Instance.Set_Flag_UI(1);
        }
    }

    private void OnMouseEnter()
    {
        if(is_cleared != true) {
            Debug.Log("Mouse entered object");
            GetComponent<SpriteRenderer>().color = new Color32(177,255,170,255);
        }
    }

    //FLAG
    private void OnMouseOver() {
        if(Input.GetMouseButtonDown(1) && GameManager.Instance.CLICK_DISABLED == false) {
            if((is_cleared == false && GameManager.Instance.flag_count > 0) || is_flagged == true) {
                Debug.Log("Place flag now!");
                is_flagged = !is_flagged;
                Toggle_Flag();
            }
        }
    }

    private void OnMouseExit()
    {
        if(is_cleared != true) {
            GetComponent<SpriteRenderer>().color = default_colour;
        }
        Debug.Log("Mouse exited object");
    }

    private void OnMouseDown()
    {
        if(is_cleared != true && is_flagged != true && GameManager.Instance.CLICK_DISABLED == false) {

            //HERE WE GO!!!!
            if(is_bomb == true && GameManager.Instance.Get_First_Click() == false) {
                GameManager.Instance.Check_Game_Over("Bomb");
                return;
            }

            //If not re-directed, than here
            Recursive_Check();
            Debug.Log("Mouse button pressed on object");
        }

    }

    //BIG BOY
    private void Recursive_Check() {

        //For Big_Reveal
        int tiles_revealed = 0;

        Debug.Log(GameManager.Instance.Get_First_Click());
        int freebee = (GameManager.Instance.Get_First_Click())? 3 : 0;

        if(is_bomb == true && GameManager.Instance.Get_First_Click() == true) {
            Randomize_Bomb();
        }

        if(GameManager.Instance.Get_First_Click() == true) {
            GameManager.Instance.Set_First_Click(false);
        }
        Stack<GameObject> tiles_to_check = new Stack<GameObject>();
        tiles_to_check.Push(gameObject);

        while(tiles_to_check.Count != 0) {
            int bomb_count = 0;

            GameObject new_check = tiles_to_check.Pop();
            int tile_id = new_check.GetComponent<Tile>().Get_Tile_ID();

            List<GameObject> neighbours = Get_Neighbours(tile_id);

            foreach(GameObject neighbour in neighbours) {
                if(neighbour.GetComponent<Tile>().Get_Is_Bomb() == true) {
                    bomb_count += 1;
                }
            }

            if(new_check.GetComponent<Tile>().Get_Is_Flagged() == true) {
                new_check.GetComponent<Tile>().Set_Is_Flagged(false);
            }

            //BASED ON RECURSIVE END to determine whether it has number or is blank, and bomb count to determine number, decide the current gameobjects sprite
            new_check.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.bomb_status[bomb_count];
            new_check.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);;

            //Audio!!! ANIMATION!
            audio.Play();
            particle.Play();

            if(bomb_count == 0 || freebee > 0) {
                foreach(GameObject neighbour in neighbours) {
                    if(neighbour.GetComponent<Tile>().Get_Is_Cleared() == false && neighbour.GetComponent<Tile>().Get_Is_Bomb() == false) {
                        tiles_to_check.Push(neighbour);
                    }
                }
            }

            freebee -= 1;
            tiles_revealed += 1;
            if(new_check.GetComponent<Tile>().Get_Is_Cleared() == false) {
                GameManager.Instance.Check_Game_Over("Cleared");
            }
            new_check.GetComponent<Tile>().Set_Is_Cleared(true);
        }
        GameManager.Instance.Set_Revealed_Count(tiles_revealed);
    }


    private void Randomize_Bomb() {
        //Make this bomb no longer a bomb

        is_bomb = false;
        List<GameObject> neighbours = Get_Neighbours(tile_id);

        foreach(GameObject neighbour in neighbours) {

            Tile script = neighbour.GetComponent<Tile>();

            if(script.Get_Is_Bomb() == false) {
                script.Set_Is_Bomb(true);
                break;
            }

            //IF FOR SOME REASON THERE IS A BOMB SURRONDING THE TILE? WHAT HAPPENS THEN? I GUESS JUST ONE LESS BOMB THAN USUAL? BUT THAT WOULD BE INSANE BAD LUCK
        }
    }

    private List<GameObject> Get_Neighbours(int id) {

        List<GameObject> neighbours = new List<GameObject>();

        int map_size = (GameManager.Instance.map_width * GameManager.Instance.map_height);
        int map_width = GameManager.Instance.map_width;
        int map_height = GameManager.Instance.map_height;

        //furthest left
        if(id % map_width != 0) {
            GameObject l_tile = GameManager.Instance.tiles[id-1];
            bool is_cleared = l_tile.GetComponent<Tile>().Get_Is_Cleared();
            if(is_cleared == false) {
                neighbours.Add(l_tile);
            }
        }

        if(id % map_width != map_width-1) {
            GameObject r_tile = GameManager.Instance.tiles[id+1];
            bool is_cleared = r_tile.GetComponent<Tile>().Get_Is_Cleared();
            if(is_cleared == false) {
                neighbours.Add(r_tile);
            }
        }

        if(id - map_height >= 0) {
            GameObject b_tile = GameManager.Instance.tiles[id-map_height];
            bool is_cleared = b_tile.GetComponent<Tile>().Get_Is_Cleared();
            if(is_cleared == false) {
                neighbours.Add(b_tile);
            }
        }

        if(id + map_height < map_size) {
            GameObject t_tile = GameManager.Instance.tiles[id+map_height];
            bool is_cleared = t_tile.GetComponent<Tile>().Get_Is_Cleared();
            if(is_cleared == false) {
                neighbours.Add(t_tile);
            }
        }

        if(id % map_width != 0 && id - map_height >= 0) {
            GameObject bl_tile = GameManager.Instance.tiles[(id-1)-map_height];
            bool is_cleared = bl_tile.GetComponent<Tile>().Get_Is_Cleared();
            if(is_cleared == false) {
                neighbours.Add(bl_tile);
            }
        }

        if(id % map_width != map_width-1 && id - map_height >= 0) {
            GameObject br_tile = GameManager.Instance.tiles[(id+1)-map_height];
            bool is_cleared = br_tile.GetComponent<Tile>().Get_Is_Cleared();
            if(is_cleared == false) {
                neighbours.Add(br_tile);
            }
        }

        if(id % map_width != 0 && id + map_height < map_size) {
            GameObject tl_tile = GameManager.Instance.tiles[(id-1)+map_height];
            bool is_cleared = tl_tile.GetComponent<Tile>().Get_Is_Cleared();
            if(is_cleared == false) {
                neighbours.Add(tl_tile);
            }
        }

        if(id % map_width != map_width-1 && id + map_height < map_size) {
            GameObject tr_tile = GameManager.Instance.tiles[(id+1)+map_height];
            bool is_cleared = tr_tile.GetComponent<Tile>().Get_Is_Cleared();
            if(is_cleared == false) {
                neighbours.Add(tr_tile);
            }
        }

        return neighbours;
    }
}
