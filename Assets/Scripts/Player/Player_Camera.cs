using UnityEngine;
using System.Collections;

public class Player_Camera : MonoBehaviour 
{
    Transform player;
    public int offset = 15, altura = 6;

    bool shaking = false;
    float duration_shake = 0.5f;
    float speed_shake = 1;
    float magnitude_shake = 0.5f;

	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(player.position.x, altura, -offset);
        transform.LookAt(player);
	}
    
    void LateUpdate()
    {
        if (!shaking)
        {
            if (player.GetComponent<Player>().GetState() == States.State_Speak)
            {
                if (Vector3.Distance(transform.position, player.position) > 8)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.position, 5 * Time.deltaTime);
                }
            }
            else
            {
                float speed = Mathf.Max(4, Mathf.Abs(player.GetComponent<Rigidbody>().velocity.x)) * 0.95f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.position + new Vector3(0, altura, -offset), speed);
            }

            transform.LookAt(player);
        }
    }

    public void PlayShake()
    {
        shaking = true;
        StartCoroutine("Shake");
    }

    IEnumerator Shake()
    {
        float elapsed = 0;

        Vector3 originalCamPos = transform.position;
        float randomStart = Random.Range(-1000.0f, 1000.0f);

        while (elapsed < duration_shake)
        {
            elapsed += Time.fixedDeltaTime;

            float percentComplete = elapsed / duration_shake;

            float damper = 1.0f - Mathf.Clamp(2 * percentComplete - 1, 0, 1);
            float alpha = randomStart + speed_shake * percentComplete;

            /*float x = Util.Noise.GetNoise();
            float y = Util.Noise.GetNoise();*/

            float x = SimplexNoise.Noise(alpha, 0, 0);
            float y = SimplexNoise.Noise(0, alpha, 0);

            x *= magnitude_shake * damper;
            y *= magnitude_shake * damper;

            transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

            yield return null;
        }

        shaking = false;
        transform.position = originalCamPos;
    }
}
