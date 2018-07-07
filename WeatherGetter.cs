using System;
using System.Collections.Generic;
using MargieBot;
using CreativeGurus.Weather.Wunderground;
using CreativeGurus.Weather.Wunderground.Models;
using CreativeGurus.Weather.Wunderground.ResultModels;

public static class WeatherGetter
{
    public static WeatherClient Client { get; set; }

    public static string GetWeather(string Query)
    {
        if (Client != null)
        {
            try
            {
                string Response = "";

                GeoLookupResponse ReturnedGeo = Client.GetGeoLookup(QueryType.ZipCode, new QueryOptions() { ZipCode = Query });

                HourlyResponse ReturnedHourly = Client.GetHourly(QueryType.ZipCode, new QueryOptions() { ZipCode = Query });

                ForecastResponse ReturnedForecast = Client.GetForecast(QueryType.ZipCode, new QueryOptions() { ZipCode = Query });

                string Location = "";

                if (ReturnedGeo != null)
                {
                    Location = ReturnedGeo.Location.City + ", " + ReturnedGeo.Location.State;
                }

                if (ReturnedHourly != null)
                {
                    Response = $"TEST Hourly{(!string.IsNullOrWhiteSpace(Location) ? " for " + Location + ":" : ":")}{Environment.NewLine}{Environment.NewLine}";

                    Response += "Thing1 | Thing 2" + Environment.NewLine + "------------|------------" + Environment.NewLine + "bleh | blah" + Environment.NewLine + "123 | 456";

                    int i = 0; //Don't need 40 million hours
                    int HourlyLimit = 12;

                    foreach (var H in ReturnedHourly.Hourly_Forecast)
                    {
                        if (i < HourlyLimit)
                        {
                            string LeadingZero = (H.FctTime.Hour > 0 && H.FctTime.Hour < 10) || (H.FctTime.Hour > 12 && H.FctTime.Hour < 22) ? "0" : "";

                            Response += string.Format("{0, -39}\t{1, -7}\t{2, 5}\t{3, -13}\t{4, 5}\t{5, -16}\t{6}" + Environment.NewLine, LeadingZero + H.FctTime.Pretty, "Temp:", H.Temp.English, "Feels Like:", H.Feelslike.English, "Conditions:", H.Condition);
                            i += 1;
                        }
                    }
                }

                if (ReturnedForecast != null)
                {
                    Response = !string.IsNullOrWhiteSpace(Response) ? $"{Response}{Environment.NewLine}Forecast" : "Forecast";
                    Response += (!string.IsNullOrWhiteSpace(Location) ? $" for {Location}:" : ":") + Environment.NewLine + Environment.NewLine;

                    foreach (var FD in ReturnedForecast.Forecast.Txt_Forecast.ForecastDay)
                    {
                        Response += $"{FD.Title}: {FD.FctText}{Environment.NewLine}{Environment.NewLine}";
                    }
                }

                return Response;
            }
            catch (Exception)
            {
                return "Sorry, something went wrong. :-(";
            }
        }
        else
        {
            return "No WUnderground client configured.";
        }

        return "";
    }

    public static List<SlackAttachment> GetWeatherAsAttachments(string Query)
    {
        if (Client != null)
        {
            try
            {
                List<SlackAttachment> Attachments = new List<SlackAttachment>();

                string Response = "";

                GeoLookupResponse ReturnedGeo = Client.GetGeoLookup(QueryType.ZipCode, new QueryOptions() { ZipCode = Query });

                HourlyResponse ReturnedHourly = Client.GetHourly(QueryType.ZipCode, new QueryOptions() { ZipCode = Query });

                ForecastResponse ReturnedForecast = Client.GetForecast(QueryType.ZipCode, new QueryOptions() { ZipCode = Query });

                string Location = "[Location Not Found]";
                string WUndergroundURL = "https://www.wunderground.com";
                string ForecastURL = "https://www.wunderground.com/weather/";
                string HourlyForecastURL = "https://www.wunderground.com/hourly/";

                if (ReturnedGeo != null)
                {
                    Location = ReturnedGeo.Location.City + ", " + ReturnedGeo.Location.State;
                    ForecastURL += ReturnedGeo.Location.Zip;
                    HourlyForecastURL += ReturnedGeo.Location.Zip;
                }

                if (ReturnedHourly != null)
                {
                    int i = 0; //Don't need 40 million hours
                    int HourlyLimit = 12;

                    SlackAttachment SA = new SlackAttachment();

                    var FirstHour = ReturnedHourly.Hourly_Forecast[0];

                    SA.AuthorName = "WUndergound";
                    SA.AuthorLink = WUndergroundURL;
                    SA.AuthorIcon = "https://www.wunderground.com/logos/images/wundergroundLogo_4c_rev.jpg";
                    SA.ColorHex = "2d9ee0";
                    SA.PreText = "Hourly Forecast for " + FirstHour.FctTime.MonAbbrev + " " + FirstHour.FctTime.MdayPadded + ", " + FirstHour.FctTime.Year + ":";
                    SA.Title = "Hourly";
                    SA.TitleLink = HourlyForecastURL;

                    foreach (var H in ReturnedHourly.Hourly_Forecast)
                    {
                        if (i == 0)
                        {
                            SA.ThumbUrl = H.IconUrl.Replace("/i/c/k/", "/i/c/i/");
                        }

                        if (i < HourlyLimit)
                        {
                            string LeadingZero = (H.FctTime.Hour > 0 && H.FctTime.Hour < 10) || (H.FctTime.Hour > 12 && H.FctTime.Hour < 22) ? "0" : "";

                            Response += string.Format("{0, -39}\t{1, -7}\t{2, 5}\t{3, -13}\t{4, 5}\t{5, -16}\t{6}" + Environment.NewLine, LeadingZero + H.FctTime.Pretty, "Temp:", H.Temp.English, "Feels Like:", H.Feelslike.English, "Conditions:", H.Condition);

                            SlackAttachmentField SAF = new SlackAttachmentField();

                            SAF.Title = (H.FctTime.Hour > 12 ? H.FctTime.Hour - 12 : (H.FctTime.Hour == 0 ? 12 : H.FctTime.Hour)) + ":00 " + H.FctTime.AmPm;
                            SAF.Value = "Temp: " + H.Temp.English + " | " + "Feels Like: " + H.Feelslike.English + Environment.NewLine + "Conditions: " + H.Condition;
                            SAF.IsShort = true;

                            SA.Fields.Add(SAF);

                            i += 1;
                        }
                    }

                    SA.Fallback = Response;

                    Attachments.Add(SA);
                }

                if (ReturnedForecast != null)
                {
                    SlackAttachment SA = new SlackAttachment();

                    SA.AuthorName = "WUndergound";
                    SA.AuthorLink = WUndergroundURL;
                    SA.AuthorIcon = "https://www.wunderground.com/logos/images/wundergroundLogo_4c_rev.jpg";
                    SA.ColorHex = "2d9ee0";
                    SA.PreText = "Forecast for " + Location;
                    SA.Title = "Forecast";
                    SA.TitleLink = ForecastURL;

                    Response = !string.IsNullOrWhiteSpace(Response) ? $"{Response}{Environment.NewLine}Forecast" : "Forecast";
                    Response += (!string.IsNullOrWhiteSpace(Location) ? $" for {Location}:" : ":") + Environment.NewLine + Environment.NewLine;

                    bool first = true;

                    foreach (var FD in ReturnedForecast.Forecast.Txt_Forecast.ForecastDay)
                    {
                        if (first)
                        {
                            SA.ThumbUrl = FD.Icon_Url.Replace("/i/c/k/", "/i/c/i/");
                            first = false;
                        }

                        Response += $"{FD.Title}: {FD.FctText}{Environment.NewLine}{Environment.NewLine}";

                        SlackAttachmentField SAF = new SlackAttachmentField();

                        SAF.Title = FD.Title;
                        SAF.Value = FD.FctText;
                        SAF.IsShort = true;

                        SA.Fields.Add(SAF);
                    }

                    SA.Fallback = Response;
                    Attachments.Add(SA);
                }

                return Attachments;
            }
            catch (Exception)
            {
                SlackAttachment SA = new SlackAttachment();

                SA.AuthorName = "WUndergound";
                SA.Fallback = "Sorry, something went wrong. :-(";
                SA.ColorHex = "2d9ee0";
                SA.PreText = "So broken...";
                SA.Title = "Everything is broken.";

                return new List<SlackAttachment>() { SA };
            }
        }
        else
        {
            SlackAttachment SA = new SlackAttachment();

            SA.AuthorName = "WUndergound";
            SA.Fallback = "No WUnderground client configured.";
            SA.ColorHex = "2d9ee0";
            SA.PreText = "No WUnderground client configured.";
            SA.Title = "No WUnderground client configured.";

            return new List<SlackAttachment>() { SA };
        }

        return new List<SlackAttachment>();
    }
}