using UnityEngine;

public class ToggleObject : MonoBehaviour {

	public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
