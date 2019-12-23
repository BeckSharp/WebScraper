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
/* Package reference uses:
 * System.Threading - Required for CancellationTokenSource.
 * AngleSharp.Html.Dom - Required to use IHtmlDocument.
 * AngleSharp.Html.Parser - Required for HtmlParser.
 * System.Net.Http - Required for HTPP objects.
 * System.IO - Required to use Stream.
 */

namespace Web_Scraper
{
    public partial class WebScrapeDisplay : MetroFramework.Forms.MetroForm
    {
        public WebScrapeDisplay()
        {
            InitializeComponent();
        }

        //Query terms.
        private string Title { get; set; }
        private string Url { get; set; }
        private readonly string siteUrl = "https://www.oceannetworks.ca/news/stories";
        public string[] QueryTerms { get; } = { "Ocean", "Nature", "Pollution" };

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        //Starting scraping when the button is clicked by the user.
        private void StartCodeButton_Click(object sender, EventArgs e)
        {
            ScrapeWebsite();
        }

        //Method setting up document to scrape.
        internal async void ScrapeWebsite()
        {
            //Providing token id a cancellation is requested by a task or thread.
            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            //HttpClient used as base for sending requests and recieving responses.
            HttpClient httpClient = new HttpClient();

            //HttpResponseMessage used as response message that includes status code and data.
            HttpResponseMessage request = await httpClient.GetAsync(siteUrl);

            cancellationToken.Token.ThrowIfCancellationRequested();

            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            //AngleSharp Classes that build and parse documents from website HTML content.
            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(response);

            GetScrapeResults(document);
        }

        //Receiving results from AngleSharp document and looking for specific terms in the html code.
        private void GetScrapeResults(IHtmlDocument document)
        {
            IEnumerable<IElement> articleLink = null;

            //Looping through each query term and parsing the document to find all instances where the class name is "views-field views-field-nothing" & where the ParentElement.InnerHtml contains the query being searched for.
            foreach (var term in QueryTerms)
            {
                articleLink = document.All.Where(x => x.ClassName == "views-field views-field-nothing" && (x.ParentElement.InnerHtml.Contains(term) || x.ParentElement.InnerHtml.Contains(term.ToLower())));

                if (articleLink.Any())
                {
                    PrintResults(articleLink);
                }
            }            
        }

        //Cleaning up every results so that it can be appended  to the rich text box.
        public void PrintResults(IEnumerable<IElement> articleLink)
        {
            foreach (var element in articleLink)
            {
                CleanResults(element);             
            }
        }

        //Replacing html code with place markers, and making text look presentable for a display.
        private void CleanResults(IElement result)
        {
            //Retrieving URL link from html code.
            string htmlResult = result.InnerHtml.ReplaceFirst("        <span class=\"field-content\"><div><a href=\"", "https://www.oceannetworks.ca");
            htmlResult = htmlResult.ReplaceFirst("\">", "*");
            //Retrieving title from html code.
            htmlResult = htmlResult.ReplaceFirst("</a></div>\n<div class=\"article-title-top\">", "-");
            htmlResult = htmlResult.ReplaceFirst("</div>\n<hr></span>  ", "");

            ResultValidation(htmlResult);
        }

        //Validating that no invalid html code will be passed through the code to be displayed to the user after results have been cleaned.
        private void ResultValidation(string htmlResult)
        {
            if ((htmlResult.Contains("<") || htmlResult.Contains(">")) == false)
            {
                SplitResults(htmlResult);
            }
        }

        //Splitting htmlResult into 2 pieces of data using the '*' placeholdedr from CleanResults().
        private void SplitResults(string htmlResult)
        {
            string[] splitResults = htmlResult.Split('*');
            Url = splitResults[0];
            Title = splitResults[1];
            AppendResults();
        }

        //Appending result to the rich text box.
        private void AppendResults()
        {
            richTextBox.AppendText($"{Title} - {Url}{Environment.NewLine}");
        }
    }
}
