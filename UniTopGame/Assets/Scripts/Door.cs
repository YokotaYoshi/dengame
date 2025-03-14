using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangeId = 0;
    public bool IsGoldDoor = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (IsGoldDoor)
            {
                if (ItemKeeper.hasGoldKeys >0)
                {
                    ItemKeeper.hasGoldKeys--;
                    Destroy(this.gameObject);
                    SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
                }
            }
            else 
            {
                if (ItemKeeper.hasSilverKeys > 0)
                {
                    ItemKeeper.hasSilverKeys--;
                    Destroy(this.gameObject);
                    SaveDataManager.SetArrangeId(arrangeId,gameObject.tag);
                }
            }
        }
        
    }
}
