# Ocean Networks Web Scraper
### Purpose of the application:
The purpose of this application is to use a web scraper to scrape data from the [Ocean Networks Stories](https://www.oceannetworks.ca/news/stories) website. The user clicks on the start button, and all the stories from the website are displayed alongside their corresponding URL.

**More detailed explanation:**
The user clicks on the start button and a HttpClient requests and recieves responses from the website. The website's code is then parsed and this data is then stored on a IHtmlDocument, so that it can be further processed. The data in the document is then examined to find articles that contain the query terms set, and the HTML code for these elements is acquired. The HTML code is then cleaned and validated, so that it is readible for the user. Then the data is displayed to the user showing them a stories' name and URL.

### Code explained:

'''
Test
'''
