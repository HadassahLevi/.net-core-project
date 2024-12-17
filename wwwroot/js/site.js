const uri = 'api/MusicalInstruments';
const uriUsers = 'api/Users';
let musics = [];


if(localStorage.getItem("Token") == null){
    location.href = "./login.html";
}

function getItems() {
    fetch(uri, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("Token")}`
        },
    })
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => {
            console.error('Unable to get items.', error);
            location.href = "./logIn.html"
        });
}
function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const addPriceTextBox = document.getElementById('add-price');
    const addElectricCheckbox = document.getElementById('add-electric').checked;
        
    const item = {
        IsElectric: addElectricCheckbox,
        name: addNameTextbox.value.trim(),
        price: addPriceTextBox.value,
        id: user.id
    };

    fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem("Token")}`
            },
            body: JSON.stringify(item)
        })
        .then(response => {
            return response.json()
        })
        .then(() => {
            getItems();
            addNameTextbox.value = '';
            addPriceTextBox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("Token")}`
        },
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = musics.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isElectric').checked = item.isElectric;
    document.getElementById('edit-price').value = item.price;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        IsElectric: document.getElementById('edit-isElectric').checked,
        name: document.getElementById('edit-name').value.trim(),
        price: document.getElementById('edit-price').value,
        userId: user.id
    };
    
    fetch(`${uri}/${itemId}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem("Token")}`
            },
            body: JSON.stringify(item)
        })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'music' : 'music kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('musics');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isElectricCheckbox = document.createElement('input');
        isElectricCheckbox.type = 'checkbox';
        isElectricCheckbox.disabled = true;
        isElectricCheckbox.checked = item.isElectric;

        let priceTextNode = document.createTextNode(item.price);
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isElectricCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(priceTextNode);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);

    });

    musics = data;
}

function createLink() {
    if (localStorage.getItem("link") == "true") {
        let link = document.createElement("a");
        link.href = "./usersList.html";
        link.innerHTML = "Users List";
        link.className = "lists";

        let tableElement = document.getElementsByTagName("table")[0];
        if (tableElement) {
            tableElement.parentNode.insertBefore(link, tableElement);
        } else {
            document.body.appendChild(link);
        }
    }
}

function editUser() {
    document.getElementById('edit-name-user').value = user.name
    document.getElementById('edit-id-user').value = user.id;
    document.getElementById('edit-password-user').value = user.password;
    document.getElementById('editUserForm').style.display = 'block';
}

// const userName.innerHTML = user.name;
function updateUser() {
    const newUser = {
        id: user.id,
        name: document.getElementById('edit-name-user').value.trim(),
        password: document.getElementById('edit-password-user').value.trim()
    };
    fetch(`${uriUsers}/${user.id}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("Token")}`
        },
        body: JSON.stringify(newUser)
    })
        .then(() =>{
            user=newUser;
            // userName.innerHTML=user.name;
        }
        )
        .catch(error => console.error('Unable to update item.', error));

closeInput('editUserForm')
    return false;
}
function createUser(response) {
    user = response;
    // userName.innerHTML = user.name;
}

function getUser() {
    const userId = localStorage.getItem("userId");
    
    fetch(`${uriUsers}/${userId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("Token")}`
        },
    })
        .then(response => {
            return response.json();})
        .then(response => createUser(response))
        .catch(error =>
            console.log(`oppst error ${error}`));
}

let user;
getUser()
getItems();
createLink();