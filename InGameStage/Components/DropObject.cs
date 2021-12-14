using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogGame.Game
{
    public class DropObject : MonoBehaviour
    {
        public delegate void OnEnded(DropObject dropObject);
        public OnEnded onEnded;

        public delegate void OnClickEvent();
        public OnClickEvent onClickEvent;

        public delegate void OnPositionX(float positionX);
        public OnPositionX onPositionX;

        // 오브젝트가 떨어지는 속력
        public float dropSpeed;



        // 캐릭터와 겹쳤을 때, 배고픔 게이지(체력)에 더해줄 값
        //  -> 쓰레기라면 감소(음수), 물고기라면 증가(양수)
        public float changeHungryValue;


        void FixedUpdate()
        {
            FallDown();
        }

    

        /// <summary>
        /// 오브젝트의 떨어짐 이동을 구현
        /// </summary>
        private void FallDown()
        {
            transform.Translate(Vector3.down * dropSpeed * Time.deltaTime, Space.World);
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Pause()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 특정 오브젝트와 겹쳤을 경우 호출되는 콜백 
        /// </summary>
        /// <param name="collision">충돌 또는 겹친 콜라이더의 정보</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Collider 컴포넌트를 갖는 객체는 유니티에서 제공하는 콜백을
            // 사용할 수 있다.
            // 충돌 감지 -> OnCollision ~
            // 겹침 감지 -> OnTrigger ~

            // Enter -> 충돌/겹치는 순간 1번 호출
            // Stay -> 충돌/겹침이 발생하고 있다면 계속해서 호출
            // Exit -> 충돌/겹침 상태에서 벗어나는 순간 1번 호출

            // 콜백이 작동하려면 피충돌체 중에 하나는 무조건 강체(Rigidbody)를
            // 가지고 있어야 합니다

            // Player와 겹쳤는지 확인  
            

            if (collision.CompareTag("Player"))
            {
                var mCommom = Type.Commom.Player;
                onClickEvent();


            }

            else if(collision.CompareTag("Floor"))
            {

                onPositionX(gameObject.transform.position.x);
               
                
            }

            onEnded(this);
        }

    }
}
    