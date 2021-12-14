using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogGame.Game
{
    public class PuddleObject : MonoBehaviour
    {
        public delegate void OnEnded(PuddleObject puddleObject);
        public OnEnded onEnded;


        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Pause()
        {
            gameObject.SetActive(false);
        }
        public void TryChangeColor()
        {
            StartCoroutine(ChangeColor());
        }

        /// <summary>
        /// 웅덩이 RGB값 변경
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private IEnumerator ChangeColor()
        {
            var sr = gameObject.GetComponent<SpriteRenderer>();

            for (int i = 30; i >= 0; i--)
            {
                float f = i / 30.0f;
                Color c = sr.material.color;
                c.a = f;
                sr.material.color = c;
                yield return new WaitForSeconds(0.1f);
            }
            onEnded(this);

            //var objColor = gameObject.GetComponent<SpriteRenderer>().color;
            //var from = new Color(255f, 255f, 255f, 255f);
            //var to = new Color(0f, 0f, 0f, 0f);

            //var remainTime = 3f;
            //while (true)
            //{
            //    yield return null;

            //    objColor = Color.Lerp(from, to, remainTime);
            //    remainTime -= Time.deltaTime;

            //    if (0f > remainTime)
            //    {
            //        onEnded(this);
            //        break;
            //    }

            //}

        }

    }
}
