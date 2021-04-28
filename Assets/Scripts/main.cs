using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField inputB;
    [SerializeField] TMPro.TMP_InputField inputL;
    [SerializeField] TMPro.TMP_InputField inputT;
    [SerializeField] TMPro.TMP_InputField inputNumB;
    [SerializeField] TMPro.TMP_InputField inputNumL;
    [SerializeField] TMPro.TMP_InputField inputNumT;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        text.SetActive(false);
        //var path = StandaloneFileBrowser.OpenFolderPanel("保存先を選択", "", false);
        //Debug.Log(path[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void click()
    {
        StartCoroutine(_click());
    }
    IEnumerator _click()
    {
        panel.SetActive(true);
        text.SetActive(true);
        var path = StandaloneFileBrowser.OpenFolderPanel("保存先を選択", "", false);
        Debug.Log(path[0]);
        meshController meshController = new meshController(float.Parse(inputB.text), float.Parse(inputL.text), float.Parse(inputT.text), text.GetComponent<TMPro.TMP_Text>());
        yield return StartCoroutine(meshController.createNodeList(int.Parse(inputNumB.text), int.Parse(inputNumL.text), int.Parse(inputNumT.text)));
        meshController.fixNode(0.0f);
        meshController.saveFile(path[0]);
        panel.SetActive(false);
        text.GetComponent<TMPro.TMP_Text>().text = "Finish";
    }
}
