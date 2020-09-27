using System.Collections;
using System.IO;
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

    [SerializeField]
    KeyCode _saveKey = KeyCode.S;


    [SerializeField]
    KeyCode _loadKey = KeyCode.L;


    [Header("Saving - Details")]
    [SerializeField]
    string _saveFileName = "NewFile";

    #endregion


    #region Run Time

    List<Transform> _allObjects = default;
    string _applicationSavePath = default;

    #endregion

    #region Unity LifeCycle Methods

    private void Awake()
    {
        _allObjects = new List<Transform>();
        InitializeApplicationPath();
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
            CreateObject();
        }
        else if (Input.GetKeyDown(_destroyKey))
        {
            StartNewGame();
        }
        else if (Input.GetKeyDown(_saveKey))
        {
            Save();
        }
        else if (Input.GetKeyDown(_loadKey))
        {
            Load();
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

    #region Read & Write Methods

    void InitializeApplicationPath()
    {
        _applicationSavePath = Path.Combine(Application.persistentDataPath, "Game");

        if (!Directory.Exists(_applicationSavePath))
        {
            //Create directory folder
            Directory.CreateDirectory(_applicationSavePath);
        }

        //Append the rest of the savefile name to ensure that the file will exist
        _applicationSavePath = Path.Combine(_applicationSavePath, _saveFileName);


        Debug.Log(_applicationSavePath);
    }

    void Save()
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(_applicationSavePath, FileMode.Create)))
        {
            writer.Write(_allObjects.Count);
        }



    }

    void Load()
    {
        using (BinaryReader reader = new BinaryReader(File.Open(_applicationSavePath, FileMode.Open)))
        {
            int count = reader.Read();
            Debug.Log(count);
        }

    }
    #endregion



}
