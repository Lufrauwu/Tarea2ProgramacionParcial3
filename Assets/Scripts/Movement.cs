using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D = default;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _speed = default;
    [SerializeField] private bool _isGrounded = default;
    [SerializeField] private Transform _groundCheck = default;
    private Animator _animator = default;
    private bool _facingRight = true;
    private readonly int ahSpeed = Animator.StringToHash("SPEED");
    public float RadiusDetection = default;
    public float _jumpHeight = 7;
    private float _InputX = default;
    private const string _floorTag = "Floor";
    private float _InputY = default;
    private bool _animationState = false;
    public int _basejumps = 2;
    public int _jumps = 2;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


        if (Input.GetKey(KeyCode.S))
        {
            _InputX = Input.GetAxisRaw("Horizontal") * 0;

            _InputY = _rigidbody2D.velocity.y;
            _rigidbody2D.velocity = new Vector2(_InputX * _speed, _InputY);
            _jumpHeight = 0;
            _animator.SetBool("CROUCH", true);
            if (_jumps == 0)
            {
                _jumps ++;
            }
        }
        else
        {
            _InputX = Input.GetAxis("Horizontal");
            if (_InputX < 0 && _facingRight)
            {
                Flip();
            }
            else if (_InputX > 0 && !_facingRight)
            {
                Flip();
            }
            _InputY = _rigidbody2D.velocity.y;
            _rigidbody2D.velocity = new Vector2(_InputX * _speed, _InputY);
            _jumpHeight = 7;
            _animator.SetBool("CROUCH", false);
        }

        _animationState = _isGrounded ? false : true;
        _animator.SetBool("JUMP", _animationState);
        _animator.SetFloat("SPEED", Mathf.Abs(_InputX));
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, RadiusDetection, _whatIsGround);
    }
    
    private void Jump()
    {
        if (_jumps > 0)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpHeight);
            _jumps--;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == _floorTag )
        {
            _jumps = _basejumps;
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
