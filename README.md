# Hanging ConnectAsync issue

Here you'll find two Xamarin Forms projets which both show a potential bug in two different ways: the first will call Cancel() on a cancellation token source manually as a result of a button press, the other will simply use a timeout on the Cancellation Token Source. Both show a hang, however. You will never see an exception thrown from ConnectAsync, nor will it ever complete. 

In order for these issues to duplicate the device MUST be offline. 
