using UnityEngine;

using UnityEngine.EventSystems;


namespace Battlehub.UIControls
{
    public class ExternalDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
    {
        public TreeView TreeView;


        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            TreeView.ExternalBeginDrag(eventData.position);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            TreeView.ExternalItemDrag(eventData.position);
        }

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if (TreeView.DropTarget != null)
            {
                TreeView.AddChild(TreeView.DropTarget, new GameObject());
            }

            TreeView.ExternalItemDrop();           
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
           TreeView.ExternalItemDrop();
        }       
    }
}
