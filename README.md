# Ocean Networks Web Scraper
### Purpose of the application:
The purpose of this application is to use a web scraper to scrape data from the [Ocean Networks Stories](https://www.oceannetworks.ca/news/stories) website. The user clicks on the start button, and all the stories from the website are displayed alongside their corresponding URL.

**More detailed explanation:**
The user clicks on the start button and a HttpClient requests and recieves responses from the website. The website's code is then parsed and this data is then stored on a IHtmlDocument, so that it can be further processed. The data in the document is then examined to find articles that contain the query terms set, and the HTML code for these elements is acquired. The HTML code is then cleaned and validated, so that it is readible for the user. Then the data is displayed to the user showing them a stories' name and URL.
____
### Code documentation:
* __private void StartCodeButton_Click(object sender, EventArgs e):__
	```
	ScrapeWebsite();
	```
	When the start button is clicked on the form, the ScrapeWebsite() method will be executed by the application.

* __internal async void ScrapeWebsite():__
	 ```
    CancellationTokenSource cancellationToken = new CancellationTokenSource();

	 HttpClient httpClient = new HttpClient();
     HttpResponseMessage request = await httpClient.GetAsync(siteUrl);

     cancellationToken.Token.ThrowIfCancellationRequested();

     Stream response = await request.Content.ReadAsStreamAsync();
     cancellationToken.Token.ThrowIfCancellationRequested();

     HtmlParser parser = new HtmlParser();
     IHtmlDocument document = parser.ParseDocument(response);

     GetScrapeResults(document);
	```
	A token is provided if a cancellation is requested by a task or thread. Then an instance of a HttpClient is created, so it can be used as a base for sending and recieving responses. Then a response message is created for a request that includes the request's status code and data.  If a cancellation is requested, the exception is thrown. Then for the 7th & 8th lines of code, AngleSharp classes are used to create/build and parse documents from the website's HTML content.

* __private void GetScrapeResults(IHtmlDocument document):__
	```
	IEnumerable<IElement> articleLink = null;
	
    foreach (var term in QueryTerms)
    {
        articleLink = document.All.Where(x => x.ClassName == "views-field views-field-nothing" && (x.ParentElement.InnerHtml.Contains(term) || x.ParentElement.InnerHtml.Contains(term.ToLower())));

        if (articleLink.Any())
        {
            PrintResults(articleLink);
        }
    }            
	```
	In this method, a loop is used to loop through each query term and parse the document to find all the instances where the class name of an element is "views-field views-field-nothing" & where the ParentElement.InnerHtml contains the query being searched for.

* __public void PrintResults(IEnumerable<IElement> articleLink):__
	```
	foreach (var element in articleLink)
	{
		CleanResults(element);
	}
	```
	In this method a for loop is used so that every element that is stored will be cleaned and validated, so that it can be appended to the rich text box ready for a user to read.

* __private void CleanResults(IElement result):__
	```
	string htmlResult = result.InnerHtml.ReplaceFirst("        <span class=\"field-content\"><div><a href=\"", "https://www.oceannetworks.ca");
	htmlResult = htmlResult.ReplaceFirst("\">", "*");
	
	htmlResult = htmlResult.ReplaceFirst("</a></div>\n<div class=\"article-title-top\">", "-");
	htmlResult = htmlResult.ReplaceFirst("</div>\n<hr></span>  ", "");

	ResultValidation(htmlResult);
	```
	In this method the HTML code stored within the IElement is cleaned, by removing strings of text from the HTML. By replacing strings with an asterisk ( * ) it allows methods later on in the software to split the string up into seperate pieces of data.
* __private void ResultValidation(string htmlResult):__
	```
	  if ((htmlResult.Contains("<") || htmlResult.Contains(">")) == false)
	  {
	      SplitResults(htmlResult);
	  }
	```
	In this method, the htmlResult element is validated to check whether or not it still has any HTML tags present . If the string passes validation, then it is passed into the next method.

* __private void SplitResults(string htmlResult):__ 
	```
	string[] splitResults = htmlResult.Split('*');
    Url = splitResults[0];
    Title = splitResults[1];
    AppendResults();
	```
	Following on from the method __"CleanResults(IElement result)"__ where the code replaced HTML with an asterisk, the code now uses the asterisk as a way to split the string up into multiple strings, so that the URL and Title of the story can be fully extracted from the website's HTML code. This data is then assigned to global variables that are declared in the **"WebScrapeDisplay" Class**.
* __AppendResults():__
	```
	richTextBox.AppendText($"{Title} - {Url}{Environment.NewLine}");
	```
	In the final method of the process, a string is created in a specific format so that it is readable for the user. This string is then appended to the rich text box and is displayed to the reader
____
### Further remarks:
If you do not understand some lines of code displayed in the documentation, you can check the history of the code branches to see some code commenting that can explain certain parts to you in more detail.
