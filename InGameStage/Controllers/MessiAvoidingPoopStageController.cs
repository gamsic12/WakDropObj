using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace DogGame.Game
{
    class MessiAvoidingPoopStageController : DogGame.Core.InGame.Controller
    {
        public override int ID
        {
            get { return (int)DogGame.Type.Game.MessiAvoidingPoop; }
        }

        protected const int MAX_LIFE = 3;

        [SerializeField] protected GameObject spawner;
        [SerializeField] protected GameObject spawnObjectImage;
        [SerializeField] protected GameObject poopPrefabs;
        [SerializeField] protected DropObject[] dropObjects;
        protected PuddleObject[] puddleObjs;

        protected List<PuddleObject> pool = new List<PuddleObject>();
        protected List<DropObject> pool2 = new List<DropObject>();


        // 메시똥피하기게임 시간제한
        [SerializeField] protected float limitTime;

        // 드롭오브젝트 스폰 시간 
        [SerializeField] protected float spawnCheckTime;
        // 마지막으로 스폰한 시간
        private float lastSpawnTime;


        // 이동영역을 나타내는 트랜스폼 객체의 참조들
        [SerializeField] protected Transform maxLeftPos, maxRightPos;

        [SerializeField] protected AudioSource audioSource;
        [SerializeField] protected AudioClip[] clips;

        private void Awake()
        {
            for (int i = 0, ii = dropObjects.Length; ii > i; ++i)
            {
                dropObjects[i].onEnded = PushDropObject;
                dropObjects[i].onClickEvent = ClashWithPlayer;
                dropObjects[i].onPositionX = ClashWithFloor;
                dropObjects[i].Pause();
                pool2.Add(dropObjects[i]);
            }

            puddleObjs = GetComponentsInChildren<PuddleObject>();
            for (int i = 0; i < puddleObjs.Length; i++)
            {
                puddleObjs[i].onEnded = Push;
                puddleObjs[i].Pause();
                pool.Add(puddleObjs[i]);
            }

        }
        protected override IEnumerator OnStart()
        {
            yield return null;
        }

        protected override IEnumerator OnLoop()
        {
            while (true)
            {
                yield return null;

                // 스폰할 수 있는 시간이 되면 똥 오브젝트를 스폰해줌
                limitTime -= Time.deltaTime;
                if (Time.time - lastSpawnTime > spawnCheckTime)
                {
                    // 적을 새로 생성했으므로 마지막 생성 시간 갱신
                    lastSpawnTime = Time.time;

                    RandomSpawn();
                }


                // 시간제한 후 다음 스테이지로 넘어감
                if (0 >= limitTime)
                    break;
            }
        }

        protected override IEnumerator OnEnd()
        {
            yield return null;
        }

        protected override void GameEnd()
        {
            base.GameEnd();

            Game.Event.Page evt = Core.Event.Getter.Get<Game.Event.Page>();
            evt.PageID = Game.Page.Type.MainMenu;
            evt.PageState = Game.Page.State.OnChange;

            Core.Event.Dispatcher.Dispatch(evt);
        }


        private void RandomSpawn()
        {
            // 오브젝투풀에서 드롭 오브젝트를 가져온다
            var spawnObj = PopDropObject();
            spawnObj.Show();

            spawnObj.transform.position = spawner.transform.position;

            // 떨어뜨릴 때 초기위치를 설정  
            // 처음에 스포너의 위치를 넣음 (y값 위치는 스포너를 기준으로 고정된 y값을 사용)
            var dropPos = spawnObj.transform.position;

            
            // x값만 랜덤하게 변경
            dropPos.x = Random.Range(maxLeftPos.position.x, maxRightPos.position.x);
            //dropPos.x = Random.Range(-50, 50);

            spawnObjectImage.transform.position = dropPos;

            // 설정한 위치를 드롭오브젝트에 적용
            spawnObj.transform.position = dropPos;
           

        }

        /// <summary>
        /// 드롭오브젝트가 플레이어한테 맞았을 때
        /// </summary>
        private void ClashWithPlayer()
        {

            // 플레이어의 이미지 바뀌게하기
        }


        /// <summary>
        /// 드롭오브젝트가 땅에 떨어졌을 때
        /// </summary>
        /// <param name="positionX"></param>
        private void ClashWithFloor(float positionX)
        {
            // 드롭오브젝트의 position.x 를 받아 그 좌표에 웅덩이 보이게하기
            var obj = Pop();
            obj.Show();

            var pos = transform.position;
            pos.x = positionX;
            pos.y = Random.Range(-28.8f, -30.74f);

            obj.transform.position = pos;
           // obj.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f);

            obj.TryChangeColor();
           
        }

        protected DropObject PopDropObject()
        {
            if (pool2.Count > 0)
            {
                DropObject temp = pool2[0];
                pool2.RemoveAt(0);

                return temp;
            }

            return null;
        }
        protected void PushDropObject(DropObject dropObj)
        {
            pool2.Add(dropObj);
            dropObj.Pause();
        }


        protected PuddleObject Pop()
        {
            if (pool.Count > 0)
            {
                PuddleObject temp = pool[0];
                pool.RemoveAt(0);

                return temp;
            }

            return null;
        }
        protected void Push(PuddleObject puddle)
        {
            pool.Add(puddle);
            puddle.Pause();
        }
    }

    
}
