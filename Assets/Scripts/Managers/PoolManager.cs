using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PoolingData
{
    [Tooltip("풀링 이름 비어있을시 오브젝트 이름으로")] public string name;
    public int createCount = 0;
    [OnValueChanged(nameof(OnObjectChanged))] public GameObject originObject;
    public List<GameObject> poolingList;
    private void OnObjectChanged()
    {
        if (string.IsNullOrEmpty(name))
            name = originObject.name;
    }
}

public class PoolManager : Singleton<PoolManager>
{
    private readonly Dictionary<string, List<GameObject>> pools = new();
    private readonly Dictionary<string, GameObject> originObjects = new();

    [SerializeField] private List<PoolingData> poolingDataList = new();

    protected override void OnCreated()
    {
        foreach (var data in poolingDataList)
        {
            string poolName = string.IsNullOrEmpty(data.name) ? data.originObject.name : data.name;
            originObjects.Add(poolName, data.originObject);

            if (data.createCount > 0)
                CreatePoolingData(poolName, data.createCount);

            if (data.poolingList.Count <= 0) continue;

            pools.Add(poolName, new List<GameObject>());
            foreach (var obj in data.poolingList)
            {
                pools[poolName].Add(obj);
                obj.gameObject.SetActive(false);
            }
        }

        poolingDataList.Clear();
    }

    public void JoinPoolingData(string objName, GameObject obj)
    {
        if (!originObjects.ContainsKey(objName))
            originObjects.Add(objName, obj);
    }

    public void CreatePoolingData(string objName, int count = 1, Transform parent = null)
    {
        if (string.IsNullOrEmpty(objName)) return;

        if (!originObjects.ContainsKey(objName))
        {
            Debug.Log(objName + " Pooling Error");
            return;
        }

        if (!pools.ContainsKey(objName))
            pools.Add(objName, new List<GameObject>());

        var objects = pools[objName];

        if (objects.Count > count) return;

        for (int i = 0; i < count; i++)
        {
            GameObject copy = parent != null ? Instantiate(originObjects[objName], parent) : Instantiate(originObjects[objName]);
            copy.SetActive(false);

            objects.Add(copy);
        }
    }

    public GameObject Init(string objName, Transform parent = null)
    {
        if (string.IsNullOrEmpty(objName)) return null;
        if (poolingDataList.Count > 0) OnCreated();

        GameObject copy;
        if (pools.ContainsKey(objName))
        {
            if (pools[objName].FindAll((x) => !x.activeSelf).Count > 0)
            {
                copy = pools[objName].Find((x) => !x.activeSelf);
                copy.SetActive(true);

                return copy;
            }
        }
        else
        {
            pools.Add(objName, new List<GameObject>());
        }

        if (!originObjects.ContainsKey(objName))
        {
            Debug.Log(objName + " Pooling Error");
            return null;
        }

        copy = parent != null ? Instantiate(originObjects[objName], parent) : Instantiate(originObjects[objName]);
        copy.SetActive(true);

        pools[objName].Add(copy);
        return copy;
    }

    protected override void OnReset()
    {
        foreach (var obj in pools.Values.SelectMany(objs => objs))
            obj.gameObject.SetActive(false);
    }
}