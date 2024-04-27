using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    [SerializeField]
    private Text _stateText;
    [SerializeField]
    private GameObject _cleanButton;
    [SerializeField]
    private Transform _outPos;

    [SerializeField]
    private CameraTransition _transition;

    internal bool isOccupied;
    public bool isLocked;

    private void Start()
    {
        if (isLocked)
        {
            _stateText.text = "Locked";
        }
        else
            _stateText.text = "Ready";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Customer"))
        {
            if (!isOccupied)
            {
                ShowOccupiedText();
                isOccupied = true;
                StartCoroutine(SleepAndLeave(other.gameObject.GetComponent<NavMeshAgent>()));
            }
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            _transition.TransitionToCamera2();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Customer"))
        {
            if (isOccupied)
            {
                ShowCleanText();
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            _transition.TransitionToCamera1();
        }
    }

    private IEnumerator SleepAndLeave(NavMeshAgent navMeshAgent)
    {
        yield return new WaitForSeconds(1f);
        navMeshAgent.SetDestination(_outPos.position);
        Destroy(navMeshAgent.gameObject, 4f);
    }

    void ShowOccupiedText()
    {
        _stateText.text = "Sleeping";
        _cleanButton.SetActive(false);
    }

    void ShowCleanText()
    {
        _cleanButton.SetActive(true);
        _stateText.text = "Clean";

    }

    public void CleanRoom()
    {
        _cleanButton.SetActive(false);
        _stateText.text = "Ready";
        isOccupied = false;
    }

    public void UnlockRoom()
    {
        if(isLocked)
        {
            isLocked = false;
            _stateText.text = "Ready";
        }
    }

}
