console.log('Hello World dari external file')

        let name = 'Alice';
        const age = 30;
        var city = 'New York';

        // Print variables
        console.log(name);
        console.log(age);
        console.log(city);

        // Control Structure
        if (age > 18) {  // If condition
            console.log('You are an adult');
        }

        // Function definition and execution
        function squareNumber(number) {
            return number * number;
        }
        let result = squareNumber(4);
        console.log(result);
const button = document.getElementById('newQuoteButton');
const quoteDisplay = document.getElementById('quoteDisplay');

// Array of quotes
const quotes = [
   "The best way to predict the future is to invent it.",
   "Life is 10% what happens to us and 90% how we react to it.",
   "Success is not the key to happiness. Happiness is the key to success."
];

// Add event listener to the button
button.addEventListener('click', function() {
   const randomIndex = Math.floor(Math.random() * quotes.length);
   quoteDisplay.textContent = quotes[randomIndex];
});
// async bagian
console.log('Start');
setTimeout(() => console.log('Timeout'), 0);
console.log('End');

function fetchData(callback) {
  setTimeout(() => { callback("Data received"); }, 2000);
}
fetchData((data) => console.log(data));

function fetchData2() {
  return new Promise((resolve) => setTimeout(() => resolve("Data receiveasd"), 3000));
}
fetchData2().then(data => console.log(data));

async function getData() {
  try {
    const data = await fetchData2();
    console.log(data);
  } catch (error) {
    console.error("Error:", error);
  }
}
getData();