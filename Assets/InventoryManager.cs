using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool MenuActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && MenuActivated)
        {
            InventoryMenu.SetActive(false);
            MenuActivated = false;
        }
        else if (Input.GetButtonDown("Inventory") && !MenuActivated)
        {
            InventoryMenu.SetActive(true);
            MenuActivated = true;
        }
    }
}
