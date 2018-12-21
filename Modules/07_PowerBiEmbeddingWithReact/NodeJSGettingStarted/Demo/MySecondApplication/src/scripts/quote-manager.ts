import { Quote } from './quote';

export class QuoteManager {

  private static quotes: Quote[] = [
    new Quote("Always borrow money from a pessimist. He won’t expect it back.", "Oscar Wilde"),
    new Quote("Behind every great man is a woman rolling her eyes.", "Jim Carrey"),
    new Quote("In Hollywood a marriage is a success if it outlasts milk.", "Rita Rudner"),
    new Quote("Between two evils, I always pick the one I never tried before.", "Mae West"),
    new Quote("When you are courting a nice girl an hour seems like a second. When you sit on a red-hot cinder a second seems like an hour. That's relativity.", "Albert Einstein"),
    new Quote("The day I made that statement, about the inventing the internet, I was tired because I'd been up all night inventing the Camcorder.", "Al Gore"),
    new Quote("I always wanted to be somebody, but now I realize I should have been more specific.", "Lily Tomlin"),
    new Quote("I think it's wrong that only one company can make the game Monopoly", "Stephen Wright"),
    new Quote("Happiness is having a large, loving, caring, close-knit family in another city.", "George Burns"),
    new Quote("All generalizations are false, including this one.", "Mark Twain"),
    new Quote("My grandmother started walking five miles a day when she was sixty. She's ninety-seven now, and we don't know where the hell she is.", "Ellen DeGeneres"),
    new Quote("I haven't spoken to my wife in years. I didn't want to interrupt her.", "Rodney Dangerfield"),
    new Quote("You know you have a drinking problem when the bartender knows your name -- and you've never been to that bar before.", "Zach Galifianakis"),
    new Quote("No man has a good enough memory to be a successful liar", "Abraham Lincoln"),
    new Quote("I love deadlines. I like the whooshing sounds they make as they fly by.", "Douglas Adams")
  ];

  public static getQuote = (): Quote => {
    var index = Math.floor(Math.random() * QuoteManager.quotes.length);
    return QuoteManager.quotes[index];
  }

}
