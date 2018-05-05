using UnityEngine;

public class CameraController : MonoBehaviour {

	public float offsetx;
	public float offsety;
	public float offsetz;

	Vector3 cameraPosition;
	public GameObject player;

	void LateUpdate() {
		cameraPosition.x = player.transform.position.x - offsetx;
		cameraPosition.y = player.transform.position.y - offsety;
		cameraPosition.z = player.transform.position.z - offsetz;
		
		transform.position = cameraPosition;
		transform.LookAt(player.transform);
	}

}
