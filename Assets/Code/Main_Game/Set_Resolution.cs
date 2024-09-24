using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Resolution : MonoBehaviour
{
    public void Set_Res() {

        //Centre camera
        Vector3 min_vec = GameManager.Instance.tiles[0].transform.position;
        Vector3 max_vec = GameManager.Instance.tiles[GameManager.Instance.tiles.Count-1].transform.position;

        Debug.Log(min_vec);
        Debug.Log(max_vec);

        float centerX = (min_vec.x + max_vec.x) / 2f;
        float centerY = ((min_vec.y + max_vec.y) / 2f);

        GameObject.Find("Main Camera").GetComponent<Camera>().transform.position = new Vector3(centerX, centerY, -10f);

        //Scale camera

        float camera_scale = (max_vec.y - min_vec.y);
        GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = camera_scale / 1.5f;
    }
}
