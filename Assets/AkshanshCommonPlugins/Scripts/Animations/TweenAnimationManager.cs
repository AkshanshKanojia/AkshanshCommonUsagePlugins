using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TweenAnimationManager : MonoBehaviour
{
    public enum TweenAnimationTypes
    {
        Position, Rotation, Scale, Fade, AnchorPos, ChangeImageColor,
        ChangeSpriteColor
    }

    private enum SequenceTransitionType { PlayAllAtAwake, PlaySequential, PlayParentFirst }
    /// <summary>
    /// Holds the data about type of tween animation to play
    /// </summary>
    [System.Serializable]
    internal struct TweenDataHolder
    {
        [SerializeField] internal TweenAnimationTypes type;
        [SerializeField] internal float startDelay;
        [SerializeField] internal float tweenTime;
        [SerializeField] internal float targetValue;
        [SerializeField] internal Vector3 targetVectorValue;
        [SerializeField] internal Color targetColorValue;
        [SerializeField] internal Ease easeType;
        [SerializeField] internal int loopCount;
        [SerializeField] internal LoopType loopType;
        [SerializeField] internal bool setUpdateType;
        [SerializeField] internal bool useLocal;
    }

    [SerializeField] internal Transform _targetTransform;
    [SerializeField] private bool _playOnAwake;
    [SerializeField] internal RectTransform targetRectTrans;
    [SerializeField] internal SpriteRenderer targetSpriteRenderer;
    [SerializeField] internal Image targetImage;
    [SerializeField] internal CanvasGroup targetCanvasGroup;
    [SerializeField] private SequenceTransitionType _sequenceTransistionType;
    [SerializeField] private TweenAnimationManager[] _sequencesToPlay;
    [SerializeField] internal List<TweenDataHolder> tweenSequenceList;

    [HideInInspector] public TweenAnimationTypes keyTweenType;
    private Tween _activeTween;
    private int _tweenIndex, _currentSequenceIndex = -1;
    private TweenAnimationManager _activeAnimationManager;

    internal event Action<int> OnSequenceComplete;
    internal event Action OnAllSequencesFinished;

    private void Awake()
    {
        AssignRefrences();
    }
    private void Start()
    {
        CheckSequenceTransistion();
    }

    internal void AssignRefrences()
    {
        _targetTransform = _targetTransform != null ? _targetTransform : transform;
        targetRectTrans = targetRectTrans ? targetRectTrans :
            _targetTransform.GetComponent<RectTransform>();
        targetImage = targetImage ? targetImage : _targetTransform.GetComponent<Image>();
        targetCanvasGroup = targetCanvasGroup ? targetCanvasGroup :
            _targetTransform.GetComponent<CanvasGroup>();
        targetSpriteRenderer = targetSpriteRenderer ? targetSpriteRenderer :
            _targetTransform.GetComponent<SpriteRenderer>();
    }

    private void CheckSequenceTransistion()
    {
        if (_playOnAwake)
        {
            PlaySequence();
            if (_sequenceTransistionType == SequenceTransitionType.PlayAllAtAwake)
            {
                foreach (var _animations in _sequencesToPlay)
                {
                    _animations.PlaySequence();
                }
            }
        }
        OnAllSequencesFinished += () =>
        {
            if (_sequenceTransistionType == SequenceTransitionType.PlaySequential)
            {
                CheckSequenceStatus();
            }
            else if (_sequenceTransistionType == SequenceTransitionType.PlayParentFirst)
            {
                foreach (var _animations in _sequencesToPlay)
                {
                    _animations.PlaySequence();
                }
            }
        };
    }

    private void CheckSequenceStatus()
    {
        _currentSequenceIndex++;
        if (_currentSequenceIndex < _sequencesToPlay.Length)
        {
            _activeAnimationManager = _sequencesToPlay[_currentSequenceIndex];
            _activeAnimationManager.OnAllSequencesFinished += CheckSequenceStatus;
            _activeAnimationManager.PlaySequence();
        }
    }

    internal void PlaySequence()
    {
        //default overload restarts the sequnce 
        _activeTween?.Kill();// checks if active twen is present if so stop the current tween.
        _activeTween = null;
        _tweenIndex = 0;//restart the tweens

        BeginTweenSequenceCheck();
    }

    private void BeginTweenSequenceCheck()
    {
        if (_tweenIndex >= tweenSequenceList.Count)
        {
            OnAllSequencesFinished?.Invoke();
            return;
        }

        var tempTweenTarget = tweenSequenceList[_tweenIndex];
        switch (tempTweenTarget.type)
        {
            case TweenAnimationTypes.Position:
                if (!tempTweenTarget.useLocal)
                {
                    _activeTween = _targetTransform.DOMove(tempTweenTarget.targetVectorValue, tempTweenTarget.tweenTime).
                         SetDelay(tempTweenTarget.startDelay).SetEase(tempTweenTarget.easeType).
                         SetLoops(tempTweenTarget.loopCount, tempTweenTarget.loopType).
                         SetUpdate(tempTweenTarget.setUpdateType);
                }
                else
                {
                    _activeTween = _targetTransform.DOLocalMove(tempTweenTarget.targetVectorValue, tempTweenTarget.tweenTime).
                         SetDelay(tempTweenTarget.startDelay).SetEase(tempTweenTarget.easeType).
                         SetLoops(tempTweenTarget.loopCount, tempTweenTarget.loopType).
                         SetUpdate(tempTweenTarget.setUpdateType);
                }
                break;
            case TweenAnimationTypes.Rotation:
                if (!tempTweenTarget.useLocal)
                {
                    _activeTween = _targetTransform.DORotate(tempTweenTarget.targetVectorValue, tempTweenTarget.tweenTime).
                        SetDelay(tempTweenTarget.startDelay).SetEase(tempTweenTarget.easeType).
                        SetLoops(tempTweenTarget.loopCount, tempTweenTarget.loopType).
                        SetUpdate(tempTweenTarget.setUpdateType);
                }
                else
                {
                    _activeTween = _targetTransform.DOLocalRotate(tempTweenTarget.targetVectorValue, tempTweenTarget.tweenTime).
                        SetDelay(tempTweenTarget.startDelay).SetEase(tempTweenTarget.easeType).
                        SetLoops(tempTweenTarget.loopCount, tempTweenTarget.loopType).
                        SetUpdate(tempTweenTarget.setUpdateType);
                }
                break;
            case TweenAnimationTypes.Scale:
                _activeTween = _targetTransform.DOScale(tempTweenTarget.targetVectorValue, tempTweenTarget.tweenTime).
                    SetDelay(tempTweenTarget.startDelay).SetEase(tempTweenTarget.easeType).
                    SetLoops(tempTweenTarget.loopCount, tempTweenTarget.loopType).
                    SetUpdate(tempTweenTarget.setUpdateType);
                break;
            case TweenAnimationTypes.AnchorPos:
                _activeTween = targetRectTrans.DOAnchorPos(tempTweenTarget.targetVectorValue, tempTweenTarget.tweenTime).
                    SetDelay(tempTweenTarget.startDelay).SetEase(tempTweenTarget.easeType).
                    SetLoops(tempTweenTarget.loopCount, tempTweenTarget.loopType).
                    SetUpdate(tempTweenTarget.setUpdateType);
                break;
            case TweenAnimationTypes.Fade:
                _activeTween = targetCanvasGroup.DOFade(tempTweenTarget.targetValue, tempTweenTarget.tweenTime).
                    SetDelay(tempTweenTarget.startDelay).SetEase(tempTweenTarget.easeType).
                    SetLoops(tempTweenTarget.loopCount, tempTweenTarget.loopType).
                    SetUpdate(tempTweenTarget.setUpdateType);
                break;
            case TweenAnimationTypes.ChangeImageColor:
                _activeTween = targetImage.DOColor(tempTweenTarget.targetColorValue, tempTweenTarget.tweenTime).
                    SetDelay(tempTweenTarget.startDelay).SetEase(tempTweenTarget.easeType).
                    SetLoops(tempTweenTarget.loopCount, tempTweenTarget.loopType).
                    SetUpdate(tempTweenTarget.setUpdateType);
                break;
            case TweenAnimationTypes.ChangeSpriteColor:
                _activeTween = targetSpriteRenderer.DOColor(tempTweenTarget.targetColorValue, tempTweenTarget.tweenTime).
                    SetDelay(tempTweenTarget.startDelay).SetEase(tempTweenTarget.easeType).
                    SetLoops(tempTweenTarget.loopCount, tempTweenTarget.loopType).
                    SetUpdate(tempTweenTarget.setUpdateType);
                break;
            default:
                return;
        }

        _activeTween.onComplete += () =>
        {
            OnSequenceComplete?.Invoke(_tweenIndex);
            _tweenIndex++;
            BeginTweenSequenceCheck();
        };
    }

    public void PauseSequence()
    {
        _activeTween.Pause();
    }

    public void ResumeSequence()
    {
        _activeTween.Play();
    }
}

#region Editor Script
#if UNITY_EDITOR
[CustomEditor(typeof(TweenAnimationManager))]
public class TweenAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Box("");
        GUILayout.Label("Animation Buttons");
        var animationMang = (TweenAnimationManager)target;
        animationMang.keyTweenType = (TweenAnimationManager.TweenAnimationTypes)EditorGUILayout.EnumPopup("Key Type", animationMang.keyTweenType);
        if (GUILayout.Button("Add Key"))
        {
            animationMang.tweenSequenceList ??= new List<TweenAnimationManager.TweenDataHolder>();
            animationMang.AssignRefrences();

            var tempTargetVector = animationMang.keyTweenType switch
            {
                TweenAnimationManager.TweenAnimationTypes.Scale => animationMang.transform.localScale,
                TweenAnimationManager.TweenAnimationTypes.AnchorPos => animationMang.targetRectTrans ? animationMang.targetRectTrans.anchoredPosition :
                Vector3.zero,
                TweenAnimationManager.TweenAnimationTypes.Position => animationMang.transform.position,
                TweenAnimationManager.TweenAnimationTypes.Rotation => animationMang.transform.eulerAngles,
                _ => Vector3.zero
            };

            var tempFloatTarget = animationMang.keyTweenType switch
            {
                TweenAnimationManager.TweenAnimationTypes.Fade => animationMang.targetCanvasGroup ? animationMang.targetCanvasGroup.alpha : 0,
                _ => 0
            };

            var tempColorTarget = animationMang.keyTweenType switch
            {
                TweenAnimationManager.TweenAnimationTypes.ChangeImageColor => animationMang.targetImage ? animationMang.targetImage.color : Color.black,
                TweenAnimationManager.TweenAnimationTypes.ChangeSpriteColor => animationMang.targetSpriteRenderer ? animationMang.targetSpriteRenderer.color :
                Color.black,
                _ => Color.black
            };
            animationMang.tweenSequenceList.Add(new TweenAnimationManager.TweenDataHolder
            {
                tweenTime = 1,
                type = animationMang.keyTweenType,
                loopType = LoopType.Restart,
                loopCount = 0,
                easeType = Ease.InOutQuad,
                useLocal = false,
                targetVectorValue = tempTargetVector,
                targetValue = tempFloatTarget,
                targetColorValue = tempColorTarget,
            });
        }
    }
}
#endif
#endregion
