using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class QueueManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] _queuePositions;
    [SerializeField]
    private GameObject _customerPrefab;
    [SerializeField]
    private float _delayBetweenCustomers = 2f;
    [SerializeField]
    private float maxQueueSize = 5;
    [SerializeField]
    private Transform _aiSpawnPosition;
    [SerializeField]
    private Transform _roomDestination;
    [SerializeField]
    private GameObject _moneyPrefab;
    [SerializeField]
    private Transform _moneyTransform;

    [SerializeField]
    private RoomData[] _roomData;

    private Queue<GameObject> _customerQueue = new Queue<GameObject>();
    private bool isAttending;
    internal bool isRoomFull;

    private void Start()
    {
        InvokeRepeating(nameof(AddCustomerToTheQueue), 0f, _delayBetweenCustomers);
    }

    private void AddCustomerToTheQueue()
    {
        if (_customerQueue.Count < maxQueueSize)
        {
            // spawn new AI element
            GameObject customer = Instantiate(_customerPrefab, _aiSpawnPosition.position, Quaternion.identity);
            _customerQueue.Enqueue(customer);
            MoveCharacterToQueue(customer);
        }
        else
        {
            Debug.Log("Queue is full");
        }
    }

    private void MoveCharacterToQueue(GameObject character)
    {
        NavMeshAgent navMeshAgent = character.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null && _queuePositions.Length > 0)
        {
            // this ensure that if the queue is empty. then customer moves to first spot and so on
            int targetIndex = _customerQueue.Count == 0 ? 0 : Mathf.Min(_customerQueue.Count - 1, _queuePositions.Length - 1);
            navMeshAgent.SetDestination(_queuePositions[targetIndex].position);
        }
    }

    public void ServeCustomer()
    {
        if (!isAttending && _customerQueue.Count > 0 && HasAvailableRoom())
        {
            isAttending = true;
            GameObject nextCustomer = _customerQueue.Dequeue();
            // move characters in the queue
            Debug.Log("Attending customer...");
            MoveCharacterToRoom(nextCustomer);
            // perform navmesh task
            isAttending = false;
            MoveCharactersInTheQueue();

            if (_customerQueue.Count < maxQueueSize)
            {
                Invoke(nameof(AddCustomerToTheQueue), 0.5f);
            }
        }
    }

    private void MoveCharacterToRoom(GameObject character)
    {
        // drop money
        DropMoney();
        NavMeshAgent navMeshAgent = character.GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(_roomDestination.position);
    }

    private void DropMoney()
    {
        RaycastHit hit;
        if (Physics.Raycast(_moneyTransform.position, Vector3.down, out hit))
        {
            float heightOffset = 0f;
            if (hit.collider.gameObject != _moneyTransform.gameObject)
            {
                heightOffset = 1.5f * hit.collider.transform.childCount; // Adjust height based on the number of children
            }
            Vector3 stackPosition = hit.point + new Vector3(0f, heightOffset, 0f);
            GameObject money = Instantiate(_moneyPrefab, stackPosition, Quaternion.identity);
            if(hit.collider.gameObject.CompareTag("Money"))
            {
                money.transform.parent = hit.collider.gameObject.transform; // Set the new cube as a child of the hit object

            }
        }
        else
        {
            Instantiate(_moneyPrefab, _moneyTransform.position, Quaternion.identity);
        }
    }

    private void MoveCharactersInTheQueue()
    {
        for (int i = 0; i < _customerQueue.Count; i++)
        {
            GameObject character = _customerQueue.ElementAt(i);
            NavMeshAgent navMeshAgent = character.GetComponent<NavMeshAgent>();

            if (_queuePositions[i] != null && i < _queuePositions.Length)
            {
                navMeshAgent.SetDestination(_queuePositions[i].position);
            }
        }
    }

    private bool HasAvailableRoom()
    {
        foreach (var room in _roomData)
        {
            if(!room.isOccupied && !room.isLocked)
            {
                return true;
            }
        }
        Debug.Log("Room full");
        return false;
    }
}
