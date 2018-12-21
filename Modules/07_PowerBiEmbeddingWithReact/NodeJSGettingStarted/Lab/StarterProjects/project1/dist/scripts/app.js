

$(function () {

  var displayNewQuote = function () {
    var quote = quoteManager.getQuote();
    $("#quote").text(quote.value);
    $("#author").text(quote.author);
  };

  $("#new-quote").click(function () {
    displayNewQuote();
  });

  displayNewQuote();

});

var quoteManager = function () {

  var quotes = [
    { "value": "Always borrow money from a pessimist. He won’t expect it back.", "author": "Oscar Wilde" },
    { "value": "Behind every great man is a woman rolling her eyes.", "author": "Jim Carrey" },
    { "value": "In Hollywood a marriage is a success if it outlasts milk.", "author": "Rita Rudner" },
    { "value": "Between two evils, I always pick the one I never tried before.", "author": "Mae West" },
    { "value": "When you are courting a nice girl an hour seems like a second. When you sit on a red-hot cinder a second seems like an hour. That's relativity.", "author": "Albert Einstein" },
    { "value": "The day I made that statement, about the inventing the internet, I was tired because I'd been up all night inventing the Camcorder.", "author": "Al Gore" },
    { "value": "I always wanted to be somebody, but now I realize I should have been more specific.", "author": "Lily Tomlin" },
    { "value": "I think it's wrong that only one company can make the game Monopoly", "author": "Stephen Wright" },
    { "value": "Happiness is having a large, loving, caring, close-knit family in another city.", "author": "George Burns" },
    { "value": "All generalizations are false, including this one.", "author": "Mark Twain" },
    { "value": "My grandmother started walking five miles a day when she was sixty. She's ninety-seven now, and we don't know where the hell she is.", "author": "Ellen DeGeneres" },
    { "value": "I haven't spoken to my wife in years. I didn't want to interrupt her.", "author": "Rodney Dangerfield" },
    { "value": "You know you have a drinking problem when the bartender knows your name -- and you've never been to that bar before.", "author": "Zach Galifianakis" },
    { "value": "No man has a good enough memory to be a successful liar", "author": "Abraham Lincoln" },
    { "value": "I love deadlines. I like the whooshing sounds they make as they fly by.", "author": "Douglas Adams" }
  ];

  return {
    getQuote: function () {
      var index = Math.floor(Math.random() * quotes.length);
      console.log("Getting quote " + index)
      return quotes[index];
    }
  };
}();
