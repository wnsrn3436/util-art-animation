using UnityEngine;

public class ArtApply : MonoBehaviour
{
    public Texture2D artTerrain;
    public Texture2D artBone;
    public Texture2D source;

    [HideInInspector]
    public Texture texture;

    //void Update()
    //{
    //    if (Input.GetKey(KeyCode.Return))
    //    {
    //        TextureLoader.Copy(source, artTerrain);
    //        TextureLoader.Copy(source, artBone);
    //        TextureLoader.ClearOverlap(artTerrain, artBone);
    //    }
    //}
}