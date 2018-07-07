using System;
using System.Collections.Generic;
using System.ServiceProcess;
using MargieBot;
using CreativeGurus.Weather.Wunderground;
using System.Configuration;

namespace AdamWeatherBotService
{
    public partial class AdamWeatherBotService : ServiceBase
    {
        private static Bot bot;

        public AdamWeatherBotService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Set up MargieBot
            bot = new Bot();

            var responders = new List<AdamWeatherBotResponder>() { new AdamWeatherBotResponder() };

            foreach (var responder in responders)
            {
                bot.Responders.Add(responder);
            }

            //Set up WUnderground stuff
            string WUnderkey = ConfigurationManager.AppSettings["WUndergroundKey"];
            WeatherClient WUnderClient = new WeatherClient(WUnderkey);
            WeatherGetter.Client = WUnderClient;

            bot.ConnectionStatusChanged += (bool isConnected) => {
                if (!isConnected)
                {
                    bot.Connect(ConfigurationManager.AppSettings["SlackBotKey"]);
                    Console.WriteLine("-------------------------BOT RECONNECTED!-------------------------");
                }
            };

            bot.Connect(ConfigurationManager.AppSettings["SlackBotKey"]);
            Console.WriteLine("-------------------------BOT CONNECTED!-------------------------");
        }

        protected override void OnStop()
        {
            bot.Disconnect();
        }
    }
}