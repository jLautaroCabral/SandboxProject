using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public static GUIManager sharedInstance;
    public Texture2D blackBoxSemiTrans;

    private void Awake()
    {
        if(sharedInstance == null) sharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Texture2D getBlackBoxSemiTrans()
    {
        return blackBoxSemiTrans;
    }
}
