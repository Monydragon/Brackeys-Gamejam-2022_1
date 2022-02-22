using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 10;
    public float speed;
    public float attackSpeed = 0.5f;
    public int damage;
    public InventoryObject inventory;

    private Vector2 movement;
    private Vector2 lastMove;
    [SerializeField]
    private Animator playerAnim;
    private void OnEnable()
    {
        EventManager.onItemUse += EventManager_onItemUse;
    }


    private void EventManager_onItemUse(ItemObject _item, GameObject _obj)
    {
        if(_obj == this.gameObject)
        {
            Debug.Log($"Player Used Item: {_item.name}");
            inventory.RemoveItem(_item);
        }
    }

    private void OnDisable()
    {
        EventManager.onItemUse -= EventManager_onItemUse;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        playerAnim.SetFloat("MoveX", movement.x);
        playerAnim.SetFloat("MoveY", movement.y);
        if (movement != Vector2.zero)
        {
            playerAnim.SetBool("isMoving", true);
            lastMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            playerAnim.SetFloat("LastMoveX", lastMove.x);
            playerAnim.SetFloat("LastMoveY", lastMove.y);
        }
        else
        {
            playerAnim.SetBool("isMoving", false);
        }

        transform.position += (Vector3)movement.normalized * Time.deltaTime * speed;

        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.Load();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            inventory.Save();
        }
    }

    private void OnApplicationQuit()
    {
        inventory.container.Clear();
        inventory.currentSize = 0;
    }
}
