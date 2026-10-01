using NUnit.Framework;
using CardMatching.Core.Events;
using CardMatching.Features.Scoresystem.Controllers;


namespace CardMatching.Tests.EditMode
{
    public class ScoreManagerTest
    {
        private GameEvents _dummyGameEvents;
        private ScoreManager _scoreManager;

        // [SetUp] etiketi, bu dosyadaki HER [Test] metodundan önce OTOMATİK olarak 1 kez çalışır!
        // Böylece her testin içine "new GameEvents()" yazmak zorunda kalmayız.
        [SetUp]
        public void Setup()
        {
            _dummyGameEvents = new GameEvents();
            _scoreManager = new ScoreManager(2, _dummyGameEvents); // minimumStreak = 2
            _scoreManager.Initialize();
        }

        // [TearDown] etiketi, bu dosyadaki HER [Test] metodundan sonra OTOMATİK olarak çalışır!
        [TearDown]
        public void Teardown()
        {
            _scoreManager.Dispose();
        }


        [Test]
        public void EarnedPointEvent_IncreasesScore_WhenCardsMatch()
        {
            // ARRANGE (Değişkenler Setup'tan hazır geldi)
            float earnedPoint = 0;
            _dummyGameEvents.EarnedPoint += (point) => earnedPoint += point;

            // ACT
            _dummyGameEvents.MatchingCard?.Invoke(null, null);

            // ASSERT
             Assert.AreEqual(5f, earnedPoint);
             // Assert.AreEqual(6f, earnedPoint);
             // Assert.AreEqual(5f, earnedPoint);
        }

        // İKİNCİ TESTİMİZ: Kartlar eşleşmezse skor ARTMAMALI!
        [Test]
        public void EarnedPointEvent_DoesNotFire_WhenCardsMismatch()
        {
            // ARRANGE
            float earnedPoint = 0;
            _dummyGameEvents.EarnedPoint += (point) => earnedPoint += point;

            // ACT (Bu sefer Mismatch - Yanlış eşleşme tetikliyoruz)
            _dummyGameEvents.MismatchingCard?.Invoke();

            // ASSERT
            // Yanlış eşleşmede puan verilmez, bu yüzden earnedPoint hala 0 kalmalı!
            Assert.AreEqual(0f, earnedPoint);
        }
    }
}
