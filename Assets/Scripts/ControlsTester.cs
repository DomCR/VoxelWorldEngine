using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine;

public class ControlsTester : MonoBehaviour
{
    public WorldGenerator World;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ControlSectionLevel();
    }

    void ControlSectionLevel()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Add a level into the section level
            World.SectionLevel = World.SectionLevel + 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Substract a unit in the level
            World.SectionLevel = World.SectionLevel - 1;
        }
    }

    private void OnGUI()
    {
        
    }
}
