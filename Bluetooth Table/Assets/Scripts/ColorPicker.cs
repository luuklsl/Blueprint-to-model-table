using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public static Color SelectedColor { get; private set; }

    [SerializeField]
    private Renderer selectedColorPreview; //Where we store the selected color, needed for our purpose?
  
    private void Update() 
    {
        if (Input.GetMouseButton(0)) //Check if user clicked the frame
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Cast ray to check where the user has clicked

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var picker = hit.collider.GetComponent<ColorPicker>();      //Select color, we could transform this to textures?
                if (picker != null)
                {
                    Renderer rend = hit.transform.GetComponent<Renderer>();
                    MeshCollider meshCollider = hit.collider as MeshCollider;

                    if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                        return;

                    Texture2D tex = rend.material.mainTexture as Texture2D;
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= tex.width;
                    pixelUV.y *= tex.height;
                    SelectedColor = tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);

                    selectedColorPreview.material.color = SelectedColor;        //Set preview to this color
                }
            }
        }
    }
}