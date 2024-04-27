using UnityEngine;
using UnityEngine.UI;

public class TouchMovement : MonoBehaviour
{
    [SerializeField]
    private Text _directionText;
    [SerializeField]
    private float _moveSpeed;

    private Rigidbody _rigidBody;

    private Touch _touch;
    private Vector2 _touchStartPosition, _touchEndPosition;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>(); 
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0); 

            if(_touch.phase == TouchPhase.Began)
            {
                _touchStartPosition = _touch.position;
            }
            else if(_touch.phase == TouchPhase.Moved /*|| _touch.phase == TouchPhase.Ended*/)
            {
                _touchEndPosition = _touch.position;

                float horizontalMovement = _touchEndPosition.x - _touchStartPosition.x;
                float verticalMovement = _touchEndPosition.y - _touchStartPosition.y;

                if(Mathf.Abs(horizontalMovement) == 0 && Mathf.Abs(verticalMovement) == 0)
                {
                    _directionText.text = "Tapped";
                }

                Vector3 movement = new Vector3(-verticalMovement, 0f, horizontalMovement).normalized * _moveSpeed;
                _rigidBody.velocity = movement;
            }
            else
            {
                //_rigidBody.velocity = Vector3.zero;
            }
        }
    }

    // up down movement
    private void MoveX(float moveHorizontal)
    {
        Vector3 movement = new Vector3(moveHorizontal, 0, 0);
        _rigidBody.velocity = movement * _moveSpeed * Time.deltaTime;
    }

    // left and right movement 
    private void MoveZ(float moveVertical)
    {
        Vector3 movement = new Vector3(0, 0, moveVertical);
        _rigidBody.velocity = movement * _moveSpeed * Time.deltaTime;
    }
}
