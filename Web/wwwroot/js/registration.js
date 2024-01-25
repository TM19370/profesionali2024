function createClient() {
    let firstName = document.getElementById('firstName').value;
    alert(firstName);

    const Http = new XMLHttpRequest();
    const url = 'http://192.168.147.50\training';
    Http.open("GET", url, false, "intern", "0000");
    Http.send();

    Http.onreadystatechange = (e) => {
        console.log(Http.responseText);  // => получим массив данных в формате JSON
    }

}