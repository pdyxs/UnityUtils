using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class LoadSceneButton : MonoBehaviour {

    public SceneId scene;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(Clicked);
	}

    private void Clicked()
    {
        SceneManager.LoadScene(scene.name);
    }
}
