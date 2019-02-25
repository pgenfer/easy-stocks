# easy-stocks
A web based stock quote application

easy-stocks is a stock-ticker application that uses the Yahoo Financial API to retrieve current stock exchange courses and visualizes them in an Angular based frontend.

For every stock, a stop rate which is 90% of the inital exchange rate is stored in the beginning and will be adjusted every time the exchange rate increases.
So, whenever the price of the share rises, the stop rate goes along, but when the price decreases, the stop rate remains.

![A list of stock items](/images/easy-stocks.png?raw=true "Optional Title")

As soon as the current exchange rate drops below the current stop rate, a notifcation is generated and the user can react accordingly.

![A list of stock items](/images/stop-rate.png?raw=true "Optional Title")

In the given picture, the rate for the given stock has already dropped and is now below the stop rate (indicated by the company name printed in red color).

But since the stock did also increase since it was initially bought, the stop rate also increased and therefore the total loss would only be about 4.2% if the stock would be sold now. 

In that way, losses during stock trading could be reduced to a maximum of 10% of the initial value.

The [NewsRiver REST API](https://newsriver.io/) is used to provide additional news information for selected stock items.

![A list of stock items](/images/news.png?raw=true "Optional Title")

