using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    public GameObject groundPrefab;
    public GameObject mainGround;
    public GameObject character;

    private float farthestDistance = 10f;

    private void Update()
    {
        mainGround.transform.position = new Vector2(character.transform.position.x, -1.98f);
        if (farthestDistance - character.transform.position.x < 10)
        {
            farthestDistance += 10;
            AutoGenerate();
        }
    }

    private void AutoGenerate()
    {
        
        for (int i = 0; i < 5; i++)
        {
            GameObject generatedGround = Instantiate(groundPrefab, new Vector3((farthestDistance - 10) + 2 * i, Random.Range(-1.5f, -0.5f), 0), Quaternion.Euler(0, 0, Random.Range(-12, 12)));
            generatedGround.transform.localScale = new Vector3(Random.Range(1f, 3f), 1, 1);
        }
    }


}
