using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class meshController
{
    float b;
    float l;
    float t;
    List<Vector3> nodes;
    List<List<int>> elems;
    List<int> fixs;
    public TMPro.TMP_Text text;
    public meshController(float b,float l,float t,TMPro.TMP_Text text)
    {
        this.b = b;
        this.l = l;
        this.t = t;
        this.text = text;
    }
    public IEnumerator createNodeList(int numB, int numL, int numT)
    {
        this.nodes = new List<Vector3> { };
        this.elems = new List<List<int>> { };
        float stepB = this.b / (float)numB;
        float stepL = this.l / (float)numL;
        float stepT = this.t / (float)numT;
        int count = 0;
        for (int i = 0; i < numT+1; i++)
        {
            text.text = ((float)i / (float)numT*100.0f).ToString()+"%";
            yield return null;
            for (int j = 0; j < numB+1; j++)
            {
                for (int k = 0; k < numL+1; k++)
                {
                    Vector3 node = new Vector3((float)k * stepL, (float)j * stepB, (float)i * stepT);
                    this.nodes.Add(node);
                    /*
                    Vector3 node2 = new Vector3((float)(k + 1) * stepL, (float)j * stepB, (float)i * stepT);
                    Vector3 node3 = new Vector3((float)(k + 1) * stepL, (float)(j + 1) * stepB, (float)i * stepT);
                    Vector3 node4 = new Vector3((float)k * stepL, (float)(j + 1) * stepB, (float)i * stepT);
                    Vector3 node5 = new Vector3((float)k * stepL, (float)j * stepB, (float)(i + 1) * stepT);
                    Vector3 node6 = new Vector3((float)(k + 1) * stepL, (float)j * stepB, (float)(i + 1) * stepT);
                    Vector3 node7 = new Vector3((float)(k + 1) * stepL, (float)(j + 1) * stepB, (float)(i + 1) * stepT);
                    Vector3 node8 = new Vector3((float)k * stepL, (float)(j + 1) * stepB, (float)(i + 1) * stepT);
                    List<Vector3> nodes2 = new List<Vector3> { node1, node2, node3, node4, node5, node6, node7, node8 };
                    List<int> list = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1 };
                    for (int l = 0; l < 8; l++)
                    {
                        int index = this.nodes.IndexOf(nodes2[l]);
                        if(index>-1)
                            list[l] = index;
                    }
                    for (int l = 0; l < 8; l++)
                    {
                        if (list[l] < 0)
                        {
                            list[l] = count;
                            this.nodes.Add(nodes2[l]);
                            count = count + 1;
                        }
                    }
                    this.elems.Add(list);
                    */
                    //yield return null;
                }
            }
        }
        
        for (int i = 0; i < this.nodes.Count; i++)
        {
            if (this.nodes[i].x >= this.l || this.nodes[i].y >= this.b || this.nodes[i].z >= this.t)
                continue;
            List<int> list = new List<int> { i, i + 1, i + (numL + 1) + 1, i + (numL + 1), i + (numB + 1) * (numL + 1), i + (numB + 1) * (numL + 1) + 1, i + (numB + 1) * (numL + 1) + (numL + 1)+1, i + (numB + 1) * (numL + 1) + (numL + 1) };
            this.elems.Add(list);
        }
        
    }
    public void fixNode(float x)
    {
        this.fixs = new List<int> { };
        for (int i = 0; i < this.nodes.Count; i++)
        {
            if (x == this.nodes[i].x)
                this.fixs.Add(i);
        }
    }
    public void saveFile(string dirPath)
    {
        StringBuilder sbNode = new StringBuilder(this.nodes.Count.ToString() + "\n");
        //string nodeStr = this.nodes.Count.ToString() + "\n";
        foreach (Vector3 v in this.nodes)
            sbNode.Append(v.x.ToString() + "," + v.y.ToString() + "," + v.z.ToString() + "\n");
        StreamWriter writer = new StreamWriter(dirPath+"/node.dat", false);
        writer.Write(sbNode.ToString());
        writer.Close();
        //string elemStr = this.elems.Count.ToString() + " 8\n";
        StringBuilder sbElem = new StringBuilder(this.elems.Count.ToString() + " 8\n");
        for (int i = 0; i < this.elems.Count; i++)
        {
            for(int j=0;j<8; j++)
            {
                if (j == 0)
                    sbElem.Append((this.elems[i][j] + 1).ToString());
                else
                    sbElem.Append("," + (this.elems[i][j] + 1).ToString());
            }
            sbElem.Append("\n");
        }
        StreamWriter writer2 = new StreamWriter(dirPath + "/elem.dat", false);
        writer2.Write(sbElem.ToString());
        writer2.Close();
        StringBuilder sbBc = new StringBuilder((3 * this.fixs.Count).ToString() + " 3\n");
        //string bcStr = (3 * this.fixs.Count).ToString() + " 3\n";
        foreach(int i in this.fixs)
        {
            sbBc.Append((i + 1).ToString() + ",1,0.0\n");
            sbBc.Append((i + 1).ToString() + ",2,0.0\n");
            sbBc.Append((i + 1).ToString() + ",3,0.0\n");
        }
        StreamWriter writer3 = new StreamWriter(dirPath + "/bc.dat", false);
        writer3.Write(sbBc.ToString());
        writer3.Close();
    }
}