using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading; //Required for CancellationTokenSource.
using AngleSharp.Html.Dom; //Required to use IHtmlDocument.
using AngleSharp.Html.Parser; //Required for HtmlParser.
using System.Net.Http; // Required for HTPP objects.
using System.IO; //Required to use Stream.
using AngleSharp.Dom;
using AngleSharp.Text;
using System.Windows;


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

        //Cleaning up results and appending to rich text box.
        public void PrintResults(IEnumerable<IElement> articleLink)
        {
            foreach (var element in articleLink)
            {
                CleanResults(element);

                richTextBox.AppendText($"{Title} - {Url}{Environment.NewLine}");
            }
        }

        //Replacing html code with place markers, and makes text look presentable.
        private void CleanResults(IElement result)
        {
            //Retrieving link from html code.
            string htmlResult = result.InnerHtml.ReplaceFirst("        <span class=\"field-content\"><div><a href=\"", "https://www.oceannetworks.ca");
            htmlResult = htmlResult.ReplaceFirst("\">", "*");
            //Retrieving title from html code.
            htmlResult = htmlResult.ReplaceFirst("</a></div>\n<div class=\"article-title-top\">", "-");
            htmlResult = htmlResult.ReplaceFirst("</div>\n<hr></span>  ", "");

            SplitResults(htmlResult);
        }

        //Splitting htmlResult into 2 pieces of data using the '*' placeholdedr from CleanResults().
        private void SplitResults(string htmlResult)
        {
            string[] splitResults = htmlResult.Split('*');
            Url = splitResults[0];
            Title = splitResults[1];
        }

    }
}
