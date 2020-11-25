using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class BloodcellSpawner : MonoBehaviour
{
    
    public GameObject bloodCellPrefab;
    public List<Sprite> bloodCellSprites;
    public List<PathCreator> paths;
    public List<Transform> EndPositions;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(SpawnBloodCells());
    }

    IEnumerator SpawnBloodCells()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            SpawnCell();
        }

    }

    void SpawnCell()
    {
        GameObject cell = Instantiate(bloodCellPrefab, transform.position, Quaternion.identity);
        PathFollower follower = cell.GetComponent<PathFollower>();
        follower.pathCreator = paths[index];

        BloodCell bc = cell.GetComponent<BloodCell>();
        bc.endPos = EndPositions[index];

        SpriteRenderer render = cell.GetComponent<SpriteRenderer>();
        render.sprite = bloodCellSprites[Random.Range(0, bloodCellSprites.Count)];

        index++;
        index = index % paths.Count;

        
    }
}
