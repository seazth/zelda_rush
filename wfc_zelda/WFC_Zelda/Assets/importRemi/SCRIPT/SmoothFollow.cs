using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	#region Consts
	private const float SMOOTH_TIME = 0.3f;
	#endregion
	
	#region Public Properties
	public float offSetZ = -13f;
	public float offSetY = 13f;
	public float mouseWheelOffset = 1f;
    public bool LockX;
	public bool LockZ;
	public bool useSmoothing;
	public Transform target;
	#endregion
	
	#region Private Properties
	private Transform thisTransform;
	private Vector3 velocity;
	#endregion

	bool hudActive = true;
	private void Awake()
	{
		thisTransform = transform;
		velocity = new Vector3(0.5f, 0.5f, 0.5f);
	}

	void Update()
	{
        if (Input.mouseScrollDelta.y != 0.0f)
        {
            offSetZ -= mouseWheelOffset * Input.mouseScrollDelta.y;
            offSetY += mouseWheelOffset * Input.mouseScrollDelta.y;
        }
	}

	// ReSharper disable UnusedMember.Local
	private void LateUpdate()
		// ReSharper restore UnusedMember.Local
	{
		var newPos = Vector3.zero;
		
		if (useSmoothing)
		{
			newPos.x = Mathf.SmoothDamp(thisTransform.position.x, target.position.x, ref velocity.x, SMOOTH_TIME);
			newPos.y = Mathf.SmoothDamp(thisTransform.position.y, target.position.y, ref velocity.y, SMOOTH_TIME);
			newPos.z = Mathf.SmoothDamp(thisTransform.position.z, target.position.z + offSetZ, ref velocity.z, SMOOTH_TIME);
		}
		else
		{
			newPos.x = target.position.x;
			newPos.y = target.position.y;
			newPos.z = target.position.z;
		}

        newPos.y = Mathf.Lerp(transform.position.y, offSetY+ target.position.y, SMOOTH_TIME);

        if (LockX)
		{
			newPos.x = thisTransform.position.x;
		}
		if (LockZ)
		{
			newPos.z = thisTransform.position.z;
		}
	
		transform.position = Vector3.Slerp(transform.position, newPos, Time.time);
	}
}