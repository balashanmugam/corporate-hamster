using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSystem : MonoBehaviour {


	
	[SerializeField]
	private float rotationSpeed;

	[SerializeField]
	private int direction = 1;

	private Transform _transform;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		_transform = this.GetComponent<Transform>();
	}
    public float RoationSpeed
    {
        get
        {
            return rotationSpeed;
        }

        set
        {
            rotationSpeed = value;
        }
    }

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(GameManager.currentGameState == GameStates.Game || GameManager.currentGameState == GameStates.PowerUp)
		_transform.Rotate(new Vector3(0,0,1 * rotationSpeed * direction * Time.deltaTime));	
	}
}
