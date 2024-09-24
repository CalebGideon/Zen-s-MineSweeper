using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject prefab = Resources.Load<GameObject>("Explosion/Explosion");
        GameObject particalSystem = Instantiate(prefab, transform.position, Quaternion.identity);
        particalSystem.transform.position = new Vector3(particalSystem.transform.position.x, particalSystem.transform.position.y, 0f);
    }
}
