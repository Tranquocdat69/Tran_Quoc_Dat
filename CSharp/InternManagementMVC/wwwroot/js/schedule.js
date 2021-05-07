let username = document.getElementById("usernameFromController").innerText;
let userId;
let currentPageSchedule = 1;
let rowPerPageSchedule = 10;

function start() {
    getSCheduleByUsername(username, renderSchedule);
    getUserDetail(username);
    handleFormCreateSchedule();
}

function changePageSchedule(indexPage) {
    let pageSelection = document.getElementById("selectPageNumberSchedule");
    if (pageSelection !== null) {
        pageSelection.onchange = function () {
            currentPageSchedule = pageSelection.value;
            getSCheduleByUsername(username, renderSchedule);
        }
    }
}

//nếu mà schedule trống 
function getUserDetail(username) {
    fetch(APIUrl + "/tstudent/studentFilter/?username=" + username)
        .then(function (response) {
            return response.json();
        }).then(function (data) {
            userId = data[0]["aStudentId"];
            document.getElementById("fullnameUser").innerText = data[0]["aFullName"];
        })
}

function getSCheduleByUsername(username, callback) {
    fetch(APIUrl + "/tschedule/scheduleUsername/" + username)
        .then(function (response) {
            return response.json();
        })
        .then(callback)
}

var titleHeader = [
    "Count",
    "Attended Date",
    "Session",
    "Action"
];

var keyObject = [
    "aAttendedDate",
    "aSession"
];

function renderSchedule(schedules) {
    if (schedules.length >= 0) {
        let deleteTable = document.getElementById("tableSchedules");
        if (deleteTable) {
            deleteTable.remove();
        }
        let createTable = document.createElement("table");
        createTable.setAttribute("id", "tableSchedules");
        createTable.classList.add("table");
        document.getElementById("scheduleIndex").appendChild(createTable);
        let tableSchedules = document.getElementById("tableSchedules");
        let header = tableSchedules.createTHead();
        let body = tableSchedules.createTBody();
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

        for (let i = (currentPageSchedule - 1) * rowPerPageSchedule; i < currentPageSchedule * rowPerPageSchedule; i++) {
            if (schedules[i] === undefined) {
                continue;
            }
            let tr = document.createElement("tr");
            tr.setAttribute("id", "row_" + schedules[i]["aScheduleId"]);
            let tdFirst = document.createElement("td");
            tdFirst.innerText = index;
            tr.appendChild(tdFirst);
            for (let j = 0; j < keyObject.length; j++) {
                let td = document.createElement("td");
                let value = document.createTextNode(schedules[i][keyObject[j]]);
                if (keyObject[j] === "aAttendedDate") {
                    let date = document.createTextNode(formatDate(schedules[i][keyObject[j]]));
                    td.appendChild(date);
                } else if (keyObject[j] === "aSession") {
                    let string;
                    if (schedules[i][keyObject[j]] === 0) {
                        string = document.createTextNode("Không đi");
                        td.appendChild(string);
                    }
                    if (schedules[i][keyObject[j]] === 1) {
                        string = document.createTextNode("Sáng");
                        td.appendChild(string);
                    }
                    if (schedules[i][keyObject[j]] === 2) {
                        string = document.createTextNode("Chiều");
                        td.appendChild(string);
                    }
                    if (schedules[i][keyObject[j]] === 3) {
                        string = document.createTextNode("Cả ngày");
                        td.appendChild(string);
                    }
                } else {
                    td.appendChild(value);
                }
                tr.appendChild(td);
            }

            let tdEdit = document.createElement("td");
            let buttonEdit = document.createElement("a");
            let valueEdit = document.createTextNode("Edit");
            buttonEdit.classList.add("btn", "btn-success", "text-white", "pointerMouse");
            buttonEdit.setAttribute("onclick", "handleFormEditSchedule(" + schedules[i]['aScheduleId'] + ")");
            buttonEdit.setAttribute("data-toggle", "modal");
            buttonEdit.setAttribute("data-target", "#modalEditSchedule");
            buttonEdit.appendChild(valueEdit);
            tdEdit.appendChild(buttonEdit);

            let tdDelete = document.createElement("td");
            let buttonDelete = document.createElement("a");
            let valueDelete = document.createTextNode("Delete");
            buttonDelete.classList.add("btn", "btn-danger", "pointerMouse", "text-white");
            buttonDelete.setAttribute("onclick", "handleDeleteSchedule(" + schedules[i]['aScheduleId'] + ")");
            buttonDelete.appendChild(valueDelete);
            tdDelete.appendChild(buttonDelete);

            tr.appendChild(tdEdit);
            tr.appendChild(tdDelete);
            body.appendChild(tr);
            index++;
        }
    }

    let deleteButtonAddSchedule = document.getElementById("buttonAddSchedule");
    if (deleteButtonAddSchedule) {
        deleteButtonAddSchedule.remove();
    }

    let deleteSelection = document.getElementById("selectPageNumberSchedule");
    if (deleteSelection) {
        deleteSelection.remove();
    }

    let deleteDiv = document.getElementById("buttonAndSelectSchedule");
    if (deleteDiv) {
        deleteDiv.remove();
    }

    let createDiv = document.createElement("div");
    createDiv.setAttribute("id", "buttonAndSelectSchedule");
    document.getElementById("scheduleIndex").appendChild(createDiv);
    let divButtonSelect = document.getElementById("buttonAndSelectSchedule");
    divButtonSelect.style.display = "flex";
    divButtonSelect.style.justifyContent = "space-between";

    let buttonAddNewSchedule = document.createElement("button");
    buttonAddNewSchedule.setAttribute("id", "buttonAddSchedule");
    buttonAddNewSchedule.classList.add("btn", "btn-primary");
    buttonAddNewSchedule.setAttribute("data-toggle", "modal");
    buttonAddNewSchedule.setAttribute("data-target", "#modalCreateSchedule");
    buttonAddNewSchedule.innerText = "New Row";
    createDiv.appendChild(buttonAddNewSchedule);

    if (schedules.length > 0) {
        let createSelection = document.createElement("select");
        createSelection.setAttribute("id", "selectPageNumberSchedule");
        for (var i = 1; i <= Math.ceil(schedules.length / rowPerPageSchedule); i++) {
            let option = document.createElement("option");
            option.value = i;
            option.text = "page " + i;
            option.setAttribute("id", "pageSchedule_" + i);
            createSelection.appendChild(option);
        }
        divButtonSelect.appendChild(createSelection);
    }

    if (document.getElementById("pageSchedule_" + currentPageSchedule) !== null) {
        document.getElementById("pageSchedule_" + currentPageSchedule).setAttribute("selected", true)
    } else {
        if (schedules.length > 0) {
            currentPageSchedule = currentPageSchedule - 1;
            getSCheduleByUsername(username, renderSchedule);
        }
    }
    changePageSchedule(currentPageSchedule);
}

function createSchedule(data, callback) {
    let options = {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    };
    fetch(APIUrl + "/tschedule/upsertSchedule", options)
        .then(function (response) {
            if (response.status == 400) {
                alert("Ngày tháng năm đã tồn tại");
            } else {
                currentPageSchedule = 1;
                return response.json();
            }
        })
        .then(callback)
    resetForm();
}

function handleFormCreateSchedule() {
    let btnAddSchedule = document.getElementById("btnSubmitAddSchedule");
    btnAddSchedule.onclick = function () {
        let sesionTable = document.querySelectorAll("#tableSchedules tbody tr");
        let attendedDate = document.querySelector("#attendedDateSchedule").value;
        let session = document.querySelector("input[name='sessionSchedule']:checked").value;
        let check = false;
        if (attendedDate == "") {
            document.querySelector("#attendedDateSchedule").focus();
            alert("Vui lòng chọn ngày tháng năm");
            check = false;
        } else {
            if (sesionTable.length > 0) {
                for (let i = 0; i < sesionTable.length; i++) {
                    if (formatDate(attendedDate) === sesionTable[i].cells[1].innerText) {
                        alert("Ngày tháng năm đã tồn tại");
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

        if (check) {
            let data = {
                aStudentId: userId,
                aAttendedDate: attendedDate,
                aSession: session
            }
            createSchedule(data, function () {
                getSCheduleByUsername(username, renderSchedule);
            });
        }
    }
}

function handleDeleteSchedule(id) {
    let options = {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        }
    };
    let result = confirm("Bạn chắc chắn muốn xóa?");
    if (result) {
        fetch(APIUrl + "/tschedule/deleteSchedule/" + id, options)
            .then(function () {
                getSCheduleByUsername(username, renderSchedule);
            });
    }
}

function editSchedule(id, data, callback) {
    let options = {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    };
    fetch(APIUrl + "/tschedule/updateSchedule/" + id, options)
        .then(function (response) {
            if (response.status == 400) {
                alert("Ngày tháng năm đã tồn tại");
            } else {
                return response.json();
            }
        })
        .then(callback)
    resetForm();
}

function handleFormEditSchedule(id) {
    let scheduleUpdate = document.getElementById("row_" + id);
    let btnEditSchedule = document.getElementById("btnSubmitEditSchedule");
    document.querySelector("#attendedDateEditSchedule").value = scheduleUpdate.cells[1].innerText;
    let valueSession = scheduleUpdate.cells[2].innerText;
    let idSession;
    let check = false;

    if (valueSession === "Sáng") {
        idSession = "morning";
    }
    if (valueSession === "Chiều") {
        idSession = "afternoon";
    }
    if (valueSession === "Cả ngày") {
        idSession = "allDay";
    }
    if (valueSession === "Không đi") {
        idSession = "absent";
    }
    document.getElementById(idSession).checked = true;

    btnEditSchedule.onclick = function () {
        let attendedDate = document.querySelector("#attendedDateEditSchedule").value;
        let session = document.querySelector("input[name='sessionEditSchedule']:checked").value;
        let sesionTable = document.querySelectorAll("#tableSchedules tbody tr");
        if (sesionTable.length > 1) {
            for (let i = 0; i < sesionTable.length; i++) {
                if (sesionTable[i] === scheduleUpdate) {
                    continue;
                }
                if (formatDate(attendedDate) === sesionTable[i].cells[1].innerText) {
                    alert("Ngày tháng năm đã tồn tại");
                    check = false;
                    break;
                } else {
                    check = true;
                }
            }
        } else {
            check = true;
        }
        if (check) {
            let data = {
                aStudentId: userId,
                aAttendedDate: attendedDate,
                aSession: session
            };
            editSchedule(id, data, function () {
                getSCheduleByUsername(username, renderSchedule);
            });
        }
    }
}


function resetForm() {
    document.getElementById("closeCreateScheduleModal").click();
    document.getElementById("closeEditScheduleModal").click();
    document.getElementById("attendedDateSchedule").value = "";
    document.getElementById("checkedSessionSchedule").checked = true;
}

function formatDate(date) {
    let d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}

start();