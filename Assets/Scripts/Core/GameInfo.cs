using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[System.Serializable]
public class GameInfo
{
    public static GameInfo current;
    public Vector3Serialized player_post;
    public Inventory player_inventory;

    public GameInfo()
    {
        
    }

    public void LoadCheckPoint()
    {
        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        p.ChangeState<State_Idle>();
        p.GetRigidbody().velocity = Vector3.zero;
        p.GetRigidbody().MovePosition(new Vector3(current.player_post.x, current.player_post.y, current.player_post.z));
    }

    public void LoadGame()
    {
        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        p.ChangeState<State_Idle>();
        p.GetRigidbody().velocity = Vector3.zero;
        p.GetRigidbody().MovePosition(new Vector3(current.player_post.x, current.player_post.y, current.player_post.z));

        p.SetInventory(player_inventory);
    }
}

[System.Serializable]
public struct Vector3Serialized 
{
    public float x, y, z;

    public Vector3Serialized(float x1, float y1, float z1) { x = x1; y = y1; z = z1; }
}