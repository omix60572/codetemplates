namespace CodeTemplatesProject.Templates.BehaviorPatterns;

public interface IAccountState
{
    IAccountState Deposit(BankAccount account, decimal amount);
    IAccountState Withdraw(BankAccount account, decimal amount);
}

public class PositiveState : IAccountState
{
    public IAccountState Deposit(BankAccount account, decimal amount)
    {
        Console.WriteLine($"Trying to deposit: {amount:C}");
        if (amount <= 0)
        {
            Console.WriteLine($"Cannot process operation, amount is negative or equal to zero: {amount:C}");
            return this;
        }

        account.Balance += amount;
        Console.WriteLine($"Deposit action: {amount:C} | New balance: {account.Balance:C}");
        return this;
    }

    public IAccountState Withdraw(BankAccount account, decimal amount)
    {
        Console.WriteLine($"Trying to withdraw: {amount:C}");
        if (amount <= 0)
        {
            Console.WriteLine($"Cannot process operation, amount is negative or equal to zero: {amount:C}");
            return this;
        }

        if (account.Balance >= amount)
        {
            account.Balance -= amount;
            Console.WriteLine($"Withdraw operation {amount:C} | New balance: {account.Balance:C}");

            return this;
        }
        else
        {
            if (account.MinBalance < 0)
            {
                Console.WriteLine("Account state now is: Negative state");
                return new NegativeState().Withdraw(account, amount);
            }
            else
            {
                Console.WriteLine($"Insufficient funds. Current balance: {account.Balance:C}");
                return this;
            }
        }
    }
}

public class NegativeState : IAccountState
{
    public IAccountState Deposit(BankAccount account, decimal amount)
    {
        Console.WriteLine($"Trying to deposit: {amount:C}");
        if (amount <= 0)
        {
            Console.WriteLine($"Cannot process operation, amount is negative or equal to zero: {amount:C}");
            return this;
        }

        account.Balance += amount;
        Console.WriteLine($"Deposit action: {amount:C} | New balance: {account.Balance:C}");

        if (account.Balance > 0)
        {
            Console.WriteLine("Account state now is: Positive state");
            return new PositiveState();
        }

        return this;
    }

    public IAccountState Withdraw(BankAccount account, decimal amount)
    {
        Console.WriteLine($"Trying to withdraw: {amount:C}");
        if (amount <= 0)
        {
            Console.WriteLine($"Cannot process operation, amount is negative or equal to zero: {amount:C}");
            return this;
        }

        var withdrawBalance = account.Balance - amount;
        if (withdrawBalance >= account.MinBalance)
        {
            account.Balance = withdrawBalance;
            Console.WriteLine($"Withdraw operation {amount:C} | New balance: {account.Balance:C}");
        }
        else
        {
            Console.WriteLine($"Insufficient funds. Current balance: {account.Balance:C} | Min value can be: {account.MinBalance:C}");
        }

        if (account.Balance > 0)
        {
            Console.WriteLine("Account state now is: Positive state");
            return new PositiveState();
        }

        return this;
    }
}

public class BankAccount
{
    public decimal Balance { get; set; }
    public decimal MinBalance { get; }

    private IAccountState state;

    public BankAccount(decimal balance, decimal minBalance)
    {
        this.Balance = balance;
        this.MinBalance = minBalance;
        this.state = balance > 0 ? new PositiveState() : new NegativeState();

        var accountType = minBalance < 0 ? "credit" : "standard";
        Console.WriteLine($"Bank account created, current balance: {this.Balance:C}, min balance value: {this.MinBalance:C}, account type: [{accountType}]");
    }

    public void Deposit(decimal amount) =>
        this.state = this.state.Deposit(this, amount);

    public void Withdraw(decimal amount) =>
        this.state = this.state.Withdraw(this, amount);
}

public class State : ICodeTemplate
{
    public void Run()
    {
        Console.WriteLine("State pattern as bank account example");
        var rnd = new Random();

        Console.WriteLine("Some operations with standard account:");
        var standardAccount = new BankAccount(100000M, 0M);
        for (var i = 0; i < 4; i++)
        {
            standardAccount.Withdraw(Math.Round(rnd.Next() / 10000M, 2));
            standardAccount.Deposit(Math.Round(rnd.Next() / 20000M, 2));
        }

        Console.WriteLine("\nPress enter to continue...");
        Console.ReadLine();

        Console.WriteLine("\n\nSome operations with credit account:");
        var creditAccount = new BankAccount(-100000M, -100000M);
        for (var i = 0; i < 5; i++)
        {
            creditAccount.Deposit(Math.Round(rnd.Next() / 20000M, 2));
            creditAccount.Withdraw(Math.Round(rnd.Next() / 10000M, 2));
        }
    }
}
