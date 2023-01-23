using UnityEngine;
using System.Collections.Generic;
using AkshanshKanojia.Controllers.ObjectManager;

namespace AkshanshKanojia.Animations
{
    public class TransformSequencer : MonoBehaviour
    {
        //events
        public delegate void SequenceFinished(int _sequenceIndex);
        public event SequenceFinished OnSequenceEnd;
        public delegate void SequenceListFinished();
        public event SequenceListFinished OnSequenceListEnd;

        [System.Serializable]
        public class SequenceDataHolder
        {
            public GameObject TargetObject;
            public Vector3 TargetPos,TargetRot;
            [Tooltip("Use this to get id to identify if specific event is finished.")]
            public int SequenceID = 0;
            public float SequenceStartDelay = 0,TrackSpeed = 3f;
            public bool MoveOnLocalAxis = false;
        }
        [SerializeField] SequenceDataHolder[] SequenceInArray;
        [SerializeField] bool playOnAwake = true;
        
        SequenceDataHolder tempTargetSequence;
        Queue<SequenceDataHolder> CurrentSequences;



        ObjectController objCont;
        bool isActive = true,posReached,rotReached;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            objCont = FindObjectOfType<ObjectController>();
            if (!objCont)
            {
                Debug.LogWarning("Can not find object controller, transform sequencer will not work!");
                isActive = false;
            }
            if (isActive)
            {
                CurrentSequences = new Queue<SequenceDataHolder>();
                for (int i = 0; i < SequenceInArray.Length; i++)
                {
                    CurrentSequences.Enqueue(SequenceInArray[i]);
                }
                if (CurrentSequences.Count != 0)
                {
                    objCont.OnMovementEnd += OnPosReached;
                    objCont.OnRotationEnd += OnRotReached;
                }
                if (playOnAwake)
                {
                    UpdateCurrentSequence();
                }
            }
        }

        void OnPosReached(GameObject _go)
        {
            if (_go != tempTargetSequence.TargetObject)
                return;
            posReached = true;
            CheckSequenceStatus();
        }
        void OnRotReached(GameObject _go)
        {
            if (_go != tempTargetSequence.TargetObject)
                return;
            rotReached = true;
            CheckSequenceStatus();
        }

        void CheckSequenceStatus()
        {
            if(posReached&&rotReached)
            {
                OnSequenceEnd?.Invoke(tempTargetSequence.SequenceID);
                UpdateCurrentSequence();
            }
        }
        void UpdateCurrentSequence()
        {
            if(CurrentSequences.Count<=0)
            {
                isActive = false;
                objCont.OnMovementEnd -= OnPosReached;
                objCont.OnRotationEnd -= OnRotReached;
                OnSequenceListEnd?.Invoke();
                return;
            }
            tempTargetSequence = CurrentSequences.Dequeue();
            posReached = false;
            rotReached = false;
            StartCoroutine(StartSequenceQueue(tempTargetSequence.SequenceStartDelay));
        }
        IEnumerator<WaitForSeconds> StartSequenceQueue(float _tempWaitDuration)
        {
            yield return new WaitForSeconds(_tempWaitDuration);
            objCont.AddEvent(tempTargetSequence.TargetObject, tempTargetSequence.TargetPos,Quaternion.Euler(tempTargetSequence.TargetRot),
                tempTargetSequence.TrackSpeed, tempTargetSequence.MoveOnLocalAxis);
        }
    }
}
