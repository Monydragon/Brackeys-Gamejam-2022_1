using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class CutScene : MonoBehaviour
{
    public Flowchart flowchart;
    public BlockReference startingBlock;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.ShowCutscene(true);
        flowchart.ExecuteBlock(startingBlock.block);
    }
}
