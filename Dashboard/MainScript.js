var randomFact = fetch("https://uselessfacts.jsph.pl/random.txt?language=en")
.then(res => res.text())
.then(data => document.getElementById("RandomFact").innerHTML = data);
getWeather();




async function getWeather(){
const response = await fetch("https://localhost:44369/api/student?username=Kasp609g");
return  response.json();
console.log(data);

const {feels_like, humidity, pressure, temp, temp_max, temp_min} = data.main;
let feels_likeString = "";
let humidityString = "";
let pressureString = "";
let tempString = "";
let temp_maxString = "";
let temp_minString = "";
document.getElementById("WeatherFeelsLike").textContent = feels_likeString.concat("Feels like: ",Math.round(feels_like - 273.15));
document.getElementById("WeatherHumidity").textContent = humidityString.concat("Humidity: ", Math.round(humidity));
document.getElementById("WeatherPressure").textContent = pressureString.concat("Pressure: ", pressure);
document.getElementById("WeatherTemp").textContent = tempString.concat("Current temp: ", Math.round(temp - 273.15));
document.getElementById("WeatherTemp_max").textContent = temp_maxString.concat("Max temp: ", Math.round(temp_max - 273.15));
document.getElementById("WeatherTemp_min").textContent = temp_minString.concat("Min temp: ", Math.round(temp_min - 273.15));

}

var kage = getWeather();


