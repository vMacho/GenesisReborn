using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{
    bool used;

    void OnTriggerEnter(Collider other)
    {
        Player c = other.GetComponent<Player>();
        if (c && !used)
        {
            GameInfo.current.player_post = new Vector3Serialized(c.gameObject.transform.position.x, c.gameObject.transform.position.y, c.gameObject.transform.position.z);
            GameInfo.current.player_inventory = new Inventory(c.GetInvetory().GetBag(), c.GetInvetory().OpenBag());

            used = true;
            SaveLoad.SaveCheckPoint();
        }
    }
}
