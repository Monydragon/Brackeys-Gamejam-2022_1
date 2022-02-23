using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class CutScene : MonoBehaviour
{
    public Flowchart flowchart;
    public BlockReference startingBlock;
    public Image cutsceneImage;
    public bool useCutsceneImage = true;

    // Start is called before the first frame update
    void Start()
    {
        flowchart.ExecuteBlock(startingBlock.block);
    }

    // Update is called once per frame
    void Update()
    {
        if (useCutsceneImage)
        {
            cutsceneImage.gameObject.SetActive(flowchart.HasExecutingBlocks());
        }
    }
}
