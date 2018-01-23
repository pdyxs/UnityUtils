using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformUtils {

    public static bool Overlaps(this RectTransform rt1, RectTransform rt2, Canvas c = null) {
        float scale = c == null ? 1 : c.scaleFactor;
        Rect r1 = new Rect(rt1.position.x - rt1.rect.width * scale / 2, 
                           rt1.position.y - rt1.rect.height * scale, 
                           rt1.rect.width * scale, rt1.rect.height * scale);
        Rect r2 = new Rect(rt2.position.x - rt2.rect.width * scale / 2, 
                           rt2.position.y - rt2.rect.height * scale, 
                           rt2.rect.width * scale, rt2.rect.height * scale);

        return r1.Overlaps(r2);
    }
}
