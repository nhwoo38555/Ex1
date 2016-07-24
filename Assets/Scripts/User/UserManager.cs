using System;
using UnityEngine;

namespace Assets.Scripts.User
{
	public sealed class UserManager
	{
		private const float PLAYER_HEIGHT = 2.0f;
        private const float TURN_SPEED = 100.0f;

		private UserManager() { }
		public void SetPlayerCamera(Camera camera_)
		{
			if (null != camera_ )
			{
				if( null != this.userGO )
				{
					camera_.transform.SetParent(this.userGO.transform);
                    camera_.transform.localRotation = Quaternion.identity; //고정
                    camera_.transform.localPosition = new Vector3(0f, PLAYER_HEIGHT, 0f); 
                }
				this.playerCamera = camera_;

			}
		}
		public void SetUserGameObject(GameObject userGameObject_)
		{
			this.userGO = userGameObject_;
			if( null != this.userGO )
			{
                this._navMeshAgent = this.userGO.GetComponentInChildren<NavMeshAgent>();
			}
		}

		public void UpdateCustomPre()
		{
			_UpdateMoveInput();
		}

		public void UpdateCustom()
		{
			_UpdateMove();
		}
		private void _UpdateMoveInput()
		{
			this._moveDirection = Vector3.zero;

            this._moveDirection = new Vector3(0, Input.GetAxis("Mouse X"), 0);
            this._rotateDirection = new Vector3(Input.GetAxis("Mouse Y"),0, 0);

            //앞방향
            if (true == Input.GetKey(KeyCode.UpArrow))
			{
				this._moveDirection += new Vector3(0f, 0f, 1f);
			}
			//뒤방향
			if (true == Input.GetKey(KeyCode.DownArrow))
			{
				this._moveDirection += new Vector3(0f, 0f, -1f);
			}
			//앞방향
			if (true == Input.GetKey(KeyCode.RightArrow))
			{
				this._moveDirection += new Vector3(1f, 0f, 0f);
			}
			//뒤방향
			if (true == Input.GetKey(KeyCode.LeftArrow))
			{
				this._moveDirection += new Vector3(-1f, 0f, 0f);
			}



		}
		private void _UpdateMove()
		{
			if (null != this._navMeshAgent && this._moveDirection != Vector3.zero )
			{
                
				Vector3 moveOffset = this._moveDirection.normalized * this._navMeshAgent.speed * Time.deltaTime;
				this._navMeshAgent.Move(moveOffset);
                this.userGO.transform.Rotate(this._moveDirection * TURN_SPEED * Time.deltaTime);
                this.playerCamera.transform.Rotate(this._rotateDirection * TURN_SPEED * Time.deltaTime);
            }
		}

		public string userID { get; set; }
		public GameObject userGO { get; private set; }
		public Camera playerCamera { get; private set; }
		private NavMeshAgent _navMeshAgent = null;
		private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _rotateDirection = Vector3.zero;
        #region SINGLETON
        private static UserManager _instance = null;
		public static UserManager Instance { get { if (null != _instance) { return _instance; } else { return (_instance = new UserManager()); } } }
		#endregion
	}
}
