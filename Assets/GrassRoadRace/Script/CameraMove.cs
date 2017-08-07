
using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public float moveSpeed;
	public GameObject mainCamera;
	public Animator animator;
	public Vector3 mouseClickPos = Vector3.zero;
	public float doubleClick = 0F;
	public float scaleY = -3.0F;
	private bool mouseState = false;
	// Use this for initialization
	void Start () {
		//mainCamera.transform.localPosition = new Vector3 ( 0, 0, 0 );
		//mainCamera.transform.localRotation = Quaternion.Euler (18, 180, 0);

	}

	void OnEnable(){
		//GameObject objPrefab = GameObject.Instantiate((GameObject)Resources.Load("shibing"));
		//objPrefab.transform.SetParent(transform.parent);
		//objPrefab.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
		Monster3D _monster3D = (Monster3D)PoolManager.getInstance().getGameObject("Monster3D");
		_monster3D.transform.SetParent(transform.parent);
		_monster3D.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
		_monster3D.init (null);
	}
	
	// Update is called once per frame
	void Update () {

		
	}

	void FixedUpdate()
	{
		bool isClick = Input.GetMouseButton (0);
		if (isClick) {
			

			{
				if (mouseClickPos == Vector3.zero) {
					mouseClickPos = Input.mousePosition;
				} else {
					float _x = mouseClickPos.x - Input.mousePosition.x;
					float _y = mouseClickPos.y - Input.mousePosition.y;
					transform.Translate (_x / 10, 0f, _y / 10);	
					mouseClickPos = Input.mousePosition;
				}
			}
			if (!mouseState) {
				mouseState = true;
				float t = Time.time;
				Debug.Log ("OnClickOk:" + t.ToString() + "     " + doubleClick.ToString());
				if (t - doubleClick < 0.3) {//双击放大场景
					transform.Translate (0,scaleY,0);
					scaleY = -scaleY;
				} 
				doubleClick = Time.time;
			}


		} else {
			if (mouseState) {
				mouseState = false;
				mouseClickPos = Vector3.zero;
			}


		}
		//MoveObj ();
		
		if (Input.GetKeyDown (KeyCode.A)) {
			ChangeView01();
		}
		
		if (Input.GetKeyDown (KeyCode.S)) {
			ChangeView02();
		}
	}
	
	
	void MoveObj() {		
		float moveAmount = Time.smoothDeltaTime * moveSpeed;
		transform.Translate ( 0f, 0f, moveAmount );	
	}



	void ChangeView01() {
		transform.position = new Vector3 (0, 2, 10);
		// x:0, y:1, z:52
		mainCamera.transform.localPosition = new Vector3 ( -8, 2, 0 );
		mainCamera.transform.localRotation = Quaternion.Euler (14, 90, 0);
	}

	void ChangeView02() {
		transform.position = new Vector3 (0, 2, 10);
		// x:0, y:1, z:52
		mainCamera.transform.localPosition = new Vector3 ( 0, 0, 0 );
		mainCamera.transform.localRotation = Quaternion.Euler ( 19, 180, 0 );
		moveSpeed = -20f;
		
	}
}























