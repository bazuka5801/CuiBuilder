using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oxide.Game.Rust.Cui;
using UnityEngine;

public class CopyPaste : MonoBehaviour {

    private List<CuiElement> clipboardBuffer = new List<CuiElement>();

    public void Copy()
    {
        clipboardBuffer = CollectionManager.GetSelectedCui();
        clipboardBuffer.ForEach(AddRectPixelComponent);
    }

    public void Paste()
    {
        foreach (var element in clipboardBuffer)
        {
            var cui = CuiManager.Create();
            var pixelComponent = element.Components.OfType<RectPixelComponent>().FirstOrDefault();
            element.Components.Remove(pixelComponent);
            cui.Load(element);
            var rTransform = cui.GetComponent<RectTransformComponent>();
            rTransform.SetPixelLocalPosition(pixelComponent.Position);
            rTransform.SetPixelSize( pixelComponent.Size);
        }
    }

    private void AddRectPixelComponent(CuiElement element)
    {
        var rTransform = (RectTransform)Hierarchy.FindByName(element.Name).transform;
        element.Components.Add(new RectPixelComponent()
        {
            Position = rTransform.GetPositionPixelLocal(),
            Size = rTransform.GetSizePixelWorld()
        });
    }

    private class RectPixelComponent : ICuiComponent
    {
        public string Type { get { return "RectPixel"; } }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
    }
    
}
