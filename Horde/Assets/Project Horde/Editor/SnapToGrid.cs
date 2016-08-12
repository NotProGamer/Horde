using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class SnapToGrid : ScriptableObject 
{
 
    	[MenuItem ("Window/Snap to Grid %g")]
	    static void MenuSnapToGrid() 
        {
		    foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) 
            {
	    	    t.position = new Vector3 
                (
				Mathf.Round(t.position.x / EditorPrefs.GetFloat("MoveSnapX")) * EditorPrefs.GetFloat("MoveSnapX"),
				Mathf.Round(t.position.y / EditorPrefs.GetFloat("MoveSnapY")) * EditorPrefs.GetFloat("MoveSnapY"),
				Mathf.Round(t.position.z / EditorPrefs.GetFloat("MoveSnapZ")) * EditorPrefs.GetFloat("MoveSnapZ")
                );
		    }
	    } 
}