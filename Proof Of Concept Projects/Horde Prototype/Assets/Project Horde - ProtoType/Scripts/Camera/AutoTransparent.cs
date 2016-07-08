using UnityEngine;
using System.Collections;

/// <summary>
///  Code Author: Reece Howe
///  Project: Horde
///  Engine: Unity
///  Platform: Mobile and PC
///  Notes: This is an incomplete piece of code to be used for making buildings transparent when mobs are behind them.
///  Status: Incomplete
/// </summary>




public class AutoTransparent : MonoBehaviour {

    // reference : http://answers.unity3d.com/questions/44815/make-object-transparent-when-between-camera-and-pl.html

    private Shader m_oldShader = null;
    private Color m_oldColor = Color.black;
    private float m_transparency = 0.3f;
    private const float m_targetTransparency = 0.3f;
    private const float m_fallOff = 0.1f;

    private Renderer m_artRenderer;

    
    void Awake ()
    {
        m_artRenderer = transform.GetComponentInChildren<Renderer>();
        if (m_artRenderer == null)
        {
            Debug.Log("No Renderer included");
        }

    }

    public void BeTransparent()
    {
        // reset the transparency
        m_transparency = m_targetTransparency;
        if (m_oldShader = null)
        {
            m_oldShader = m_artRenderer.material.shader;
            m_oldColor = m_artRenderer.material.color;
            m_artRenderer.material.shader = Shader.Find("Transparent/Diffuse");
        }
    }


	// Update is called once per frame
	void Update ()
    {
        if (m_transparency < 1.0f)
        {
            Color colour = m_artRenderer.material.color;
            colour.a = m_transparency;
            m_artRenderer.material.color = colour;
        }
        else
        {
            // Reset the shader
            m_artRenderer.material.shader = m_oldShader;
            m_artRenderer.material.color = m_oldColor;
            // and remove this 
        }

	}
}
