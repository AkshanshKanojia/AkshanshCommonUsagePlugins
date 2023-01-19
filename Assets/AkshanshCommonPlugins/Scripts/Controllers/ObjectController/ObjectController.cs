using System.Collections.Generic;
using UnityEngine;

namespace AkshanshKanojia.Controllers.ObjectManager
{
    public class ObjectController : MonoBehaviour
    {
        [System.Serializable]
        public class ActionHolder
        {
            public GameObject TargetObject;
            public Vector3 targetLocation;
            public Quaternion targetRotation;
            public float MovementSpeed;
            public bool ReachedPos = true, ReachedRot = true,IsLocal;
            public ActionHolder(GameObject _obj,Vector3 _targetPos,float _speed,bool _isLocal)
            {
                TargetObject = _obj;
                targetLocation = _targetPos;
                MovementSpeed = _speed;
                ReachedPos = false;
                IsLocal = _isLocal;
            }
            public ActionHolder(GameObject _obj, Quaternion _targetRot, float _speed, bool _isLocal)
            {
                TargetObject = _obj;
                targetRotation = _targetRot;
                MovementSpeed = _speed;
                ReachedRot = false;
                IsLocal = _isLocal;
            }
            public ActionHolder(GameObject _obj, Vector3 _targetPos,Quaternion _targetRot, float _speed, bool _isLocal)
            {
                TargetObject = _obj;
                targetLocation = _targetPos;
                targetRotation = _targetRot;
                MovementSpeed = _speed;
                IsLocal = _isLocal;
                ReachedPos = false;
                ReachedRot = false;
            }
        }

        public List<ActionHolder> ActiveEvents;

        //events
        public delegate void RotationFinished(GameObject _obj);
        public delegate void MovementFinished(GameObject _obj);
        public event RotationFinished OnRotationEnd;
        public event MovementFinished OnMovementEnd;

        private void FixedUpdate()
        {
            ActionManager();
        }

        void ActionManager()
        {
            foreach(var tempItem in ActiveEvents)
            {
                if (tempItem == null)
                    return;
                if(!tempItem.ReachedPos)
                {
                    //update pos
                    Vector3 _tempDir = tempItem.targetLocation - tempItem.TargetObject.transform.position;
                    if (tempItem.IsLocal)
                    {
                        _tempDir = tempItem.targetLocation - tempItem.TargetObject.transform.localPosition;
                        tempItem.TargetObject.transform.localPosition += tempItem.MovementSpeed * Time.deltaTime * _tempDir.normalized;
                    }
                    else
                    {
                        tempItem.TargetObject.transform.position += tempItem.MovementSpeed * Time.deltaTime * _tempDir.normalized;
                    }
                    if(_tempDir.magnitude<0.2f)
                    {
                        tempItem.ReachedPos = true;
                        OnMovementEnd?.Invoke(tempItem.TargetObject);
                    }
                }
                if(!tempItem.ReachedRot)
                {
                    if (tempItem.IsLocal)
                    {
                        tempItem.TargetObject.transform.localRotation = Quaternion.Slerp(tempItem.TargetObject.transform.rotation,
                            tempItem.targetRotation, Time.deltaTime * tempItem.MovementSpeed);
                    }
                    else
                    {
                        tempItem.TargetObject.transform.rotation = Quaternion.Slerp(tempItem.TargetObject.transform.rotation,
                            tempItem.targetRotation, Time.deltaTime * tempItem.MovementSpeed);
                    }
                    if(Quaternion.Angle(tempItem.TargetObject.transform.rotation,
                        tempItem.targetRotation)<0.2f)
                    {
                        tempItem.ReachedRot = true;
                        OnRotationEnd?.Invoke(tempItem.TargetObject);
                    }
                    //update rot
                }
                if(tempItem.ReachedRot&&tempItem.ReachedPos)
                {
                    RemoveUsedObejct(tempItem);
                    break;
                }
            }
        }

        private void RemoveUsedObejct(ActionHolder tempItem)
        {
            for (int i = 0; i < ActiveEvents.Count; i++)
            {
                if (tempItem == ActiveEvents[i])
                {
                    ActiveEvents.RemoveAt(i);
                }
            }
        }
    } 
}
