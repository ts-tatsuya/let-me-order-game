using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObjectMovementBehaviour : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed;
    [SerializeField] private UnityEvent _onAllWaypointsReached;

    private int _currentIndex;
    private bool _isExecuting;
    private bool _isUnchild;

    public void Execute()
    {
        if (_waypoints == null) return;
        if (_currentIndex >= _waypoints.Length)
        {
            _onAllWaypointsReached.Invoke();
            return;
        }
        _isExecuting = true;
        GetComponent<Button>().interactable = false;
    }

    private void Start()
    {
        // if (transform.parent != null && transform.parent.parent != null)
        // {
        //     transform.parent = transform.parent.parent;
        // }
        // else
        // {
        //     transform.parent = null;
        // }
    }

    private void Update()
    {
        if (_isExecuting)
        {
            MoveObject();
        }

    }

    private void MoveObject()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _waypoints[_currentIndex].position,
            _speed * Time.deltaTime
        );
        if (transform.position == _waypoints[_currentIndex].position && _isExecuting == true)
        {
            _isExecuting = false;
            GetComponent<Button>().interactable = true;
            _currentIndex++;
        }
    }
}
