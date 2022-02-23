using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class NpcDialogue : MonoBehaviour
{
    public Flowchart flowchart;
    public BlockReference startingBlock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            flowchart.ExecuteBlock(startingBlock.block);
        }
    }
}
