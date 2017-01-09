void Main()
{
	string location = @"Leeds%20";
	string query =   @"""stockdales""" ;
	Scrape(location + query);
	
	var wordsPos = File.ReadAllLines(@"C:\temp\positives.txt");
	var textIn = File.ReadAllLines(@"C:\temp\out.txt");
	var count = MatchWords(textIn, wordsPos);
	Console.WriteLine(count);			
}

public void Scrape(string query)
		{
		
				var sb = new StringBuilder();
								var dateTo = GetStartDate(null);
		var dateFrom = GetPreviousDate(dateTo);
			for (int i = 0; i < 100; i++)
		{
		Console.WriteLine("from " + dateFrom + " TO " + dateTo);
		Thread.Sleep(500);
		dateFrom = GetPreviousDate(dateFrom);
		dateTo = GetPreviousDate(dateTo);
		
	HtmlWeb hw = new HtmlWeb();
           // string url = @"https://twitter.com/search?q=stockdales%20leeds%20great%20OR%20%3A)%20OR%20awesome%20OR%20fantastic%20OR%20happy%20OR%20good&src=typd";
           
			string url = GetUrl(dateFrom, dateTo, query);
			HtmlDocument doc = hw.Load(url);
					
            HtmlNode docNode = doc.DocumentNode;
			HtmlNodeCollection lis3 = docNode.SelectNodes("//*[@id='timeline']");
			HtmlNodeCollection lis4 = docNode.SelectNodes("//*[@class='stream-container']");			
			HtmlNodeCollection lis = docNode.SelectNodes("//p[@class='TweetTextSize  js-tweet-text tweet-text']");
		    HtmlNodeCollection lis2 = docNode.SelectNodes("//tr[@class='tweet-container']");
			
			// docNode.InnerHtml.Dump();
			var f = docNode.SelectNodes("//*[@class='dir-ltr']");
			if (f != null)
			{
				foreach (var element in f)
				{
					// "our" implies self tweet.
					// Console.WriteLine(element.InnerText);
					if (!element.InnerText.ToUpper().Contains("OUR"))
						sb.AppendLine(element.InnerText);
				}		
			}
		}
		
		File.WriteAllText("C:\\temp\\out.txt", sb.ToString());
}


public string GetStartDate(string currentDate)
{
// 2016-11-23
	if (currentDate == null) currentDate = DateTime.Today.ToString("yyyy-MM-dd");
	return currentDate.ToString();
}

public string GetPreviousDate(string startDate)
{
	DateTime x = DateTime.Parse(startDate);
	return x.AddDays(-1).ToString("yyyy-MM-dd");
}

public string GetUrl(string startDate, string endDate, string query)
{
string startUrl = @"https://twitter.com/search?f=tweets&vertical=default&q=";	

string url = startUrl + query + "%20since%3A" + startDate + @"%20until%3A" + endDate;
Console.WriteLine(url);

return url;
}

public int MatchWords(string[] textIn, string[] wordsToCheck)
{
	int count = 0;
	if (wordsToCheck.SequenceEqual(textIn))
	{
	Console.WriteLine("true");
	}

	foreach (var tweet in textIn.Distinct())
	{	
		var words = tweet.Split(' ');
		var match = false;
		
		foreach (var word in words)
		{
		if (wordsToCheck.Contains(word.ToLower()))
		{
			Console.WriteLine(tweet);
			count++;
			match = true;
			break;
		}

		}
		
		if (match == false)
		Console.WriteLine("NO MATCH " + tweet);
}
		return count;
}
