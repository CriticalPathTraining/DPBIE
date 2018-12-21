import * as $ from "jquery"

import { Quote } from './quote';
import { QuoteManager } from './quote-manager';

import './../css/app.css'

$(() => {

  var displayNewQuote = (): void => {
    var quote: Quote = QuoteManager.getQuote();
    $("#quote").text(quote.value);
    $("#author").text(quote.author);
  };

  $("#new-quote").click(() => {
    displayNewQuote();
  });

  displayNewQuote();

});
