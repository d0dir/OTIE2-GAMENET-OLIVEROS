using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 10;
	public float rotationSpeed = 150;
	public float currentSpeed = 0;

	public bool isControlEnabled;

	void Start()
	{
		isControlEnabled = false;
	}
	void LateUpdate()
	{
		if (isControlEnabled)
		{
			float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
			float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

			transform.Translate(0, 0, translation);
			currentSpeed = translation;

			transform.Rotate(0, rotation, 0);
		}
	}
}
