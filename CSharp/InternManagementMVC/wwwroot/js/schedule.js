var username = document.getElementById("usernameFromController").innerText;
var userId;

function start() {
    getSCheduleByUsername(username, renderSchedule);
    getUserDetail(username);
    handleFormCreateSchedule();
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
        var deleteTable = document.getElementById("tableSchedules");
        if (deleteTable) {
            deleteTable.remove();
        }
        var createTable = document.createElement("table");
        createTable.setAttribute("id", "tableSchedules");
        createTable.classList.add("table");
        document.getElementById("scheduleIndex").appendChild(createTable);
        var tableSchedules = document.getElementById("tableSchedules");
        var header = tableSchedules.createTHead();
        var body = tableSchedules.createTBody();
        var rowHeader = document.createElement("tr");
        titleHeader.forEach(data => {
            var td = document.createElement("td");
            var value = document.createTextNode(data);
            if (data === "Action") {
                td.colSpan = 2;
            }
            td.appendChild(value);
            td.classList.add("bold_Text", "h3");
            rowHeader.appendChild(td);
            header.appendChild(rowHeader);
        });

        var index = 1;

        for (var i = 0; i < schedules.length; i++) {
            var tr = document.createElement("tr");
            tr.setAttribute("id", "row_" + schedules[i]["aScheduleId"]);
            var tdFirst = document.createElement("td");
            tdFirst.innerText = index;
            tr.appendChild(tdFirst);
            for (var j = 0; j < keyObject.length; j++) {
                var td = document.createElement("td");
                var value = document.createTextNode(schedules[i][keyObject[j]]);
                if (keyObject[j] === "aAttendedDate") {
                    var date = document.createTextNode(formatDate(schedules[i][keyObject[j]]));
                    td.appendChild(date);
                } else if (keyObject[j] === "aSession") {
                    var string;
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

            var tdEdit = document.createElement("td");
            var buttonEdit = document.createElement("a");
            var valueEdit = document.createTextNode("Edit");
            buttonEdit.classList.add("btn", "btn-success", "text-white", "pointerMouse");
            buttonEdit.setAttribute("onclick", "handleFormEditSchedule(" + schedules[i]['aScheduleId'] + ")");
            buttonEdit.setAttribute("data-toggle", "modal");
            buttonEdit.setAttribute("data-target", "#modalEditSchedule");
            buttonEdit.appendChild(valueEdit);
            tdEdit.appendChild(buttonEdit);

            var tdDelete = document.createElement("td");
            var buttonDelete = document.createElement("a");
            var valueDelete = document.createTextNode("Delete");
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

    var deleteButtonAddSchedule = document.getElementById("buttonAddSchedule");
    if (deleteButtonAddSchedule) {
        deleteButtonAddSchedule.remove();
    }
    var buttonAddNewSchedule = document.createElement("button");
    buttonAddNewSchedule.setAttribute("id", "buttonAddSchedule");
    buttonAddNewSchedule.classList.add("btn", "btn-primary");
    buttonAddNewSchedule.setAttribute("data-toggle", "modal");
    buttonAddNewSchedule.setAttribute("data-target", "#modalCreateSchedule");
    buttonAddNewSchedule.innerText = "New Row";
    document.getElementById("scheduleIndex").appendChild(buttonAddNewSchedule);
}

function createSchedule(data, callback) {
    var options = {
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
        console.log(sesionTable.length);

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
    var options = {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        }
    };
    var result = confirm("Want to delete?");
    if (result) {
        fetch(APIUrl + "/tschedule/deleteSchedule/" + id, options)
            .then(function () {
                getSCheduleByUsername(username, renderSchedule);
            });
    }
}

function editSchedule(id, data, callback) {
    var options = {
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
    var d = new Date(date),
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