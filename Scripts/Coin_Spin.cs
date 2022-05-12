using UnityEngine;

public class Coin_Spin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Orient the Coin Object so that it is Vertical i.e. not parallel to the ground plane
        transform.eulerAngles = new Vector3(-90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 150f * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Jammo_Player")
        {
            // Player has made contact with Coin ~ destroy the coin & add 1 to the coin counter

            other.GetComponent<CoinCollector>().coinPoints++; // increment the Player coin points by 1

            Destroy(gameObject); // destroy the collected coin from the scene
        }

        // AI Agent collector update
        if (other.name == "AI_Agent")
        {
            // AI Agent has made contact with Coin ~ destroy the coin & add 1 to the coin counter

            other.GetComponent<AICoinCollector>().coinPoints++; // increment the AI Agent coin points by 1

            Destroy(gameObject); // destroy the collected coin from the scene
        }

    }
}