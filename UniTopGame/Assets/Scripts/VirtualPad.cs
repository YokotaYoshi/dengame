using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPad : MonoBehaviour
{
    public float MaxLength = 70;
    public bool is4DPad = true;
    GameObject player;
    Vector2 defPos;
    Vector2 downPos;

    // Start is called before the first frame update
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");
        defPos = GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PadDown()
    {
        downPos=Input.mousePosition;
    }
    public void PadDrag()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 newTabPos = mousePosition - downPos;
        if (is4DPad == false)
        {
            newTabPos.y = 0;
        }
        Vector2 axis = newTabPos.normalized;
        float len = Vector2.Distance(defPos, newTabPos);
        if (len > MaxLength)
        {
            newTabPos.x = axis.x * MaxLength;
            newTabPos.y = axis.y * MaxLength;
        }
        GetComponent<RectTransform>().localPosition = newTabPos;
        PlayerScript plcnt = player.GetComponent<PlayerScript>();
        plcnt.SetAxis(axis.x, axis.y);
    }
    public void PadUp()
    {
        GetComponent<RectTransform>().localPosition = defPos;
        PlayerScript plcnt = player.GetComponent<PlayerScript>();
        plcnt.SetAxis(0,0);
    }
    public void Attack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ArrowShoot shoot = player.GetComponent<ArrowShoot>();
        shoot.Attack();
    }
}
