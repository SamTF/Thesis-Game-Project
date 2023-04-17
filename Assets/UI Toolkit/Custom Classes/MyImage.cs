using UnityEngine.UIElements;
 
/// <summary>
/// Allows adding an Image control thing to the Unity UI Builder panel. Its sprite still needs to be set via script tho.
/// </summary>
public class MyImage : Image
{
    public new class UxmlFactory : UxmlFactory<MyImage, Image.UxmlTraits>{}
 
    public MyImage()
    {
       
    }
}