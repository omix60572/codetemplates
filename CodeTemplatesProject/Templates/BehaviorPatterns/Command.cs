namespace CodeTemplatesProject.Templates.BehaviorPatterns;

public class Mail
{
    public string To { get; set; }
    public string From { get; set; }
    public string MessageText { get; set; }
}

public class MailBox
{
    private readonly string address;
    private readonly List<Mail> mails;

    public MailBox(string address)
    {
        Console.WriteLine($"Mail box with address: [{address}], created");

        this.address = address;
        this.mails = new List<Mail>();
    }

    public MailBox(string address, IEnumerable<Mail> mails)
    {
        Console.WriteLine($"Mail box (for post office) with address: [{address}], created (with mails count: {mails.Count()})");

        this.address = address;
        this.mails = mails.ToList();
    }

    public string GetAddress() => this.address;

    public bool AddMail(Mail mail)
    {
        if (this.address.Equals(mail.To, StringComparison.InvariantCultureIgnoreCase))
        {
            this.mails.Add(mail);
            Console.WriteLine($"Mail successfully added to mailbox, [{this.address}], from: [{mail.From}], mails count: {this.mails.Count}");

            return true;
        }

        Console.WriteLine($"Wrong mail, failed to add to mailbox, [{this.address}], mail to [{mail.To}], from [{mail.From}]");
        return false;
    }

    public Mail GetMail()
    {
        var mail = this.mails.FirstOrDefault();
        if (mail != null)
        {
            this.mails.Remove(mail);
            Console.WriteLine($"Mail removed from mailbox, [{this.address}], to: [{mail.To}], from: [{mail.From}], mails count: {this.mails.Count}");
            return mail;
        }

        Console.WriteLine($"Mailbox is empty, [{this.address}]");
        return null;
    }
}

public class PostMan
{
    private readonly Func<string, MailBox> getTarget;

    public PostMan(Func<string, MailBox> getTarget) =>
        this.getTarget = getTarget;

    public bool DoJob(MailBox source)
    {
        var mailToDelivery = source.GetMail();
        do
        {
            var target = this.getTarget(mailToDelivery.To);
            var deliverySuccessfull = target.AddMail(mailToDelivery);
            if (!deliverySuccessfull)
            {
                Console.WriteLine("Something went wrong, going back to post office");
                return false;
            }

            mailToDelivery = source.GetMail();
        } while (mailToDelivery != null);

        return true;
    }
}

public class PostOffice
{
    private readonly string address;
    private readonly PostMan postMan;

    private readonly List<MailBox> mailBoxes;

    public PostOffice()
    {
        this.mailBoxes = new List<MailBox>();

        MailBox getTargetMailBox(string address)
        {
            var existingMailBox = this.mailBoxes.FirstOrDefault(x => x.GetAddress().Equals(address, StringComparison.InvariantCultureIgnoreCase));
            if (existingMailBox != null)
                return existingMailBox;

            var newMailBox = new MailBox(address);
            this.mailBoxes.Add(newMailBox);
            return newMailBox;
        }

        this.address = "Post office address 1";
        this.postMan = new PostMan(getTargetMailBox);
        Console.WriteLine($"Post office is open, post office address: [{this.address}]");
    }

    public bool DeliveryMail(Mail mail)
    {
        Console.WriteLine($"Post office received mail to delivery, post office address: [{this.address}]");
        var sourceMailBox = new MailBox(this.address, new[] { mail });

        return this.postMan.DoJob(sourceMailBox);
    }

    public bool DeliveryMails(IEnumerable<Mail> mails)
    {
        Console.WriteLine($"Post office received mails to delivery (mails count: {mails.Count()}), post office address: [{this.address}]");
        var sourceMailBox = new MailBox(this.address, mails);

        return this.postMan.DoJob(sourceMailBox);
    }
}

public class DeliveryCommand
{
    public Mail Mail { get; set; }
}

public class DeliveryManyCommand
{
    public IEnumerable<Mail> Mails { get; set; }
}

public class DeliveryCommandHandler
{
    private readonly PostOffice postOffice;

    public DeliveryCommandHandler(PostOffice postOffice) =>
        this.postOffice = postOffice;

    public bool Handle(DeliveryCommand command) =>
        this.postOffice.DeliveryMail(command.Mail);
}

public class DeliveryManyCommandHandler
{
    private readonly PostOffice postOffice;

    public DeliveryManyCommandHandler(PostOffice postOffice) =>
        this.postOffice = postOffice;

    public bool Handle(DeliveryManyCommand command) =>
        this.postOffice.DeliveryMails(command.Mails);
}

public class CommandsService
{
    private readonly DeliveryCommandHandler deliveryCommandHandler;
    private readonly DeliveryManyCommandHandler deliveryManyCommandHandler;

    public CommandsService()
    {
        var postOffice = new PostOffice();
        this.deliveryCommandHandler = new DeliveryCommandHandler(postOffice);
        this.deliveryManyCommandHandler = new DeliveryManyCommandHandler(postOffice);

        Console.WriteLine("Commands service started");
    }

    public bool ExecuteCommand(DeliveryCommand command) =>
        this.deliveryCommandHandler.Handle(command);

    public bool ExecuteCommand(DeliveryManyCommand command) =>
        this.deliveryManyCommandHandler.Handle(command);
}

public class Command : ICodeTemplate
{
    public void Run()
    {
        var service = new CommandsService();
        var command = new DeliveryCommand
        {
            Mail = new Mail
            {
                To = "Address 2",
                From = "Address 1",
                MessageText = "Hello World!"
            }
        };

        Console.WriteLine("Executing delivery command (single mail)");

        // Main point, we just executing some command
        var executedSuccessfully = service.ExecuteCommand(command);
        Console.WriteLine(executedSuccessfully ? "Command executed successfully (single mail)" : "Failed to execute provided command (single mail)");

        Console.Write("\n\n");
        Console.WriteLine("Executing next command to delivery (multiple mails)");

        var secondCommand = new DeliveryManyCommand
        {
            Mails = new[]
            {
                new Mail
                {
                    To = "Address 2",
                    From = "Address 1",
                    MessageText = "Hello World 1!"
                },
                new Mail
                {
                    To = "Address 2",
                    From = "Address 3",
                    MessageText = "Hello World 2!"
                },
                new Mail
                {
                    To = "Address 1",
                    From = "Address 2",
                    MessageText = "Hello World 3!"
                },
                new Mail
                {
                    To = "Address 3",
                    From = "Address 1",
                    MessageText = "Hello World 4!"
                }
            }
        };

        // Main point, we just executing some command
        var secondExecutedSuccessfully = service.ExecuteCommand(secondCommand);
        Console.WriteLine(secondExecutedSuccessfully ? "Second command executed successfully (multiple mails)" : "Failed to execute provided second command (multiple mails)");
    }
}
