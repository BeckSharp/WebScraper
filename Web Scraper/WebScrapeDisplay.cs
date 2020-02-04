using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Net.Http;
using System.IO; 
using AngleSharp.Dom;
using AngleSharp.Text;

namespace Web_Scraper
{
    public partial class WebScrapeDisplay : MetroFramework.Forms.MetroForm
    {
        public WebScrapeDisplay()
        {
            InitializeComponent();
        }

        private string Title { get; set; }
        private string Url { get; set; }
        private readonly string siteUrl = "https://www.oceannetworks.ca/news/stories";
        public string[] QueryTerms { get; } = { "Ocean", "Nature", "Pollution" };

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void StartCodeButton_Click(object sender, EventArgs e)
        {
            ScrapeWebsite();
        }

        internal async void ScrapeWebsite()
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(siteUrl);

            cancellationToken.Token.ThrowIfCancellationRequested();

            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(response);

            GetScrapeResults(document);
        }

        private void GetScrapeResults(IHtmlDocument document)
        {
            IEnumerable<IElement> articleLink = null;

            foreach (var term in QueryTerms)
            {
                articleLink = document.All.Where(x => x.ClassName == "views-field views-field-nothing" && (x.ParentElement.InnerHtml.Contains(term) || x.ParentElement.InnerHtml.Contains(term.ToLower())));

                if (articleLink.Any())
                {
                    PrintResults(articleLink);
                }
            }            
        }

        public void PrintResults(IEnumerable<IElement> articleLink)
        {
            foreach (var element in articleLink)
            {
                CleanResults(element);             
            }
        }

        private void CleanResults(IElement result)
        {
            string htmlResult = result.InnerHtml.ReplaceFirst("        <span class=\"field-content\"><div><a href=\"", "https://www.oceannetworks.ca");
            htmlResult = htmlResult.ReplaceFirst("\">", "*");

            htmlResult = htmlResult.ReplaceFirst("</a></div>\n<div class=\"article-title-top\">", "-");
            htmlResult = htmlResult.ReplaceFirst("</div>\n<hr></span>  ", "");

            ResultValidation(htmlResult);
        }

        private void ResultValidation(string htmlResult)
        {
            if ((htmlResult.Contains("<") || htmlResult.Contains(">")) == false)
            {
                SplitResults(htmlResult);
            }
        }

        private void SplitResults(string htmlResult)
        {
            string[] splitResults = htmlResult.Split('*');
            Url = splitResults[0];
            Title = splitResults[1];
            AppendResults();
        }

        private void AppendResults()
        {
            richTextBox.AppendText($"{Title} - {Url}{Environment.NewLine}");
        }
    }
}
