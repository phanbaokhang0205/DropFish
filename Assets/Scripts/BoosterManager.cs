using UnityEngine;
using UnityEngine.EventSystems;
public class BoosterManager : MonoBehaviour
{
    private Vector3 touchPosition;
    private Touch touch;
    void Start()
    {
    }
    
    /**
     * nếu touchPosition = [Hammer, Bomb, Shake].transform.position
     * khi moved -> update booster.position = touchPossition
     * khi ended -> booster.position = initPosition
     */
    void Update()
    {
        if (Input.touchCount > 0 && GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));

            if (touch.phase == TouchPhase.Began)
            {

            }
            else if (touch.phase == TouchPhase.Moved)
            {
            }
            else if (touch.phase == TouchPhase.Ended)
            {
            }
        }

    }


}
