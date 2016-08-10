using UnityEngine;
using UnityEditor;
using System.Collections;

public class MaterialsPostProcessor : AssetPostprocessor
{
    //The path where you keep your "Standard" library of materials
    public string StandardPath = "Assets/Models/Materials/";
    public string TempPath = "Assets/Models/Materials/Temp";

    Material OnAssignMaterialModel (Material material, Renderer renderer)
    {
         string StandardMatPath = StandardPath + material.name + ".mat";

         //the path where you want to keep your "temp" materials, that weren't found above
         //So you have a logical place to look for materials that need editing and tweaking for realtime work.
         string TemporaryMatPath = TempPath + material.name + ".mat";
         //Check for it in the standard 
         if (AssetDatabase.LoadAssetAtPath(StandardMatPath, typeof(Material)))
         {
             Debug.Log("FOUND: " + StandardMatPath);
             return (Material)AssetDatabase.LoadAssetAtPath(StandardMatPath, typeof(Material));
         }
         //Else check to see if we've already built one in the temp path
         if (AssetDatabase.LoadAssetAtPath(TemporaryMatPath, typeof(Material)))
         {
             Debug.Log("FOUND temp: " + TemporaryMatPath);
             return (Material)AssetDatabase.LoadAssetAtPath(TemporaryMatPath, typeof(Material));
         }
         //Or create it?
         material.shader = Shader.Find("Diffuse");
         AssetDatabase.CreateAsset(material, TemporaryMatPath);
         Debug.Log("CREATED temp: " + TemporaryMatPath);
         return material;
    }
}

