using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItems : MonoBehaviour
{
   public GameObject cubePrefab;
   float spacing = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 4; x++) {
    for (int y = 0; y < 4; y++) {
        for (int z = 0; z < 4; z++) {
            Vector3 position = new Vector3(x, y, z) * spacing;

            Instantiate(cubePrefab, position, cubePrefab.transform.rotation);
        }
    }
}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
