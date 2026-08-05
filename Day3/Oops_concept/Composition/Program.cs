Console.WriteLine("=== Composition Example: Notification System ===");

var emailNotification = new Notification(new EmailSender());
emailNotification.NotifyUser("Your order has shipped!");

var smsNotification = new Notification(new SMSSender());
smsNotification.NotifyUser("Your OTP is 4521");

// Demonstrating flexibility: swap sender at runtime, no inheritance needed
Console.WriteLine("\n--- Switching sender dynamically ---");
IMessageSender currentSender = new EmailSender();
var dynamicNotification = new Notification(currentSender);
dynamicNotification.NotifyUser("Reminder: your payment is due");