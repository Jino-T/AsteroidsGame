using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrap : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // transform to viewpoint so it is in 0 - 1 range regardless of screen aspect ratio
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
 
        // if moved out of viewpoint then wrap to opposite side
        Vector3 moveAdjustment = Vector3.zero;
        if (viewportPosition.x < 0) {
             // flip from one side to another
             moveAdjustment.x += 1;
        } else if (viewportPosition.x > 1)
             moveAdjustment.x -= 1;
        // same math but now for y
        else if (viewportPosition.y < 0) {
             // flip from one side to another
             moveAdjustment.y += 1;
        }
        else if (viewportPosition.y > 1) {
             // flip from one side to another
             moveAdjustment.y -= 1;
        }

        // convert back into world coordinates
        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAdjustment);
    }
}
