using NUnit.Framework;

[TestFixture]
public class UnitTest
{
    [Test]
    public void Test_Deposit_ValidAmount()
    {
        Program account = new Program(1000);

        account.Deposit(500);

        Assert.That(account.Balance, Is.EqualTo(1500));
    }

    [Test]
    public void Test_Deposit_NegativeAmount()
    {
        Program account = new Program(1000);

        Assert.That(
            () => account.Deposit(-500),
            Throws.TypeOf<System.Exception>()
                .With.Message.EqualTo("Deposit amount cannot be negative")
        );
    }

    [Test]
    public void Test_Withdraw_ValidAmount()
    {
        Program account = new Program(1000);

        account.Withdraw(400);

        Assert.That(account.Balance, Is.EqualTo(600));
    }

    [Test]
    public void Test_Withdraw_InsufficientFunds()
    {
        Program account = new Program(1000);

        Assert.That(
            () => account.Withdraw(1500),
            Throws.TypeOf<System.Exception>()
                .With.Message.EqualTo("Insufficient funds.")
        );
    }
}