using NUnit.Framework;
using CardMatching.Features.ScoreSystem.Controllers;
using MessagePipe;
using CardMatching.Features.MatchMechanic.Signals;
using CardMatching.Features.ScoreSystem.Signals;
using VContainer;
using CardMatching.Core.Events.Signals;
using UnityEngine;
using CardMatching.Core.Settings;


namespace CardMatching.Tests.EditMode
{
    public class ScoreManagerTest
    {
        private IObjectResolver _container;
        private ScoreManager _scoreManager;
        private CurrentGameDataSO _dummyGameData;

        private IPublisher<MatchingCardSignal> _matchingCardPub;
        private IPublisher<MismatchingCardSignal> _mismatchingCardPub;
        private ISubscriber<EarnedPointSignal> _earnedPointSub;


        // [SetUp] etiketi, bu dosyadaki HER [Test] metodundan önce OTOMATİK olarak 1 kez çalışır!
        // Böylece her testin içine "new GameEvents()" yazmak zorunda kalmayız.
        [SetUp]
        public void Setup()
        {
            // 1. Sadece bu test için MİNİ BİR ANAKART (Container) yaratıyoruz!
            var builder = new ContainerBuilder();
            var options = builder.RegisterMessagePipe();

            // 2. Testin ihtiyaç duyduğu sinyalleri kaydediyoruz
            builder.RegisterMessageBroker<NewGameStartingSignal>(options);
            builder.RegisterMessageBroker<MatchingCardSignal>(options);
            builder.RegisterMessageBroker<MismatchingCardSignal>(options);
            builder.RegisterMessageBroker<EarnedPointSignal>(options);
            builder.RegisterMessageBroker<EarnedComboPointSignal>(options);

            // 3. Test için "Sahte (Dummy)" bir GameData nesnesi yaratıyoruz
            _dummyGameData = ScriptableObject.CreateInstance<CurrentGameDataSO>();
            builder.RegisterInstance(_dummyGameData);

            // 4. Test edeceğimiz sınıfı (ScoreManager) anakarta kaydediyoruz
            // İçindeki int parametresini (minimumStreak) .WithParameter ile doğrudan veriyoruz
            builder.Register<ScoreManager>(Lifetime.Scoped).WithParameter(2);   // minimumStreak = 2

            // 5. Anakartı çalıştır!
            _container = builder.Build();

            // 6. Test için gereken parçaları anakarttan çek (Resolve)
            _scoreManager = _container.Resolve<ScoreManager>();
            _scoreManager.Initialize(); // Dinleyicileri (Subscribe) aktifleştir

            _matchingCardPub = _container.Resolve<IPublisher<MatchingCardSignal>>();
            _mismatchingCardPub = _container.Resolve<IPublisher<MismatchingCardSignal>>();
            _earnedPointSub = _container.Resolve<ISubscriber<EarnedPointSignal>>();
        }

        // [TearDown] etiketi, bu dosyadaki HER [Test] metodundan sonra OTOMATİK olarak çalışır!
        [TearDown]
        public void Teardown()
        {
            _scoreManager.Dispose();
            _container.Dispose();
        }


        [Test]
        public void EarnedPointEvent_IncreasesScore_WhenCardsMatch()
        {
            // ARRANGE (Değişkenler Setup'tan hazır geldi)
            float earnedPoint = 0;
            // Puan geldiğinde earnedPoint değişkenini artıracak bir dinleyici ekliyoruz
            var disposable = _earnedPointSub.Subscribe(signal => earnedPoint += signal.Point);

            // ACT (Eylem - Doğru kart eşleşme sinyali fırlat)
            _matchingCardPub.Publish(new MatchingCardSignal());

            // ASSERT
            Assert.AreEqual(5f, earnedPoint);
            // Assert.AreEqual(6f, earnedPoint);
            // Assert.AreEqual(5f, earnedPoint);

            disposable.Dispose();
        }

        // İKİNCİ TESTİMİZ: Kartlar eşleşmezse skor ARTMAMALI!
        [Test]
        public void EarnedPointEvent_DoesNotFire_WhenCardsMismatch()
        {
            // ARRANGE
            float earnedPoint = 0;
            var disposable = _earnedPointSub.Subscribe(signal => earnedPoint += signal.Point);

            // ACT (Bu sefer Mismatch - Yanlış eşleşme tetikliyoruz)
            _mismatchingCardPub.Publish(new MismatchingCardSignal());

            // ASSERT
            // Yanlış eşleşmede puan verilmez, bu yüzden earnedPoint hala 0 kalmalı!
            Assert.AreEqual(0f, earnedPoint);

            disposable.Dispose();
        }
    }
}
