using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SDF2Json : MonoBehaviour
{
    public Texture3D sdf;
    public Vector3 center, size;

    private void Start()
    {
        if (sdf != null)
        {
            List<float> datas = new List<float>();

            for (int i = 0; i < sdf.width; i++)
            {
                for (int j = 0; j < sdf.height; j++)
                {
                    for (int k = 0; k < sdf.depth; k++)
                    {
                        var data = sdf.GetPixel(i, j, k);
                        datas.Add(data.r);
                    }
                }
            }

            MySDFData mySDF = new MySDFData(sdf.name, sdf.width, sdf.height, sdf.depth, datas.ToArray());
            mySDF.SetInfo(center, size);

            JsonSerialization.Serialize(mySDF);
        }
    }
}

[System.Serializable]
public class MySDFData: ISerializableData
{
    string name;

    public int width, height, depth;
    public float[] datas;

    public Vector3 position;
    public float gridSize;
    public int maxResolution;

    public string FileName => name;
    public string FolderName => "";

    public MySDFData(string name, int w, int h, int d, float[] datas)
    {
        this.name = name;
        this.width = w;
        this.height = h;
        this.depth = d;
        this.datas = datas;
    }

    public float GetDistance(int x, int y, int z)
    {
        return datas[x * height * depth + y * depth + z];
    }

    public void SetInfo(Vector3 center, Vector3 size)
    {
        this.position = center - size / 2f;
        maxResolution = Mathf.Max(new int[] { width, height, depth });
        float maxLength = Mathf.Max(new float[] { size.x, size.y, size.z });
        gridSize = maxLength / maxResolution;
    }

    public float GetDistance(Vector3 pos)
    {
        Vector3Int gridPos = World2Grid(pos);

        if (!InBox(gridPos)) return 0;

        return GetDistance(gridPos.x, gridPos.y, gridPos.z);
    }

    private bool InBox(Vector3Int gridPos)
    {
         return gridPos.x < width - 1 && gridPos.y < height - 1 && gridPos.z < depth - 1 && gridPos.x >= 0 && gridPos.y >= 0 && gridPos.z >= 0;
    }

    private Vector3Int World2Grid(Vector3 pos)
    {
        return new Vector3Int((int)((pos.x - position.x)/gridSize), 
            (int)((pos.y - position.y)/gridSize), 
            (int)((pos.z - position.z)/gridSize));
    }
}