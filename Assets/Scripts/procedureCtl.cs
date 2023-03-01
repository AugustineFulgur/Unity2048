using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class procedureCtl : MonoBehaviour
{
    public GameObject board;
    // Start is called before the first frame update
    void Start()
    {
        board.GetComponent<boardObject>().createNew();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
