using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggedGameObject : MonoBehaviour
{

    private UnityEngine.GameObject myTag;               //The specific tag of this object)

    private void CreateTag()
    {
        if (myTag == null)
        {
            myTag = Instantiate(Resources.Load("Prefab/Tag", typeof(UnityEngine.GameObject))) as UnityEngine.GameObject;
            myTag.transform.parent = UnityEngine.GameObject.Find("Tags").transform;
            myTag.GetComponent<UITag>().Owner = this.gameObject;
        }
    }

    private void Awake()
    {
        CreateTag();
    }

    private void OnDestroy()
    {
        if (myTag != null)
        {
            UnityEngine.GameObject.Destroy(myTag.gameObject);
        }
    }

    private void OnEnable()
    {
        CreateTag();
        myTag.SetActive(true);
    }

    private void OnDisable()
    {
        if (myTag != null)
        {
            myTag.SetActive(false);
        }
    }

}
