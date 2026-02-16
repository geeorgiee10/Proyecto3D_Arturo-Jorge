using UnityEngine;
using System.Collections; 

public class BattleCameraController : MonoBehaviour 
{ 
    public float moveDuration = 0.6f; 
    public AnimationCurve ease = AnimationCurve.EaseInOut(0,0,1,1); 
    private Coroutine moveRoutine; 
    
    public void MoveTo(Transform moveTarget, Transform lookTarget = null, Vector3? lookPoint = null)
    { 
        if (moveRoutine != null) 
            StopCoroutine(moveRoutine); 
        
        moveRoutine = StartCoroutine(MoveRoutine(moveTarget, lookTarget, lookPoint)); 
    } 
    
    IEnumerator MoveRoutine(Transform moveTarget, Transform lookTarget, Vector3? lookPoint)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 endPos = moveTarget.position;
        Vector3 finalLookPoint; 
        
        if (lookTarget != null) 
            finalLookPoint = lookTarget.position; 
        else if (lookPoint.HasValue)
            finalLookPoint = lookPoint.Value;
        else finalLookPoint = endPos + moveTarget.forward;
        
        Quaternion endRot = Quaternion.LookRotation(finalLookPoint - endPos);
        float t = 0;
        
        while (t < 1)
        { 
            t += Time.deltaTime / moveDuration;
            float eval = ease.Evaluate(t);
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, eval);
            transform.position = currentPos;
            Quaternion currentRot = Quaternion.Slerp(startRot, endRot, eval);
            transform.rotation = currentRot;
            yield return null;
        } 
        
        transform.position = endPos;
        transform.rotation = endRot; 
    } 
}