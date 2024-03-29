﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ButtonAnimation : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Transform buttonTransform;
        [SerializeField] private float animationDuration = 1f;

        private void Awake()
        {
            button.interactable = false;
            buttonTransform.localScale = Vector3.zero;
        }

        public IEnumerator Animate()
        {
            yield return ScaleAnimation();
        }

        private IEnumerator ScaleAnimation()
        {
            for (float i = 0; i < 1; i += Time.deltaTime / animationDuration)
            {
                buttonTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, EaseOutBounce(i));
                yield return null;
            }
            
            buttonTransform.localScale = Vector3.one;
            button.interactable = true;
        }

        private float EaseOutBounce(float x)
        {
            float n1 = 7.5625f;
            float d1 = 2.75f;
            if (x < 1 / d1) return n1 * x * x;
            if (x < 2 / d1) return n1 * (x -= 1.5f / d1) * x + 0.75f;
            if (x < 2.5 / d1) return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }
}