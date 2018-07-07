using System;
using MargieBot;

public class AdamWeatherBotResponder : IResponder
{
    private Random _RandomNumber;
    private int _RequiredChadTries = 0;
    private int _ChadCount = 0;

    public AdamWeatherBotResponder()
    {
        //set up random number
        _RandomNumber = new Random();
    }

    public bool CanRespond(ResponseContext context)
    {
        string[] Parts = context.Message.Text.Split(' ');

        if (Parts != null && Parts.Length >= 2)
        {
            return context.Message.MentionsBot && !context.BotHasResponded && !string.IsNullOrWhiteSpace(context.Message.Text);
        }
        else
        {
            return false;
        }
    }

    public BotMessage GetResponse(ResponseContext context)
    {
        try
        {
            if (context != null && context.Message != null)
            {
                Console.WriteLine("");
                Console.WriteLine("----------------------------------");
                Console.WriteLine("");
                Console.WriteLine("Request From: " + context.Message.ChatHub.Name + " (" + context.Message.User.ID + ")");
                Console.WriteLine("");
                Console.WriteLine("----------------------------------");
                Console.WriteLine("");

                string[] Parts = context.Message.Text.Split(' ');

                if (Parts != null && Parts.Length == 2)
                {
                    //if (context.Message.User.ID.ToUpper() == "ChadUId") //|| context.Message.User.ID.ToUpper() == "someOtherUId")
                    //{
                    //    if (_ChadCount < _RequiredChadTries)
                    //    {
                    //        _ChadCount += 1;
                    //        return new BotMessage { Text = RandomBadWeatherNewsForChad.GetRandomBadWeatherNews(_RandomNumber) };
                    //    }
                    //    else
                    //    {
                    //        _ChadCount = 0;
                    //        return new BotMessage { Text = WeatherGetter.GetWeather(Parts[1]) + Environment.NewLine + Environment.NewLine + Environment.NewLine + RandomBadWeatherNewsForChad.GetRandomBadWeatherNews(_RandomNumber) };
                    //    }
                    //}
                    //else
                    //{
                    return new BotMessage { Attachments = WeatherGetter.GetWeatherAsAttachments(Parts[1]) };
                    //}
                }
                else
                {
                    return new BotMessage { Text = "Sorry, I couldn't figure out that request.  Please use \"@AdamWeatherBot [Zip Code]\"." };
                }
            }
            else
            {
                return new BotMessage { Text = "Sorry, something went wrong. :-(" };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("");
            Console.WriteLine("------------------Exception------------------");
            Console.WriteLine("");
            Console.WriteLine(ex.ToString());
            Console.WriteLine("");
            Console.WriteLine("------------------Exception------------------");
            Console.WriteLine("");

            return new BotMessage { Text = "Sorry, something went wrong. :-(  An error message should have been logged, though, so tell Adam to fix me." };
        }
    }
}