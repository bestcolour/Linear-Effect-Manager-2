using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    #region Exposed Variables
    [SerializeField]
    Transform _prefab = default;

    [SerializeField]
    KeyCode _createKey = KeyCode.C;

    [SerializeField]
    KeyCode _destroyKey = KeyCode.D;
    #endregion


    #region Run Time

    List<Transform> _allObjects = default;

    #endregion

    #region Unity LifeCycle Methods

    private void Awake()
    {
        _allObjects = new List<Transform>();
    }

    private void OnEnable()
    {
        StartNewGame();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_createKey))
        {
            Debug.Log("Hi");
            CreateObject();
        }
        else if (Input.GetKeyDown(_destroyKey))
        {
            StartNewGame();
        }


    }

    #endregion


    void StartNewGame()
    {
        for (int i = 0; i < _allObjects.Count; i++)
        {
            Destroy(_allObjects[i].gameObject);
        }

        _allObjects.Clear();
    }

    void CreateObject()
    {
        Transform t = Instantiate(_prefab);
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        _allObjects.Add(t);
    }


}
