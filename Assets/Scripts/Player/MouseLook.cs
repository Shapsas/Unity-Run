using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	public Transform player;

	void Update ()
	{
		if(MyInput.myInput.inputIsAvailable)
        {
			transform.position = player.transform.position;

			float ySens = sensitivityY;

			if (axes == RotationAxes.MouseXAndY)
			{
				float rotationX = transform.localEulerAngles.y + Mouse.current.delta.ReadValue().x * sensitivityX;

				player.parent.transform.localEulerAngles = new Vector3(0f, rotationX, 0f);

				rotationY += Mouse.current.delta.ReadValue().y * ySens;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Mouse.current.delta.ReadValue().x * sensitivityX, 0);
			}
			else
			{
				rotationY += Mouse.current.delta.ReadValue().y * ySens;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}
	}
}