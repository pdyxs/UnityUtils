using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TextUtils {

    public static int effectiveSize(this Text t) {
        if (!t.resizeTextForBestFit) {
            return t.fontSize;
        }

		t.cachedTextGenerator.Invalidate();
		Vector2 size = (t.transform as RectTransform).rect.size;
		TextGenerationSettings tempSettings = t.GetGenerationSettings(size);
		tempSettings.scaleFactor = 1;//dont know why but if I dont set it to 1 it returns a font that is to small.
		if (!t.cachedTextGenerator.Populate(t.text, tempSettings))
			Debug.LogError("Failed to generate fit size");
		return t.cachedTextGenerator.fontSizeUsedForBestFit;
    }
}
