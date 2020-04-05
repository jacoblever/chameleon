using System;
using System.Linq;
using NUnit.Framework;

namespace DataStore.Tests
{
    [TestFixture]
    public class ScorerTests
    {
        [Test]
        public void TestAllVoteForChameleon()
        {
            var room = new RoomBuilder()
                .WithPlayer(id: "person-1", character: "stick", votedFor: "person-3")
                .WithPlayer(id: "person-2", character: "stick", votedFor: "person-3")
                .WithPlayer(id: "person-3", character: "chameleon")
                .Build();
            
            var scorer = new Scorer(room);
            var result = scorer.CalculateScore();
            Assert.That(result["person-1"], Is.EqualTo(3));
            Assert.That(result["person-2"], Is.EqualTo(3));
            Assert.That(result["person-3"], Is.EqualTo(0));
        }
        
        [Test]
        public void TestAllVoteForWrongPerson()
        {
            var room = new RoomBuilder()
                .WithPlayer(id: "person-1", character: "stick", votedFor: "person-2")
                .WithPlayer(id: "person-2", character: "stick", votedFor: "person-1")
                .WithPlayer(id: "person-3", character: "chameleon")
                .Build();
            
            var scorer = new Scorer(room);
            var result = scorer.CalculateScore();
            Assert.That(result["person-1"], Is.EqualTo(-1));
            Assert.That(result["person-2"], Is.EqualTo(-1));
            Assert.That(result["person-3"], Is.EqualTo(2));
        }
        
        [Test]
        public void TestOneVotesForWrongPerson()
        {
            var room = new RoomBuilder()
                .WithPlayer(id: "person-1", character: "stick", votedFor: "person-2")
                .WithPlayer(id: "person-2", character: "stick", votedFor: "person-3")
                .WithPlayer(id: "person-3", character: "chameleon")
                .Build();
            
            var scorer = new Scorer(room);
            var result = scorer.CalculateScore();
            Assert.That(result["person-1"], Is.EqualTo(1));
            Assert.That(result["person-2"], Is.EqualTo(1));
            Assert.That(result["person-3"], Is.EqualTo(1));
        }
    }
}