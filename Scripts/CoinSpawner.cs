using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    public GameObject CoinInstance; 
    public int coinCountLimit = 100; 
    public int coinCount = 0; 
    int area_quadrant; // Integer demarcating the Area Quadrant for spawning Coin
    float xPos; // X-position of spawned coin
    float yPos; // Y-position of spawned coin
    float zPos; // Z-position of spawned coin
    
    // method to spawn coins randomly
    IEnumerator CoinSpawn()
    {
        while (coinCount <= coinCountLimit)
        {
            // continuously spawn coins randomly until reaches max number of coins
 
            area_quadrant = Random.Range(1,5);

            xPos = 0;
            yPos = 0;
            zPos = 0;

            if ( area_quadrant == 1)
            {
                // First Quadrant
                xPos = Random.Range(-11, 8);
                zPos = Random.Range(-3, 22);
                yPos = 0;

                if ( ((-4f < xPos) && (xPos < 5.7f)) && ((2.7f < zPos) && (zPos < 18f)) )
                {
                    yPos = Random.Range(-1, 0);
                }
                else
                {
                    yPos = Random.Range(-3, -1);;
                }

                float adjust = Random.Range(0, 2) - 0.5f;
                if (adjust < 0)
                {
                    adjust = 0;
                }
                yPos += adjust;
            }
            else if (area_quadrant == 2)
            {
                // Second Quadrant
                xPos = Random.Range(-11, 13);
                zPos = Random.Range(25, 42);
                yPos = -2.0f;

                if ( ((-5f < xPos) && (xPos < 7f)) && ((28f < zPos) && (zPos < 35)) )
                {
                    yPos = Random.Range(-4, -3);
                    yPos += 0.17f;
                }
                else
                {
                    yPos = Random.Range(-5, -2);
                    yPos += 0.2f;
                }
                yPos += 0.08f;
            }
            else if (area_quadrant == 3)
            {
                // Third Quadrant
                xPos = Random.Range(-31, -16);
                zPos = Random.Range(2, 16) - 0.6f;
                yPos = -1.0f;

                if (((-26 < xPos) && (xPos < -20)) && ((1.6f < zPos) && (zPos < 11.6)))
                {
                    yPos = Random.Range(-3, -2);
                    yPos += 0.3f;
                }
                else
                {
                    yPos = Random.Range(-4, -3);
                    yPos += 0.2f;
                }
            }
            else if (area_quadrant == 4)
            {
                // Fourth Quadrant
                xPos = Random.Range(0, 3); // Take two Possible regions of xPos
                zPos = Random.Range(23, 35);
                yPos = 4.0f;

                if (xPos > 1)
                {
                    xPos = Random.Range(-31, -21);
                }
                else
                {
                    xPos = Random.Range(-26, -10);
                    zPos = Random.Range(38, 42);
                }

                if (((-30.5f < xPos) && (xPos < -22.8f)) && ((30.2f < zPos) && (zPos < 34.9f)))
                {
                    // Demarcating the top of Hill region..
                    yPos = Random.Range(2, 5);
                    yPos += 1.0f;
                }
                else if (((-29f < xPos) && (xPos < -21f)) && ((23f < zPos) && (zPos < 29f)))
                {
                    // Demarcating the stone on the Hill portion..
                    yPos = Random.Range(1, 4);
                    yPos += 0.8f;
                }
                else if (((-26 < xPos) && (xPos < 10.2f)) && ((38 < zPos) && (zPos < 42)))
                {
                    yPos = Random.Range(0, 2);
                    yPos += 0.3f;
                }
            }

            Instantiate(CoinInstance, new Vector3(xPos, yPos, zPos), Quaternion.identity); // Instantiate / Spawn the Coin Object in Scene
            yield return new WaitForSeconds(0.001f);
            coinCount += 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoinSpawn()); // intiate coin spawner at the initialization of the game scene
    }

}
