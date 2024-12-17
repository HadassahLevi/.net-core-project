const uri = "/logIn";

const user = {
    name: document.getElementById('name'),
    password: document.getElementById('password'),
    sendBtn: document.getElementById('submit')
}

user.sendBtn.onclick = (event) => {
    event.preventDefault();

    const currUser = { name: user.name.value, password: user.password.value };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(currUser)
    })
        .then((response) => {
            return response.json();
        })
        .then((data) => {
            if (data.status == 401)
                console.log('name or password invalid');
            else {
                if (user.name.value == "דסי" && user.password.value == "2955"){
                    localStorage.setItem('link', true);
                }
                else{
                    localStorage.setItem('link', false);
                }
                localStorage.setItem('Token', data.token);
                localStorage.setItem('userId', data.id);
                location.href = "../index.html";
            }

        })
        .catch((error) => console.error('oops',error))
}