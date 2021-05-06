function start() {
    getStudents(renderStudent);
    handleFormCreateStudent();
};

function getStudents(callback) {
    fetch(APIUrl + "/tstudent/students")
        .then(function (response) {
            return response.json();
        })
        .then(callback)
}

function createStudent(data, callback) {
    let options = {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    };
    fetch(APIUrl + "/tstudent/upsertStudent", options)
        .then(function (response) {
            return response.json();
        })
        .then(callback);
    resetForm();
}

function editStudent(id, data, callback) {
    let options = {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    };
    fetch(APIUrl + "/tstudent/updateStudent/" + id, options)
        .then(function (response) {
            return response.json();
        })
        .then(callback)
}


var titleHeader = [
    "Count",
    "Username",
    "Fullname",
    "Email",
    "Action"
];

var keyObject = [
    "aUsername",
    "aFullName",
    "aEmail",
];


function renderStudent(students) {
    if (students.length >= 0) {
        let deleteTable = document.getElementById("tableStudents");
        if (deleteTable) {
            deleteTable.remove();
        }
        let createTable = document.createElement("table");
        createTable.setAttribute("id", "tableStudents");
        createTable.classList.add("table");
        document.getElementById("studentIndex").appendChild(createTable);
        let tableStudents = document.getElementById("tableStudents");
        let header = tableStudents.createTHead();
        let body = tableStudents.createTBody();
        let rowHeader = document.createElement("tr");
        titleHeader.forEach(data => {
            let td = document.createElement("td");
            let value = document.createTextNode(data);
            if (data === "Action") {
                td.colSpan = 2;
            }
            td.appendChild(value);
            td.classList.add("bold_Text", "h3");
            rowHeader.appendChild(td);
            header.appendChild(rowHeader);
        });
        let index = 1;
        for (let i = 0; i < students.length; i++) {
            let tr = document.createElement("tr");
            tr.setAttribute("id", "row_" + students[i]["aStudentId"]);
            let tdFirst = document.createElement("td");
            tdFirst.innerText = index;
            tr.appendChild(tdFirst);
            for (let j = 0; j < keyObject.length; j++) {
                let td = document.createElement("td");
                let value = document.createTextNode(students[i][keyObject[j]]);
                if (keyObject[j] == "aUsername") {
                    let aTag = document.createElement("a");
                    aTag.setAttribute("href", "/Schedule/" + students[i][keyObject[j]]);
                    aTag.appendChild(value);
                    td.appendChild(aTag);
                } else {
                    td.appendChild(value);
                }
                tr.appendChild(td);
            }
            let tdEdit = document.createElement("td");
            let buttonEdit = document.createElement("a");
            let valueEdit = document.createTextNode("Edit");
            buttonEdit.classList.add("btn", "btn-success", "text-white", "pointerMouse");
            buttonEdit.setAttribute("onclick", "handleFormEditStudent(" + students[i]['aStudentId'] + ")");
            buttonEdit.setAttribute("data-toggle", "modal");
            buttonEdit.setAttribute("data-target", "#modalEditStudent");
            buttonEdit.appendChild(valueEdit);
            tdEdit.appendChild(buttonEdit);

            let tdDelete = document.createElement("td");
            let buttonDelete = document.createElement("a");
            let valueDelete = document.createTextNode("Delete");
            buttonDelete.classList.add("btn", "btn-danger", "pointerMouse", "text-white");
            buttonDelete.setAttribute("onclick", "handleDeleteStudent(" + students[i]['aStudentId'] + ")");
            buttonDelete.appendChild(valueDelete);
            tdDelete.appendChild(buttonDelete);

            tr.appendChild(tdEdit);
            tr.appendChild(tdDelete);
            body.appendChild(tr);
            index++;
        }
    }
    let deleteButtonAddStudent = document.getElementById("buttonAddStudent");
    if (deleteButtonAddStudent) {
        deleteButtonAddStudent.remove();
    }
    let buttonAddNewStudent = document.createElement("button");
    buttonAddNewStudent.setAttribute("id", "buttonAddStudent");
    buttonAddNewStudent.classList.add("btn", "btn-primary");
    buttonAddNewStudent.setAttribute("data-toggle", "modal");
    buttonAddNewStudent.setAttribute("data-target", "#modalCreateStudent");
    buttonAddNewStudent.innerText = "New Row";
    document.getElementById("studentIndex").appendChild(buttonAddNewStudent);

}

function handleFormCreateStudent() {
    let btnAddStudent = document.querySelector("#btnSubmitAddStudent");
    btnAddStudent.onclick = function () {
        let usernameTable = document.querySelectorAll("#tableStudents tbody tr");
        let username = document.querySelector("#usernameStudent").value;
        let fullname = document.querySelector("#fullnameStudent").value;
        let email = document.querySelector("#emailStudent").value;
        let regexEmail = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
        let check = false;
        if (username == "") {
            alert("Vui lòng nhập Username");
            document.querySelector("#usernameStudent").focus();
            check = false;
        } else if (fullname == "") {
            alert("Vui lòng nhập Fullname");
            document.querySelector("#fullnameStudent").focus();
            check = false;
        } else if (email == "") {
            alert("Vui lòng nhập Email");
            document.querySelector("#emailStudent").focus();
            check = false;
        } else {
            if (!regexEmail.test(email)) {
                alert("Email không hợp lệ");
                document.querySelector("#emailStudent").focus();
            } else {
                if (usernameTable.length > 0) {
                    for (var i = 0; i < usernameTable.length; i++) {
                        if (usernameTable[i].cells[1].innerText === username) {
                            alert("Username đã được sử dụng");
                            document.querySelector("#usernameStudent").focus();
                            check = false;
                            break;
                        } if (usernameTable[i].cells[3].innerText === email) {
                            alert("Email đã được sử dụng");
                            document.querySelector("#emailStudent").focus();
                            check = false;
                            break;
                        } else {
                            check = true;
                        }
                    }
                } else {
                    check = true;
                }
            }
        }

        if (check) {
            let data = {
                aUsername: username,
                aFullName: fullname,
                aEmail: email
            };
            createStudent(data, function () {
                getStudents(renderStudent);
            });
        }
    };
}

function handleFormEditStudent(id) {
    let dataUpdate = document.getElementById("row_" + id);
    let btnEdit = document.querySelector("#btnSubmitEditStudent");
    document.querySelector("#usernameEditStudent").value = dataUpdate.cells[1].innerText;
    document.querySelector("#fullnameEditStudent").value = dataUpdate.cells[2].innerText;
    document.querySelector("#emailEditStudent").value = dataUpdate.cells[3].innerText;
    btnEdit.onclick = function () {
        let usernameTable = document.querySelectorAll("#tableStudents tbody tr");
        let username = document.querySelector("#usernameEditStudent").value;
        let fullname = document.querySelector("#fullnameEditStudent").value;
        let email = document.querySelector("#emailEditStudent").value;
        let regexEmail = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
        let check = false;
        if (username == "") {
            alert("Vui lòng nhập Username");
            document.querySelector("#usernameEditStudent").focus();
            check = false;
        } else if (fullname == "") {
            alert("Vui lòng nhập Fullname");
            document.querySelector("#fullnameEditStudent").focus();
            check = false;
        } else if (email == "") {
            alert("Vui lòng nhập Email");
            document.querySelector("#emailEditStudent").focus();
            check = false;
        } else {
            if (!regexEmail.test(email)) {
                alert("Email không hợp lệ");
                document.querySelector("#emailEditStudent").focus();
            } else {
                if (usernameTable.length > 1) {
                    for (var i = 0; i < usernameTable.length; i++) {
                        if (usernameTable[i] === dataUpdate) {
                            continue;
                        }
                        if (usernameTable[i].cells[1].innerText === username) {
                            alert("Username đã được sử dụng");
                            document.querySelector("#usernameEditStudent").focus();
                            check = false;
                            break;
                        } else if (usernameTable[i].cells[3].innerText === email) {
                            alert("Email đã được sử dụng");
                            document.querySelector("#emailEditStudent").focus();
                            check = false;
                            break;
                        } else {
                            check = true;
                        }
                    }
                } else {
                    check = true;
                }
            }
        }
        if (check) {
            let data = {
                aStudentId: id,
                aUsername: username,
                aFullName: fullname,
                aEmail: email
            };
            editStudent(id, data, function () {
                getStudents(renderStudent);
            });
            resetForm();
        }
    };
}

function handleDeleteStudent(id) {
    let options = {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        }
    };
    let username = document.getElementById("row_" + id).cells[1].innerText;
    let result = confirm("Want to delete?");
    let check = false;
    if (result) {
        fetch(APIUrl + "/tschedule/scheduleUsername/" + username)
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                if (data.length > 0) {
                    check = false;
                } else {
                    check = true;
                }

                if (check === true) {
                    fetch(APIUrl + "/tstudent/deleteStudent/" + id, options)
                        .then(function () {
                            getStudents(renderStudent);
                        });
                } else {
                    alert("Student này đang chứa dữ liệu trong bảng Schedule không thể xóa được");
                }
            });

    }
}


function resetForm() {
    document.getElementById("closeCreateStudentModal").click();
    document.getElementById("closeEditStudentModal").click();
    document.getElementById("usernameStudent").value = "";
    document.getElementById("fullnameStudent").value = "";
    document.getElementById("emailStudent").value = "";
}

start();