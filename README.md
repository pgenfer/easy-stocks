# easy-stocks
A web based stock quote application

easy-stocks is a stock-ticker application that uses the Yahoo Financial API to retrieve current stock exchange courses and visualizes them in an Angular based frontend.

For every stock, a stop rate which is 90% of the inital exchange rate is stored in the beginning and will be adjusted every time the exchange rate increases.
So, whenever the price of the share rises, the stop rate goes along, but when the price decreases, the stop rate remains.

As soon as the current exchange rate drops below the current stop rate, a notifcation is generated and the user can react accordingly.

In that way, losses during stock trading can ideally be reduced to 10% of the initial value.


