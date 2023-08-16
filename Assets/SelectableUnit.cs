using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnit : MonoBehaviour
{

    private Renderer renderer;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnMouseDown()
    {
        //replace this "lookup by name" with a gameobject registry that can key-value to player
        string playerName = this.transform.parent.gameObject.GetComponent<TextMesh>().text;
        Player player = Entrypoint.instance.game.GetPlayer(playerName);
        SelectedUnit.instance.unit = player;
    }

    private void OnMouseEnter()
    {
        originalColor = renderer.material.color;
        renderer.material.color = Color.red;
        print("MOUSE ENTERRING");
    }

    private void OnMouseExit()
    {
        renderer.material.color = originalColor;
        print("mOUSE eXITING");
    }

}
