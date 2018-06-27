using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public TextAsset DataSource;
    public GameObject GraphPart;
    public float RowWidth;
    public float RowMargin;

    private List<SubjectBehaviour> _behaviours;

    void Start()
    {
        string[] entries = DataSource.text.Split('\n');
        List<SubjectData> data = entries.Select(ParseDataSource).ToList();
        data = data.OrderBy(item => item.AStar).ToList();
        _behaviours = new List<SubjectBehaviour>();
        foreach (SubjectData datum in data)
        {
            GameObject obj = new GameObject(datum.SubjectName);
            SubjectBehaviour behaviour = obj.AddComponent<SubjectBehaviour>();
            behaviour.Mothership = this;
            behaviour.Data = datum;
            obj.transform.parent = transform;
            _behaviours.Add(behaviour);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _behaviours.Count; i++)
        {
            float pos = i * (RowWidth + RowMargin);
            _behaviours[i].transform.position = new Vector3(pos, 0, 0);
            _behaviours[i].transform.localScale = new Vector3(RowWidth, 1, 1);
        }
    }

    private SubjectData ParseDataSource(string line)
    {
        string[] components = line.Split('\t');
        SubjectData ret = new SubjectData();
        ret.SubjectName = components[0];
        ret.AStar = Convert.ToSingle(components[1]);
        ret.A = Convert.ToSingle(components[2]);
        ret.B = Convert.ToSingle(components[3]);
        ret.C = Convert.ToSingle(components[4]);
        ret.D = Convert.ToSingle(components[5]);
        ret.E = Convert.ToSingle(components[6]);
        ret.Other = Convert.ToSingle(components[7]);
        ret.Students = Convert.ToInt32(components[8]);
        return ret;
    }
}

public class SubjectBehaviour : MonoBehaviour
{
    public MainScript Mothership;
    public SubjectData Data;
    public Material mat;
    
    private void Start()
    {
        GameObject bar = Instantiate(Mothership.GraphPart);
        bar.name = name;
        bar.transform.parent = this.transform;
        MeshRenderer renderer = bar.GetComponent<MeshRenderer>();
        mat = renderer.material;
    }

    private void Update()
    {
        mat.SetFloat("_A", Data.AStar);
        mat.SetFloat("_B", Data.A);
        mat.SetFloat("_C", Data.B);
        mat.SetFloat("_D", Data.C);
        mat.SetFloat("_E", Data.D);
        mat.SetFloat("_F", Data.E);
    }
}

public class SubjectData
{
    public string SubjectName;
    public float AStar;
    public float A;
    public float B;
    public float C;
    public float D;
    public float E;
    public float Other;
    public int Students;
}
