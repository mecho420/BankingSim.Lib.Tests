using BankingSim.Lib;
using NUnit.Framework;


namespace Tests
{
    public class Tests
    {

        [TestFixture]
        public class AccountTests
        {
            private TestAccount _account;

            private class TestAccount : Account
            {
                public TestAccount(string owner, decimal initial = 0m)
                    : base(owner, initial) { }

                public override void Withdraw(decimal amount)
                {
                    if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
                    if (amount > Balance) throw new InvalidOperationException("Insufficient funds");
                    Balance -= amount;
                }
            }

            [SetUp]
            public void SetUp()
            {
                _account = new TestAccount("Ivan", 100m);
            }

            [TearDown]
            public void TearDown()
            {
                _account = null;
            }

            [Test]
            public void Constructor_InitializesBalanceAndOwner()
            {
                Assert.That(_account.Owner, Is.EqualTo("Ivan"));
                Assert.That(_account.Balance, Is.EqualTo(100m));
            }


            [TestCase(50)]
            [TestCase(25.75)]
            [TestCase(100)]
            public void Deposit_ShouldIncreasesBalance_WhenAmountIsPositive(decimal amount)
            {
                decimal initial = _account.Balance;
                _account.Deposit(amount);
                Assert.That(_account.Balance, Is.EqualTo(initial + amount));
            }


            [TestCase(0)]
            [TestCase(-50)]
            public void Deposit_WhenAmountIsZeroOrNegative_Throw_Exception(decimal amount)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => _account.Deposit(amount));
            }


            [Test]
            public void Withdraw_WhenEnoughFunds()
            {
                _account.Withdraw(40m);
                Assert.That(_account.Balance, Is.EqualTo(60m));
            }


            [Test]
            public void Withdraw_WhenInsufficientFunds_Throw_Exception()
            {
                Assert.Throws<InvalidOperationException>(() => _account.Withdraw(200m));
            }
        }
    }
}