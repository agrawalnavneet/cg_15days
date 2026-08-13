using System;
using System.Collections.Generic;

namespace Banking
{
    public interface ITransaction
    {
        string TransactionId { get; }
        decimal Amount { get; }
        DateTime TransactionDate { get; }

        void Execute();
        void Rollback();
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public interface IValidator
    {
        bool Validate();
    }

    public abstract class TransactionBase : ITransaction, ILogger, IValidator
    {
        public string TransactionId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime TransactionDate { get; private set; }

        public string Status { get; protected set; }

        protected TransactionBase(decimal amount)
        {
            TransactionId = Guid.NewGuid().ToString();
            Amount = amount;
            TransactionDate = DateTime.Now;
            Status = "Pending";
        }

        public abstract void Execute();

        public abstract void Rollback();

        public virtual bool Validate()
        {
            if (Amount <= 0)
            {
                Console.WriteLine("Validation failed: Amount must be greater than 0.");
                return false;
            }

            return true;
        }

        public virtual void Log(string message)
        {
            Console.WriteLine($"[LOG] {message}");
        }

        protected void ShowTransactionDetails()
        {
            Console.WriteLine($"Transaction ID: {TransactionId}");
            Console.WriteLine($"Amount: {Amount}");
            Console.WriteLine($"Date: {TransactionDate}");
            Console.WriteLine($"Status: {Status}");
        }
    }
}

namespace Banking.Deposit
{
    using Banking;

    public sealed class DepositTransaction : TransactionBase
    {
        private decimal _balance;

        public decimal Balance
        {
            get { return _balance; }
            private set { _balance = value; }
        }

        public DepositTransaction(decimal amount) : base(amount)
        {
        }

        public override void Execute()
        {
            if (!Validate())
            {
                Status = "Failed";
                return;
            }

            Balance += Amount;
            Status = "Completed";

            Log($"Deposit of {Amount} completed successfully.");

            Console.WriteLine("\nDeposit Transaction");
            ShowTransactionDetails();
            Console.WriteLine($"Balance: {Balance}");
        }

        public override void Rollback()
        {
            if (Status == "Completed")
            {
                Balance -= Amount;
                Status = "Rolled Back";

                Log($"Deposit of {Amount} rolled back.");

                Console.WriteLine($"Deposit rollback successful. Balance: {Balance}");
            }
        }
    }
}

namespace Banking.Transfer
{
    using Banking;

    public sealed class TransferTransaction : TransactionBase
    {
        public string FromAccount { get; private set; }
        public string ToAccount { get; private set; }

        public TransferTransaction(
            string fromAccount,
            string toAccount,
            decimal amount) : base(amount)
        {
            FromAccount = fromAccount;
            ToAccount = toAccount;
        }

        public override bool Validate()
        {
            if (!base.Validate())
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(FromAccount))
            {
                Console.WriteLine("Validation failed: From account is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(ToAccount))
            {
                Console.WriteLine("Validation failed: To account is required.");
                return false;
            }

            if (FromAccount == ToAccount)
            {
                Console.WriteLine("Validation failed: Accounts must be different.");
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            if (!Validate())
            {
                Status = "Failed";
                return;
            }

            Status = "Completed";

            Log(
                $"Transfer of {Amount} from {FromAccount} to {ToAccount} completed."
            );

            Console.WriteLine("\nTransfer Transaction");
            ShowTransactionDetails();
            Console.WriteLine($"From Account: {FromAccount}");
            Console.WriteLine($"To Account: {ToAccount}");
        }

        public override void Rollback()
        {
            if (Status == "Completed")
            {
                Status = "Rolled Back";

                Log(
                    $"Transfer of {Amount} from {FromAccount} to {ToAccount} rolled back."
                );

                Console.WriteLine("Transfer rollback successful.");
            }
        }
    }
}

namespace Banking.Withdraw
{
    using Banking;

    public sealed class WithdrawTransaction : TransactionBase
    {
        private decimal _balance;

        public decimal Balance
        {
            get { return _balance; }
            private set { _balance = value; }
        }

        public WithdrawTransaction(decimal amount, decimal currentBalance)
            : base(amount)
        {
            Balance = currentBalance;
        }

        public override bool Validate()
        {
            if (!base.Validate())
            {
                return false;
            }

            if (Amount > Balance)
            {
                Console.WriteLine("Validation failed: Insufficient balance.");
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            if (!Validate())
            {
                Status = "Failed";
                return;
            }

            Balance -= Amount;
            Status = "Completed";

            Log($"Withdrawal of {Amount} completed successfully.");

            Console.WriteLine("\nWithdraw Transaction");
            ShowTransactionDetails();
            Console.WriteLine($"Balance: {Balance}");
        }

        public override void Rollback()
        {
            if (Status == "Completed")
            {
                Balance += Amount;
                Status = "Rolled Back";

                Log($"Withdrawal of {Amount} rolled back.");

                Console.WriteLine($"Withdrawal rollback successful. Balance: {Balance}");
            }
        }
    }
}

namespace Banking
{
    public class TransactionProcessor
    {
        private readonly List<ITransaction> _transactionHistory;

        public IReadOnlyList<ITransaction> TransactionHistory
        {
            get { return _transactionHistory.AsReadOnly(); }
        }

        public TransactionProcessor()
        {
            _transactionHistory = new List<ITransaction>();
        }

        public void ProcessTransaction(ITransaction transaction)
        {
            transaction.Execute();

            _transactionHistory.Add(transaction);

            Console.WriteLine("\nTransaction added to history.");
        }

        public void ShowHistory()
        {
            Console.WriteLine("\n===== TRANSACTION HISTORY =====");

            if (_transactionHistory.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            foreach (ITransaction transaction in _transactionHistory)
            {
                Console.WriteLine(
                    $"ID: {transaction.TransactionId} | " +
                    $"Amount: {transaction.Amount} | " +
                    $"Date: {transaction.TransactionDate}"
                );
            }
        }

        public void RollbackLastTransaction()
        {
            if (_transactionHistory.Count == 0)
            {
                Console.WriteLine("No transaction available to rollback.");
                return;
            }

            ITransaction lastTransaction =
                _transactionHistory[_transactionHistory.Count - 1];

            lastTransaction.Rollback();

            _transactionHistory.RemoveAt(
                _transactionHistory.Count - 1
            );

            Console.WriteLine("Last transaction removed from history.");
        }
    }
}

namespace BankingApp
{
    using Banking;
    using Banking.Deposit;
    using Banking.Transfer;
    using Banking.Withdraw;

    class Program
    {
        static void Main(string[] args)
        {
            TransactionProcessor processor =
                new TransactionProcessor();

            Console.WriteLine("===== BANKING TRANSACTION PROCESSOR =====");

            // Deposit transaction
            DepositTransaction deposit =
                new DepositTransaction(5000);

            processor.ProcessTransaction(deposit);

            // Withdraw transaction
            WithdrawTransaction withdraw =
                new WithdrawTransaction(1000, 5000);

            processor.ProcessTransaction(withdraw);

            // Transfer transaction
            TransferTransaction transfer =
                new TransferTransaction(
                    "ACC1001",
                    "ACC2002",
                    2000
                );

            processor.ProcessTransaction(transfer);

            // Display transaction history
            processor.ShowHistory();

            // Undo the last transaction
            Console.WriteLine("\n===== UNDO LAST TRANSACTION =====");

            processor.RollbackLastTransaction();

            // Display updated history
            processor.ShowHistory();

            Console.WriteLine("\nProgram completed.");
        }
    }
}