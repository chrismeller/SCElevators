using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SCElevators.Client.DTOs;

namespace SCElevators.Client
{
    public class ElevatorsScraper
    {
        private readonly List<string> _userAgents = new List<string>()
        {
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; FSL 7.0.6.01001)",
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; FSL 7.0.7.01001)",
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; FSL 7.0.5.01003)",
            "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:12.0) Gecko/20100101 Firefox/12.0",
            "Mozilla/5.0 (X11; U; Linux x86_64; de; rv:1.9.2.8) Gecko/20100723 Ubuntu/10.04 (lucid) Firefox/3.6.8",
            "Mozilla/5.0 (Windows NT 5.1; rv:13.0) Gecko/20100101 Firefox/13.0.1",
            "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:11.0) Gecko/20100101 Firefox/11.0",
            "Mozilla/5.0 (X11; U; Linux x86_64; de; rv:1.9.2.8) Gecko/20100723 Ubuntu/10.04 (lucid) Firefox/3.6.8",
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.0.3705)",
            "Mozilla/5.0 (Windows NT 5.1; rv:13.0) Gecko/20100101 Firefox/13.0.1",
            "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:13.0) Gecko/20100101 Firefox/13.0.1",
            "Mozilla/5.0 (compatible; Baiduspider/2.0; +http://www.baidu.com/search/spider.html)",
            "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)",
            "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)",
            "Opera/9.80 (Windows NT 5.1; U; en) Presto/2.10.289 Version/12.01",
            "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)",
            "Mozilla/5.0 (Windows NT 5.1; rv:5.0.1) Gecko/20100101 Firefox/5.0.1",
            "Mozilla/5.0 (Windows NT 6.1; rv:5.0) Gecko/20100101 Firefox/5.02",
            "Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.112 Safari/535.1",
            "Mozilla/4.0 (compatible; MSIE 6.0; MSIE 5.5; Windows NT 5.0) Opera 7.02 Bork-edition [en]",
        };

        private HttpClient CreateClient()
        {
            var client = new HttpClient();

            var random = (new Random()).Next(0, _userAgents.Count - 1);
            client.DefaultRequestHeaders.Add("User-Agent", _userAgents[random]);

            return client;
        }

        public async Task<Elevator> GetElevator(Int32 number)
        {
            var url = $"https://eservice.llr.sc.gov/ElevLookup/ElevSearch/Detail/{number}";

            using (var http = CreateClient())
            {
                var response = await http.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(content);

                // the number we search for is actually just the number in the database, so we don't know what its registration number actually is yet
                var num = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), '#' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");

                // if there was no number found then we don't need to do anything else - this is a blank page that indicates we've hit the end of the list
                if (num == null || String.IsNullOrWhiteSpace(num.InnerText))
                {
                    throw new Exception("Elevator not found!");
                }

                var owner = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'Owner' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var location = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'Location' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var address = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'Address' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var city = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'City' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var state = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'State' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var zip = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'Zip' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var county = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'County' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var status = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'Status' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var nextInspection = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'Next Inspection' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var machineType = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'Machine Type' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var manufacturer = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'Manufacturer' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");
                var unitType = doc.DocumentNode.SelectSingleNode(
                    "//div[ contains( @class, 'display-label' ) and contains( text(), 'Unit Type' )]/following-sibling::div[ contains( @class, 'display-field' ) ]/text()");

                var numAsInt = Convert.ToInt32(num?.InnerText);
                string nextInspectionOther = null;
                if (DateTime.TryParse(nextInspection?.InnerText, out var nextInspectionAsDate) == false)
                {
                    nextInspectionOther = nextInspection?.InnerText;
                }

                var elevator = new Elevator()
                {
                    Number = numAsInt,
                    OwnerName = owner?.InnerText,
                    Location = location?.InnerText,
                    LocationAddress = address?.InnerText,
                    LocationCity = city?.InnerText,
                    LocationState = state?.InnerText,
                    LocationZip = zip?.InnerText,
                    County = county?.InnerText,
                    Status = status?.InnerText,
                    NextInspectionDue = (nextInspectionOther == null) ? nextInspectionAsDate : null as DateTime?,
                    NextInspectionDueOther = nextInspectionOther,
                    MachineType = machineType?.InnerText,
                    Manufacturer = manufacturer?.InnerText,
                    UnitType = unitType?.InnerText,
                };

                return elevator;
            }
        }
    }
}