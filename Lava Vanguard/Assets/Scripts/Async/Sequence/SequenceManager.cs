using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Async
{
    public class SequenceManager : MonoBehaviour
    {
        public static SequenceManager Instance { get; private set; }
        public Transform sequenceContainer;
        public Transform draggingTransform;
        public GameObject sequencePrefab;
        public SequenceView MainSequence { get => sequenceViews["Sequence_Main"]; }
        public Dictionary<string, SequenceView> sequenceViews = new Dictionary<string, SequenceView>();
        public Dictionary<string, SequenceData> savedData = new Dictionary<string, SequenceData>();
        
        private void Awake()
        {
            Instance = this;
        }
        public void Init()
        {
            InitSequence(Vector2.zero, "Sequence_Main");
            UpdateCustomSequence();
        }
        public void InitSequence(Vector2 localAnchorPosition, string sequenceID)
        {
            var sequenceData = GameDataManager.SequenceData[sequenceID];
            var sequenceView = Instantiate(sequencePrefab, sequenceContainer).GetComponent<SequenceView>();
            sequenceView.Init(localAnchorPosition, sequenceData);
            sequenceViews.Add(sequenceID, sequenceView);
        }
        public void GenerateAsyncSequence(Vector2 localAnchorPosition, string sequenceID, int count)
        {
            var sequenceData = new SequenceData();
            sequenceData.ID = sequenceID;
            sequenceData.CardDatas = new List<CardRankData>();
            sequenceData.CardDatas.Add(CardRankData.AsyncHead);
            for (int i = 1; i < count; i++)
            {
                sequenceData.CardDatas.Add(CardRankData.Empty);
            }
            var sequenceView = Instantiate(sequencePrefab, sequenceContainer).GetComponent<SequenceView>();
            sequenceView.Init(localAnchorPosition, sequenceData);
            sequenceViews.Add(sequenceID, sequenceView);
        }
        public void SaveSequenceData(string sequenceID)
        {
            var sequenceData = new SequenceData();
            sequenceData.ID = sequenceID;
            sequenceData.CardDatas = new List<CardRankData>();

            var sequenceView = sequenceViews[sequenceID];
            for (int i = 0; i < sequenceView.slots.Count; i++)
            {
                CardRankData cardRankData = new CardRankData();
                if (sequenceView.slots[i].content == null)
                {
                    cardRankData.ID = i.ToString();
                    cardRankData.CardID = "Card_Empty";
                }
                else
                {

                    var cardView = sequenceView.slots[i].content;
                    cardRankData = cardView.cardRankData;
                    if (cardView.cardRankData.LinkedSequenceID != null)
                    {
                        //var linkedSequenceID = "Sequence_Async_" + ++asyncID;
                        SaveSequenceData(cardRankData.LinkedSequenceID);
                        //cardRankData.LinkedSequenceID = linkedSequenceID;
                    }

                    cardRankData.ID = i.ToString();

                }
                sequenceData.CardDatas.Add(cardRankData);
            }
            savedData.Add(sequenceID, sequenceData);

        }
        public void Rebuild()
        {
            SaveSequenceData("Sequence_Main");
            GameDataManager.SequenceData = savedData;

            for (int i = sequenceContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(sequenceContainer.GetChild(i).gameObject);
            }
            sequenceViews.Clear();
            Init();
            savedData.Clear();
        }
        public string GetNextLinkedSequenceID()
        {
            string link = "Sequence_Async_";
            int cnt = 1;
            while (true)
            {
                var res = link + cnt;
                if (sequenceViews.ContainsKey(res))
                    cnt++;
                else
                    return res;
            }
        }
        public void UpdateCustomSequence()
        {
            if (BulletManager.Instance == null)
            {
                Debug.LogWarning("No bullet manager detected! You could still config your weapon, but there is no bullet!");
                return;
            }

            StopAllCoroutines(); 
            StartCoroutine(RunSequence());
        }

        private IEnumerator RunSequence()
        {
            List<(int, SequenceView)> threads = new List<(int, SequenceView)>();
            threads.Add((0, MainSequence));

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j >= threads.Count)
                    {
                        yield return new WaitForSeconds(0.05f);
                    }
                    else
                    {
                        if (i - threads[j].Item1 >= threads[j].Item2.slots.Count)
                        {
                            threads.RemoveAt(j);
                            continue;
                        }
                        if (threads[j].Item2.slots[i - threads[j].Item1].content == null)
                        {
                            yield return new WaitForSeconds(0.05f);
                            continue;
                        }
                        CardRankData data = threads[j].Item2.slots[i - threads[j].Item1].content.cardRankData;
                        BulletManager.Instance.GenerateBullet(data);
                        yield return new WaitForSeconds(0.05f);
                        if (data.LinkedSequenceID != null)
                        {
                            threads.Add((i, sequenceViews[data.LinkedSequenceID]));
                        }
                    }
                }
            }

            StartCoroutine(RunSequence()); 
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}